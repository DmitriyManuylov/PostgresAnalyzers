using PgQuery;
using PgQueryAnalyzerLib.AnalyzeContext;
using PgQueryAnalyzerLib.StmtsVisit.ExprsVisitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryAnalyzerLib.StmtsVisit.StmtsVisitors
{
    public static partial class StmtVisitor
    {
        private static void VisitStmtCase(PLpgSQL_stmt_case stmt_Case, StmtsProcessingContext context)
        {
            if(stmt_Case is null)
            {
                throw new ArgumentNullException();
            }

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessCaseStmt_DirectTraversal(node);

            if (stmt_Case.TExpr is not null)
            {
                SetStmtParseTree(stmt_Case.TExpr.PLpgSQLExpr);
                ExprVisitor.VisitExpr(stmt_Case.TExpr.PLpgSQLExpr.ParsedStmt.Stmt, context);
            }

            if (stmt_Case.CaseWhenList is not null)
            {
                foreach (var when in stmt_Case.CaseWhenList)
                {
                    SetStmtParseTree(when.PLpgSQLCaseWhen.Expr.PLpgSQLExpr);

                    var expr = when.PLpgSQLCaseWhen.Expr.PLpgSQLExpr.ParsedStmt.Stmt;
                    if (expr is not null)
                    {
                        ExprVisitor.VisitExpr(expr, context);
                    }

                    foreach(var stmt in when.PLpgSQLCaseWhen.Stmts)
                    {
                        VisitStmt(stmt, context);
                    }
                }
            }

            if (stmt_Case.HaveElse)
            {
                foreach(var stmt in stmt_Case.ElseStmts)
                {
                    VisitStmt(stmt, context);
                }
            }

            context.PgTreeWalker.ProcessCaseStmt_ReverseTraversal(node);
        }
    }
}
