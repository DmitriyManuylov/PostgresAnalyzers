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
        private void VisitList(List list, StmtsProcessingContext context)
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
