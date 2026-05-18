using PgQuery;
using PgQueryAnalyzerLib.AnalyzeContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryAnalyzerLib.StmtsVisit.ExprsVisitors
{
    public static partial class ExprVisitor
    {
        private static void VisitRangeSubselect(RangeSubselect rangeSubselect, StmtsProcessingContext context)
        {
            if (rangeSubselect is null)
            {
                throw new ArgumentNullException();
            }

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
