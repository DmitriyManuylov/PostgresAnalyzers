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
        private void VisitJoinExpr(JoinExpr joinExpr, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(joinExpr);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessJoinExpr_DirectTraversal(node);

            if (joinExpr.Larg is not null)
            {
                VisitExpr(joinExpr.Larg, context);
            }

            if (joinExpr.Rarg is not null)
            {
                VisitExpr(joinExpr.Rarg, context);
            }

            if (joinExpr.Quals is not null)
            {
                VisitExpr(joinExpr.Quals, context);
            }

            context.PgTreeWalker.ProcessJoinExpr_ReverseTraversal(node);
        }
    }
}
