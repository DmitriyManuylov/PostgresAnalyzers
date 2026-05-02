using PgQuery;
using PgQueryParseLib.AnalyzeContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryParseLib.StmtsVisit.ExprsVisitors
{
    public static partial class ExprVisitor
    {
        public static void VisitRangeFunction(RangeFunction rangeFunction, StmtsProcessingContext context)
        {
            if (rangeFunction is null)
            {
                throw new ArgumentNullException(nameof(rangeFunction));
            }

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
