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
        private void VisitRenameStmt(RenameStmt renameStmt, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(renameStmt);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessRenameStmt_DirectTraversal(node);

            if (renameStmt.Object is not null)
            {
                VisitExpr(renameStmt.Object, context);
            }

            context.PgTreeWalker.ProcessRenameStmt_ReverseTraversal(node);
        }
    }
}
