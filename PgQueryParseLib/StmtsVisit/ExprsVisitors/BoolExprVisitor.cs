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
        public static void VisitBoolExpr(BoolExpr boolExpr, StmtsProcessingContext context)
        {
            if (boolExpr is null)
            {
                throw new ArgumentNullException();
            }

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
