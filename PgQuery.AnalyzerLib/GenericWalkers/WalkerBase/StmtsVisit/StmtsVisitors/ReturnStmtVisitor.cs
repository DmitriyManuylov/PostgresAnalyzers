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
        private void VisitReturnStatement(PLpgSQL_stmt_return returnStmt, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(returnStmt);

            if(returnStmt is null)
            {
                throw new ArgumentNullException();
            }

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessReturnStmt_DirectTraversal(node);

            if (returnStmt.Expr is not null)
            {
                SetStmtParseTree(returnStmt.Expr.PLpgSQLExpr);

                VisitExpr(returnStmt.Expr.PLpgSQLExpr.ParsedStmt.Stmt, context);
            }

            context.PgTreeWalker.ProcessReturnStmt_ReverseTraversal(node);
        }
    }
}
