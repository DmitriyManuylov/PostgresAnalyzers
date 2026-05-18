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
        private static void VisitRangeVar(RangeVar rangeVar, StmtsProcessingContext context)
        {
            if(rangeVar is null)
            {
                throw new ArgumentNullException();
            }

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessRangeVar_DirectTraversal(node);

            context.PgTreeWalker.ProcessRangeVar_ReverseTraversal(node);
        }
    }
}
