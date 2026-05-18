using PgQuery;
using PgQueryAnalyzerLib.AnalyzeContext;
using PgQueryAnalyzerLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryAnalyzerLib.StmtsVisit.StmtsVisitors
{
    public static partial class StmtVisitor
    {
        private static PostgreSqlQueryParser _parser = new PostgreSqlQueryParser();

        public static void VisitStmt(PLpgSQL_stmt stmt, StmtsProcessingContext context)
        {
            if (stmt is null)
            {
                throw new ArgumentNullException();
            }

            var node = new PgGenericNode()
            {
                PLpgSQL_Stmt = stmt
            };

            context.ProcessDirectTraversal(node);

            switch (stmt.StmtCase)
            {
                case PLpgSQL_stmt.StmtOneofCase.PLpgSQLFunction:
                    VisitPLpgSQLFunction(stmt.PLpgSQLFunction, context);
                    break;
                case PLpgSQL_stmt.StmtOneofCase.PLpgSQLStmtAssign:
                    VisitAssignStatement(stmt.PLpgSQLStmtAssign, context);
                    break;
                case PLpgSQL_stmt.StmtOneofCase.PLpgSQLStmtIf:
                    VisitIfStatement(stmt.PLpgSQLStmtIf, context);
                    break;
                case PLpgSQL_stmt.StmtOneofCase.PLpgSQLStmtPerform:
                    VisitPerformStatement(stmt.PLpgSQLStmtPerform, context);
                    break;
                case PLpgSQL_stmt.StmtOneofCase.PLpgSQLStmtFors:
                    VisitForsStatement(stmt.PLpgSQLStmtFors, context);
                    break;
                case PLpgSQL_stmt.StmtOneofCase.PLpgSQLStmtFori:
                    VisitForiStatement(stmt.PLpgSQLStmtFori, context);
                    break;
                case PLpgSQL_stmt.StmtOneofCase.PLpgSQLStmtReturn:
                    VisitReturnStatement(stmt.PLpgSQLStmtReturn, context);
                    break;
                case PLpgSQL_stmt.StmtOneofCase.PLpgSQLStmtReturnNext:
                    VisitReturnNextStatement(stmt.PLpgSQLStmtReturnNext, context);
                    break;
                case PLpgSQL_stmt.StmtOneofCase.PLpgSQLStmtExecsql:
                    VisitExecSqlStmt(stmt.PLpgSQLStmtExecsql, context);
                    break;
                case PLpgSQL_stmt.StmtOneofCase.PLpgSQLStmtBlock:
                    VisitBlockStatement(stmt.PLpgSQLStmtBlock, context);
                    break;
                case PLpgSQL_stmt.StmtOneofCase.PLpgSQLStmtRaise:
                    VisitRaiseStatement(stmt.PLpgSQLStmtRaise, context);
                    break;
                case PLpgSQL_stmt.StmtOneofCase.PLpgSQLStmtCase:
                    VisitStmtCase(stmt.PLpgSQLStmtCase, context);
                    break;
                default:
                    break;
            }

            context.ProcessReverseTraversal(node);

        }

        private static void SetStmtParseTree(PLpgSQL_expr? expr)
        {
            if (expr is null)
                return;

            var parsedStmt = _parser
                .GetQueryProtobufParseTreeWithOptions<ParseResult>(
                    expr.Query,
                    (int)expr.ParseMode);

            expr.ParsedStmt = parsedStmt?.Stmts[0];

        }
    }
}
