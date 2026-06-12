using Analyzers;
using DataChangeAnalyzer.Models.DBModels;
using DMLOpsAnalyzer.Analyzer;
using PgQuery;
using PgQuery.AnalyzerLib.Services.Models.DbModels;
using PgQuery.AnalyzerLib.Services.Models.DbModels.PlainModels;
using PgQuery.Analyzers.Models.ParametersTypeCastAnalyzer;
using PgQueryAnalyzerLib;
using PgQueryAnalyzerLib.AnalyzeContext;
using PgQueryAnalyzerLib.GenericWalkers;
using PgQueryAnalyzerLib.GenericWalkers.Models;
using PgQueryAnalyzerLib.Services;
using PgQueryAnalyzerLib.Services.Models.DbModels.PlainModels;
using PgQueryAnalyzerLib.StmtsVisit.ExprsVisitors;
using PgQueryAnalyzerLib.StmtsVisit.StmtsVisitors;
using PgQueryParser;
using PgQueryParser.CustomExceptions;
using System.Configuration;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AnalyzeManagers
{
    public class AnalyzeManager
    {
        private static List<DBFunctionPlainModel> Functions;
        private static Dictionary<string, DBFunctionPlainModel> FunctionsDictionary;
        private static HashSet<TableModel> Tables;

        private List<StmtsProcessingContext> ContextList;
        private List<GenericPgTreeWalker> PgTreeWalkerList;
        private List<Type> analyzersTypes;

        string parsedStmtJson;
        private ParseResult parsedPgSqlExprs;
        private List<PLpgSQL_stmt> parsedPlPgSqlStmts;
        private List<string> ParametersList;
        private string stmtType;

        public bool IsResultSet { get; private set; } = false;

        private string query;
        private string rewritedQuery;

        static AnalyzeManager()
        {
            string functionsDefFile = ConfigurationManager.AppSettings.Get("FunctionsDefinitions");
            string columnsDefFile = ConfigurationManager.AppSettings.Get("Columns");
            string indicesDefFile = ConfigurationManager.AppSettings.Get("Indices");

            functionsDefFile = string.IsNullOrWhiteSpace(functionsDefFile) ? "FunctionsDefinitions.json" : functionsDefFile;
            columnsDefFile = string.IsNullOrWhiteSpace(columnsDefFile) ? "Columns.json" : columnsDefFile;
            indicesDefFile = string.IsNullOrWhiteSpace(indicesDefFile) ? "Indices.json" : indicesDefFile;

            List<DBFunctionPlainModel> functions = default;
            List<ColumnPlainModel> columns = default;
            List<IndexPlainModel> indices = default;

            DbEntitiesService dbEntitiesService = new DbEntitiesService();

            if (functionsDefFile is not null && File.Exists(functionsDefFile))
            {
                string fileText = File.ReadAllText(functionsDefFile);
                functions = JsonSerializer.Deserialize<List<DBFunctionPlainModel>>(fileText);
            }
            
            if (columnsDefFile is not null && File.Exists(columnsDefFile))
            {
                string fileText = File.ReadAllText(columnsDefFile);

                columns = JsonSerializer.Deserialize<List<ColumnPlainModel>>(fileText);
            }

            if (indicesDefFile is not null && File.Exists(indicesDefFile))
            {
                string fileText = File.ReadAllText(indicesDefFile);

                indices = JsonSerializer.Deserialize<List<IndexPlainModel>>(fileText);
            }

            if (columns is null)
            {
                try
                {
                    columns = Task.Run(() => dbEntitiesService.DownloadDbColumnsAsync()).GetAwaiter().GetResult();

                    string json = JsonSerializer.Serialize(columns);

                    File.WriteAllText(columnsDefFile, json);
                }
                catch
                {

                }
            }

            if (indices is null)
            {
                try
                {
                    indices = Task.Run(() => dbEntitiesService.DownloadDbIndicesAsync()).GetAwaiter().GetResult();

                    string json = JsonSerializer.Serialize(indices);

                    File.WriteAllText(indicesDefFile, json);
                }
                catch
                {

                }
            }

            if (columns is not null && indices is not null)
            {
                SetTables(columns, indices);
            }

            if (functions is null)
            {
                try
                {
                    functions = Task.Run(() => dbEntitiesService.DownloadDBFunctionsAsync()).GetAwaiter().GetResult();

                    string json = JsonSerializer.Serialize(functions);

                    File.WriteAllText(functionsDefFile, json);
                }
                catch
                {

                }
            }

            Dictionary<string, DBFunctionPlainModel> functionsDictionary = new Dictionary<string, DBFunctionPlainModel>();

            functions.ForEach(f => functionsDictionary.TryAdd($"{f.NspName}.{f.FuncName}", f));

            Functions = functions;
            FunctionsDictionary = functionsDictionary;
        }

        public AnalyzeManager(string queryText)
        {
            query = queryText;
            (this.rewritedQuery, this.ParametersList) = this.RewriteParameters(queryText);
            ParseQuery();

            switch (this.stmtType)
            {
                case "pgsql":
                    ContextList = new List<StmtsProcessingContext>(parsedPgSqlExprs.Stmts.Count);
                    PgTreeWalkerList = new List<GenericPgTreeWalker>(parsedPgSqlExprs.Stmts.Count);
                    break;

                case "plpgsql":
                    ContextList = new List<StmtsProcessingContext>(parsedPlPgSqlStmts.Count);
                    PgTreeWalkerList = new List<GenericPgTreeWalker>(parsedPlPgSqlStmts.Count);
                    break;
            }

            for (int i = 0; i < ContextList!.Capacity; i++)
            {
                var context = new StmtsProcessingContext(this.ParametersList);
                context.DBFunctionList = Functions;
                context.DBFunctionDictionary = FunctionsDictionary;
                context.DBTablesList = Tables;
                var walker = new GenericPgTreeWalker(context);
                ContextList.Add(context);

                PgTreeWalkerList!.Add(walker);
            }

            analyzersTypes = new List<Type>();

        }

        public void AddAnalyzer<TAnalyzer>() where TAnalyzer: GenericPgTreeWalkerBase, new()
        {
            analyzersTypes.Add(typeof(TAnalyzer));

            for (int i = 0; i < this.PgTreeWalkerList.Count; i++)
            {
                TAnalyzer analyzer = new TAnalyzer();
                analyzer.Context = this.ContextList[i];
                this.PgTreeWalkerList[i].AddWalker(analyzer);
            }
        }

        public void AddDMLOperationsAnalyzer()
        {
            AddAnalyzer<DMLAnalyzer>();
        }

        public void AddParametersTypeCastAnalyzer()
        {
            AddAnalyzer<ParametersTypeCastAnalyzer>();
        }

        public void Analyze()
        {

            if (!analyzersTypes.Any())
            {
                throw new Exception("Список анализаторов пуст");
            }

            for (int i = 0; i < ContextList.Count; i++)
            {
                var context = ContextList[i];
                switch (stmtType)
                {
                    case "pgsql":
                        var expr = parsedPgSqlExprs.Stmts[i].Stmt;
                        ExprVisitor.VisitExpr(expr, context);
                        break;

                    case "plpgsql":
                        var stmt = parsedPlPgSqlStmts[i];
                        StmtVisitor.VisitStmt(stmt, context);
                        break;
                }
            }

            IsResultSet = true;
        }

        public List<TPgTreeWalker> GetAnalyzerByType<TPgTreeWalker>() where TPgTreeWalker : GenericPgTreeWalkerBase
        {
            var result = this.ContextList.Select(item => item.GetTreeWalkerByType<TPgTreeWalker>()).ToList();

            return result;
        }

        public List<AnalyzeTree<DMLAnalyzeNode>> GetDMLOperationsResult()
        {
            ThrowExceptionIfNotSetResult();

            var analyzeResult = GetAnalyzerByType<DMLAnalyzer>().Select(item => item.GetResult()).ToList();

            return analyzeResult;
        }

        public List<List<ParameterTypeCastAnalyzeModel>> GetParameterTypeCastAnalyzeResult()
        {
            ThrowExceptionIfNotSetResult();

            var analyzeResult = GetAnalyzerByType<ParametersTypeCastAnalyzer>().Select(item => item.GetResult()).ToList();

            return analyzeResult;
        }

        private void ThrowExceptionIfNotSetResult()
        {
            if (!IsResultSet)
            {
                throw new Exception();
            }
        }

        private void ParseQuery()
        {
            var parser = new PostgreSqlQueryParser();

            bool tryPlPgSql = false;

            try
            {
                parsedStmtJson = parser.GetQueryParseTree(rewritedQuery);

                parsedPgSqlExprs = ParseResult.Parser.ParseJson(parsedStmtJson);

                if (new List<Node.NodeOneofCase>()
                {
                    Node.NodeOneofCase.DoStmt,
                    Node.NodeOneofCase.CreateFunctionStmt }
                .Contains(parsedPgSqlExprs.Stmts.FirstOrDefault().Stmt.NodeCase))
                {
                    tryPlPgSql = true;
                }

                if (!tryPlPgSql)
                {
                    stmtType = "pgsql";
                }
            }

            catch (Exception ex)
            {
                throw;
            }

            if (!tryPlPgSql)
            {
                return;
            }

            try
            {
                parsedStmtJson = parser.GetPlPgQueryJsonParseTree(rewritedQuery);
                List<JsonDocument> list = JsonSerializer.Deserialize<List<JsonDocument>>(parsedStmtJson);
                parsedPlPgSqlStmts = list.Select(item => PLpgSQL_stmt.Parser.ParseJson(item.RootElement.ToString())).ToList();
                stmtType = "plpgsql";
            }
            catch (Exception)
            {
                throw;
            }
        }

        private (string RewritedText, List<string> ParamList) RewriteParameters(string queryText)
        {
            string patternColon = @"(?:(?:(?<!:):(?![:=]))|@)\w+";

            Regex regex = new Regex(patternColon);

            MatchCollection matches = regex.Matches(queryText);

            List<string> parameters = matches.Select(m => m.Value.Substring(1)).Distinct().ToList();

            string text = queryText;

            foreach (Match match in matches.OrderByDescending(item => item.Value.Length))
            {
                text = text.Replace(match.Value, $"${parameters.IndexOf(match.Value.Substring(1)) + 1}");

            }

            return (text, parameters);
        }

        private static HashSet<TableModel> SetTables(List<ColumnPlainModel> columnsPlainModels, List<IndexPlainModel> indexPlainModels)
        {
            var tables = columnsPlainModels.
                Where(item => item.SchemaName is not null && item.TableName is not null)
                .GroupBy(item => new
            {
                item.SchemaName,
                item.TableName,
            }).
            Select(group =>
            {
                var table = new TableModel(group.Key.TableName, group.Key.SchemaName);

                table.Columns = group.Select(item => new PgQuery.AnalyzerLib.Services.Models.DbModels.ColumnModel
                {
                    ColumnName = item.ColumnName,
                    TypeName = item.TypeName,
                    TypeMode = item.TypeMode,
                }).DistinctBy(distinctItem => distinctItem.ColumnName).ToList();

                var tableIndices = indexPlainModels.Where(index => index.SchemaName == table.DBSchemaModel.Name && index.TableName == table.Name);

                table.Indices = tableIndices
                .GroupBy(group => group.IndexName)
                .Select(item =>
                {
                    var firstItem = item.FirstOrDefault();

                    var index = new IndexModel
                    {
                        IndexName = item.Key,
                        IsUnique = firstItem.IsUnique,
                        IndexColsCount = firstItem.IndexColsCount,
                        IndexExpressions = firstItem.IndexExpressions,
                        IndexKeyColsCount = firstItem.IndexKeyColsCount,
                        IndexWhereClause = firstItem.IndexWhereClause,
                        FullIndexDefinition = firstItem.FullIndexDefinition,
                        Columns = table.Columns.Where(column => item.Any(indexItemInner => column.ColumnName == indexItemInner.ColumnName)).ToList(),
                        Table = table
                    };

                    return index;
                }).ToList();

                table.Columns.ForEach(col =>
                    col.ColumnIndices =
                        table.Indices
                        .Where(index => index.Columns
                            .Any(indexCol => indexCol == col))
                        .ToList());

                return table;

            }).ToHashSet();

            Tables = tables;

            return tables;
        }
    }
}
