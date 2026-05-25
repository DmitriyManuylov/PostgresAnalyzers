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
        private static void VisitJoinExpr(JoinExpr joinExpr, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(joinExpr);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessJoinExpr_DirectTraversal(node);

            VisitExpr(joinExpr.Larg, context);

            VisitExpr(joinExpr.Rarg, context);

            VisitExpr(joinExpr.Quals, context);

            context.PgTreeWalker.ProcessJoinExpr_ReverseTraversal(node);
        }
    }
}
