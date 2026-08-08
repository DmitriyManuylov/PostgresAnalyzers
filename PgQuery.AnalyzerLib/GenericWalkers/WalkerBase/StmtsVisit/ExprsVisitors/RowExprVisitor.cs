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
        private void VisitRowExpr(RowExpr rowExpr, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(rowExpr);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessRowExpr_DirectTraversal(node);

            if (rowExpr.Xpr is not null)
            {
                VisitExpr(rowExpr.Xpr, context);
            }

            if (rowExpr.Args is not null)
            {
                foreach (var arg in rowExpr.Args)
                {
                    VisitExpr(arg, context);
                }
            }

            context.PgTreeWalker.ProcessRowExpr_ReverseTraversal(node);
        }
    }
}
