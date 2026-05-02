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
        public static void VisitResTarget(ResTarget  resTarget, StmtsProcessingContext context)
        {
            if(resTarget is null)
            {
                throw new ArgumentNullException();
            }

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessResTarget_DirectTraversal(node);

            VisitExpr(resTarget.Val, context);

            context.PgTreeWalker.ProcessResTarget_ReverseTraversal(node);
        }
    }
}
