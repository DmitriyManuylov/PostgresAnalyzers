using PgQuery;
using PgQueryAnalyzerLib.AnalyzeContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PgQueryAnalyzerLib.StmtsVisit.ExprsVisitors
{
    public static partial class ExprVisitor
    {
        private static void VisitCoalesceExpr(CoalesceExpr coalesceExpr, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(coalesceExpr);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessCoalesceExpr_DirectTraversal(node);

            if(coalesceExpr.Xpr is not null)
            {
                VisitExpr(coalesceExpr.Xpr, context);
            }

            if (coalesceExpr.Args is not null)
            {
                foreach (var arg in coalesceExpr.Args)
                {
                    VisitExpr(arg, context);
                }
            }

            context.PgTreeWalker.ProcessCoalesceExpr_ReverseTraversal(node);
        }
    }
}
