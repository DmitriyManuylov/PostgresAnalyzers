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
        private static void VisitIndexElem(IndexElem indexElem, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(indexElem);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessIndexElem_DirectTraversal(node);

            if (indexElem.Expr is not null)
            {
                VisitExpr(indexElem.Expr, context);
            }

            context.PgTreeWalker.ProcessIndexElem_ReverseTraversal(node);
        }
    }
}
