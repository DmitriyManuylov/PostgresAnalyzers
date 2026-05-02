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
        public static void VisitWithClause(WithClause withClause, StmtsProcessingContext context)
        {
            if(withClause is null)
            {
                throw new ArgumentNullException();
            }

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessWithClause_DirectTraversal(node);

            foreach(var cte in withClause.Ctes)
            {
                VisitExpr(cte, context);
            }

            context.PgTreeWalker.ProcessWithClause_ReverseTraversal(node);
        }
    }
}
