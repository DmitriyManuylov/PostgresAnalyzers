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
        private void VisitBlockStatement(PLpgSQL_stmt_block blockStmt, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(blockStmt);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessBlockStmt_DirectTraversal(node);

            foreach (var stmt in blockStmt.Body)
            {
                VisitStmt(stmt, context);
            }

            context.PgTreeWalker.ProcessBlockStmt_ReverseTraversal(node);

        }
    }
}
