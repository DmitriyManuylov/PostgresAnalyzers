using Analyzers;
using Analyzers.Models.ParametersTypeCastAnalyzer;
using DMLOpsAnalyzer.Analyzer;
using PgQuery;
using PgQueryAnalyzerLib;
using PgQueryAnalyzerLib.AnalyzeContext;
using PgQueryAnalyzerLib.GenericWalkers;
using PgQueryAnalyzerLib.GenericWalkers.Models;
using PgQueryAnalyzerLib.Services;
using PgQueryAnalyzerLib.Services.Models.DbModels.PlainModels;
using PgQueryAnalyzerLib.StmtsVisit.ExprsVisitors;
using PgQueryAnalyzerLib.StmtsVisit.StmtsVisitors;
using PgQueryParser;
using System.Configuration;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AnalyzeManagers
{
    public class AnalyzeManager
    {
        private static List<DBFunctionPlainModel> Functions;
        private static Dictionary<string, DBFunctionPlainModel> FunctionsDictionary;

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
            string functionsDefFile = ConfigurationManager.AppSettings.Get("FunctionsDefinitions") ?? "FunctionsDefinitions.json";

            List<DBFunctionPlainModel> functions = default;
            if (functionsDefFile is not null && File.Exists(functionsDefFile))
            {
                string fileText = File.ReadAllText(functionsDefFile);
                functions = JsonSerializer.Deserialize<List<DBFunctionPlainModel>>(fileText);
            }
            DbEntitiesService dbEntitiesService = new DbEntitiesService();

            if (functions is null)
            {
                functions = Task.Run(() => dbEntitiesService.DownloadDBFunctionsAsync()).GetAwaiter().GetResult();

                string json = JsonSerializer.Serialize(functions);
                string file = !string.IsNullOrWhiteSpace(functionsDefFile) ? functionsDefFile : "FunctionsDefinitions.json";
                File.WriteAllText(functionsDefFile, json);
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

            try
            {
                parsedStmtJson = parser.GetQueryParseTree(rewritedQuery);

                parsedPgSqlExprs = ParseResult.Parser.ParseJson(parsedStmtJson);

                if (parsedPgSqlExprs.Stmts.FirstOrDefault().Stmt.NodeCase == Node.NodeOneofCase.DoStmt)
                {
                    throw new Exception();
                }

                stmtType = "pgsql";
            }
            catch (Exception ex)
            {
                parsedStmtJson = parser.GetPlPgQueryJsonParseTree(rewritedQuery);
                List<JsonDocument> list = JsonSerializer.Deserialize<List<JsonDocument>>(parsedStmtJson);
                parsedPlPgSqlStmts = list.Select(item => PLpgSQL_stmt.Parser.ParseJson(item.RootElement.ToString())).ToList();
                stmtType = "plpgsql";
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
    }
}
