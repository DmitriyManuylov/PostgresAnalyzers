using PgQuery;
using PgQueryAnalyzerLib.AnalyzeContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQuery.AnalyzerLib.GenericWalkers.WalkerBase
{
    public abstract partial class GenericPgTreeWalkerBase
    {
        private void VisitForiStatement(PLpgSQL_stmt_fori foriStmt, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(foriStmt);

            SetStmtParseTree(foriStmt?.Lower?.PLpgSQLExpr);
            SetStmtParseTree(foriStmt?.Upper?.PLpgSQLExpr);
            SetStmtParseTree(foriStmt?.Step?.PLpgSQLExpr);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessForiStmt_DirectTraversal(node);

            VisitExpr(foriStmt?.Lower?.PLpgSQLExpr.ParsedStmt.Stmt, context);
            VisitExpr(foriStmt?.Upper?.PLpgSQLExpr.ParsedStmt.Stmt, context);
            VisitExpr(foriStmt?.Step?.PLpgSQLExpr.ParsedStmt.Stmt, context);

            foreach (PLpgSQL_stmt stmt in foriStmt!.Body)
            {
                VisitStmt(stmt, context);
            }

            context.PgTreeWalker.ProcessForiStmt_ReverseTraversal(node);
        }
    }
}
