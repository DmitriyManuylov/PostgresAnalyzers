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
        private void VisitPerformStatement(PLpgSQL_stmt_perform performStmt, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(performStmt);

            SetStmtParseTree(performStmt.Expr.PLpgSQLExpr);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessPerformStmt_DirectTraversal(node);

            VisitExpr(performStmt.Expr.PLpgSQLExpr.ParsedStmt.Stmt, context);

            context.PgTreeWalker.ProcessPerformStmt_ReverseTraversal(node);
        }
    }
}
