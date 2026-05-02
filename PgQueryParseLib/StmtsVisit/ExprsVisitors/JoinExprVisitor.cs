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
        public static void VisitJoinExpr(JoinExpr joinExpr, StmtsProcessingContext context)
        {
            if(joinExpr is null)
            {
                throw new ArgumentNullException();
            }

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessJoinExpr_DirectTraversal(node);

            VisitExpr(joinExpr.Larg, context);

            VisitExpr(joinExpr.Rarg, context);

            VisitExpr(joinExpr.Quals, context);

            context.PgTreeWalker.ProcessJoinExpr_ReverseTraversal(node);
        }
    }
}
