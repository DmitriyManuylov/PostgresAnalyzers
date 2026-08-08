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
        private void VisitReturnNextStatement(PLpgSQL_stmt_return_next returnNextStmt, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(returnNextStmt);

            if(returnNextStmt is null)
            {
                throw new ArgumentNullException();
            }

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessReturnNextStmt_DirectTraversal(node);

            SetStmtParseTree(returnNextStmt.Expr.PLpgSQLExpr);

            VisitExpr(returnNextStmt.Expr.PLpgSQLExpr.ParsedStmt.Stmt, context);

            context.PgTreeWalker.ProcessReturnNextStmt_ReverseTraversal(node);
        }
    }
}
