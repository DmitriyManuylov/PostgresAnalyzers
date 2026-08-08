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
        private void VisitBoolExpr(BoolExpr boolExpr, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(boolExpr);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessBoolExpr_DirectTraversal(node);

            if (boolExpr.Args is not null)
            {
                foreach (var arg in boolExpr.Args)
                {
                    VisitExpr(arg, context);
                }
            }

            if(boolExpr.Xpr is not null)
            {
                VisitExpr(boolExpr.Xpr, context);
            }

            context.PgTreeWalker.ProcessBoolExpr_ReverseTraversal(node);
        }
    }
}
