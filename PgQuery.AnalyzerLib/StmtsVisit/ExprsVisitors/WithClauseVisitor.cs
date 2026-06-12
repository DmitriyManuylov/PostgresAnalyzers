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
        private static void VisitWithClause(WithClause withClause, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(withClause);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessWithClause_DirectTraversal(node);

            foreach(var cte in withClause.Ctes)
            {
                node.SubOperation = "Cte";
                VisitExpr(cte, context);
                node.SubOperation = null;
            }

            context.PgTreeWalker.ProcessWithClause_ReverseTraversal(node);
        }
    }
}
