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
        private void VisitResTarget(ResTarget  resTarget, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(resTarget);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessResTarget_DirectTraversal(node);

            if (resTarget.Val is not null)
            {
                VisitExpr(resTarget.Val, context);
            }

            context.PgTreeWalker.ProcessResTarget_ReverseTraversal(node);
        }
    }
}
