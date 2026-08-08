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
        private void VisitRangeVar(RangeVar rangeVar, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(rangeVar);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessRangeVar_DirectTraversal(node);

            context.PgTreeWalker.ProcessRangeVar_ReverseTraversal(node);
        }
    }
}
