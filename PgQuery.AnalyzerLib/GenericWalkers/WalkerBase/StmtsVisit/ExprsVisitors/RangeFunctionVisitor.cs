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
        private void VisitRangeFunction(RangeFunction rangeFunction, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(rangeFunction);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessRangeFunction_DirectTraversal(node);

            if (rangeFunction.Functions?.Count < 1)
            {
                return;
            }

            var funcCall = rangeFunction.Functions![0].List.Items[0];

            if (funcCall.FuncCall is null)
            {
                return;
            }

            VisitExpr(funcCall, context);

            context.PgTreeWalker.ProcessRangeFunction_ReverseTraversal(node);
        }
    }
}
