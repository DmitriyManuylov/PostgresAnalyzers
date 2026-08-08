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
        private void VisitAlterTableCmd(AlterTableCmd alterTableCmd, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(alterTableCmd);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessAlterTableCmd_DirectTraversal(node);

            VisitExpr(alterTableCmd.Def, context);

            context.PgTreeWalker.ProcessAlterTableCmd_ReverseTraversal(node);
        }
    }
}
