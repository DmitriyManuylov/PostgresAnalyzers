using PgQuery;
using PgQueryAnalyzerLib.AnalyzeContext;
using PgQueryAnalyzerLib.StmtsVisit.StmtsVisitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryAnalyzerLib.StmtsVisit.ExprsVisitors
{
    public static partial class ExprVisitor
    {
        private static void VisitNullIfExpr(NullIfExpr nullIfExpr, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(nullIfExpr);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessNullIfExpr_DirectTraversal(node);

            if (nullIfExpr.Xpr is not null)
            {
                VisitExpr(nullIfExpr.Xpr, context);
            }

            if (nullIfExpr.Args is not null)
            {
                foreach (var arg in nullIfExpr.Args)
                {
                    VisitExpr(arg, context);
                }
            }

            context.PgTreeWalker.ProcessNullIfExpr_ReverseTraversal(node);
        }
    }
}
