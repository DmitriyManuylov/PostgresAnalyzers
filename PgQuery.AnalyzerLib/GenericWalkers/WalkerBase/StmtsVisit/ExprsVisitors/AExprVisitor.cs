using PgQuery;
using PgQueryAnalyzerLib.AnalyzeContext;
using PgQueryAnalyzerLib.GenericWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQuery.AnalyzerLib.GenericWalkers.WalkerBase
{
    public abstract partial class GenericPgTreeWalkerBase
    {
        private void VisitAExpr(A_Expr aExpr, StmtsProcessingContext context)
        {

            ArgumentNullException.ThrowIfNull(aExpr);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessAExpr_DirectTraversal(node);

            if (aExpr.Lexpr is not null)
            {
                VisitExpr(aExpr.Lexpr, context);
            }

            if (aExpr.Rexpr is not null)
            {
                VisitExpr(aExpr.Rexpr, context);
            }

            foreach (var item in aExpr.Name)
            {
                VisitExpr(item, context);
            }

            context.PgTreeWalker.ProcessAExpr_ReverseTraversal(node);
        }
    }
}
