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
        private static void VisitSubLink(SubLink subLink, StmtsProcessingContext context)
        {
            if(subLink is null)
            {
                throw new ArgumentNullException();
            }

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessSubLink_DirectTraversal(node);

            if(subLink.Xpr is not null)
            {
                VisitExpr(subLink.Xpr, context);
            }

            if(subLink.Subselect is not null)
            {
                VisitExpr(subLink.Subselect, context);
            }

            if(subLink.Testexpr is not null)
            {
                VisitExpr(subLink.Testexpr, context);
            }

            context.PgTreeWalker.ProcessSubLink_ReverseTraversal(node);
        }
    }
}
