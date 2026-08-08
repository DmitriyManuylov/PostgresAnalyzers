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
        private void VisitRaiseStatement(PLpgSQL_stmt_raise raiseStmt, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(raiseStmt);

            if (raiseStmt is null)
            {
                throw new ArgumentNullException();
            }

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessRaiseStmt_DirectTraversal(node);

            foreach(var item in raiseStmt.Params)
            {
                SetStmtParseTree(item.PLpgSQLExpr);

                VisitExpr(item.PLpgSQLExpr.ParsedStmt.Stmt, context);
            }

            context.PgTreeWalker.ProcessRaiseStmt_ReverseTraversal(node);
        }
    }
}
