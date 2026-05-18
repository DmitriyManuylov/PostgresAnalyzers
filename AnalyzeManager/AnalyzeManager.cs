using PgQuery;
using PgQueryAnalyzerLib;
using PgQueryAnalyzerLib.AnalyzeContext;
using PgQueryAnalyzerLib.GenericWalkers;
using PgQueryAnalyzerLib.GenericWalkers.Models;
using PgQueryAnalyzerLib.StmtsVisit.ExprsVisitors;
using PgQueryAnalyzerLib.StmtsVisit.StmtsVisitors;
using System.Text.Json;

namespace AnalyzeManagers
{
    public class AnalyzeManager
    {
        private StmtsProcessingContext Context { get; set; }

        public bool IsResultSet { get; private set; } = false;

        public AnalyzeManager()
        {
            Context = new StmtsProcessingContext();
        }

        public void AddDMLOperationsAnalyzer()
        {
            DMLAnalyzer analyzer = new DMLAnalyzer(this.Context);

            this.Context.PgTreeWalker = new GenericPgTreeWalker(this.Context);
            this.Context.PgTreeWalker.AddWalker(analyzer);
        }

        public void Analyze(string queryText)
        {
            if (!Context.PgTreeWalker.IsWalkerListNotEmpty())
            {
                throw new Exception("Список анализаторов пуст");
            }

            var parser = new PostgreSqlQueryParser();

            string parsedStmtJson;

            string stmtType = null;
            ParseResult parsedExpr = null;
            List<PLpgSQL_stmt> parsedStmts;

            try
            {
                parsedStmtJson = parser.GetQueryParseTree(queryText);

                parsedExpr = ParseResult.Parser.ParseJson(parsedStmtJson);

                if (parsedExpr.Stmts.FirstOrDefault().Stmt.NodeCase == Node.NodeOneofCase.DoStmt)
                {
                    throw new Exception();
                }

                stmtType = "pgsql";
            }
            catch (Exception ex)
            {
                parsedStmtJson = parser.GetPlPgQueryJsonParseTree(queryText);

                stmtType = "plpgsql";
            }

            switch (stmtType)
            {
                case "pgsql":
                    if (parsedExpr is null)
                    {
                        throw new NullReferenceException();
                    }
                    ExprVisitor.VisitExpr(parsedExpr.Stmts.FirstOrDefault().Stmt, this.Context);
                    break;
                case "plpgsql":
                    List<JsonDocument> list = JsonSerializer.Deserialize<List<JsonDocument>>(parsedStmtJson);
                    parsedStmts = list.Select(item => PLpgSQL_stmt.Parser.ParseJson(item.RootElement.ToString())).ToList();
                    foreach (var stmt in parsedStmts)
                    {
                        StmtVisitor.VisitStmt(stmt, this.Context);
                    }
                    break;
            }

            IsResultSet = true;
        }

        public TPgTreeWalker GetAnalyzerByType<TPgTreeWalker>() where TPgTreeWalker : GenericPgTreeWalkerBase
        {
            var result = this.Context.GetTreeWalkerByType<TPgTreeWalker>();

            return result;
        }

        public AnalyzeTree<DMLAnalyzeNode> GetDMLOperationsResult()
        {
            var analyzer = GetAnalyzerByType<DMLAnalyzer>();

            return analyzer.GetResult();
        }
    }
}
