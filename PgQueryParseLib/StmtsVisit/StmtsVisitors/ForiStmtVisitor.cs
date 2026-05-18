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
        private static void VisitForiStatement(PLpgSQL_stmt_fori foriStmt, StmtsProcessingContext context)
        {
            if (foriStmt is null)
            {
                throw new ArgumentNullException();
            }

            SetStmtParseTree(foriStmt?.Lower?.PLpgSQLExpr);
            SetStmtParseTree(foriStmt?.Upper?.PLpgSQLExpr);
            SetStmtParseTree(foriStmt?.Step?.PLpgSQLExpr);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessForiStmt_DirectTraversal(node);

            ExprVisitor.VisitExpr(foriStmt?.Lower?.PLpgSQLExpr.ParsedStmt.Stmt, context);
            ExprVisitor.VisitExpr(foriStmt?.Upper?.PLpgSQLExpr.ParsedStmt.Stmt, context);
            ExprVisitor.VisitExpr(foriStmt?.Step?.PLpgSQLExpr.ParsedStmt.Stmt, context);

            foreach (PLpgSQL_stmt stmt in foriStmt!.Body)
            {
                VisitStmt(stmt, context);
            }

            context.PgTreeWalker.ProcessForiStmt_ReverseTraversal(node);
        }
    }
}
