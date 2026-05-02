using PgQuery;
using PgQueryParseLib.AnalyzeContext;
using PgQueryParseLib.GenericWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryParseLib.StmtsVisit.ExprsVisitors
{
    public static partial class ExprVisitor
    {
        public static void VisitAExpr(A_Expr aExpr, StmtsProcessingContext context)
        {
            
            if (aExpr is null)
            {
                throw new ArgumentNullException();
            }

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessAExpr_DirectTraversal(node);

            VisitExpr(aExpr.Lexpr, context);

            VisitExpr(aExpr.Rexpr, context);

            foreach (var item in aExpr.Name)
            {
                VisitExpr(item, context);
            }

            context.PgTreeWalker.ProcessAExpr_ReverseTraversal(node);
        }
    }
}
