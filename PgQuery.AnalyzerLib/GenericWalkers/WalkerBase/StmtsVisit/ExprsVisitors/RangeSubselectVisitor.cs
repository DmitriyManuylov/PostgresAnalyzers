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
        private void VisitRangeSubselect(RangeSubselect rangeSubselect, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(rangeSubselect);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessRangeSubselect_DirectTraversal(node);

            if(rangeSubselect.Subquery is not null)
            {
                VisitExpr(rangeSubselect.Subquery, context);
            }

            context.PgTreeWalker.ProcessRangeSubselect_ReverseTraversal(node);
        }
    }
}
