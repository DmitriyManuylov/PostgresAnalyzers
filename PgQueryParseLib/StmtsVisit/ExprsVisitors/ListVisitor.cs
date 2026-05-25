using PgQuery;
using PgQueryAnalyzerLib.AnalyzeContext;
using PgQueryAnalyzerLib.StmtsVisit.StmtsVisitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryAnalyzerLib.StmtsVisit.ExprsVisitors
{
    public static partial class ExprVisitor
    {
        private static void VisitList(List list, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(list);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessList_DirectTraversal(node);

            if (list.Items is not null)
            {
                foreach (var item in list.Items)
                {
                    VisitExpr(item, context);
                }
            }

            context.PgTreeWalker.ProcessList_ReverseTraversal(node);
        }
    }
}
