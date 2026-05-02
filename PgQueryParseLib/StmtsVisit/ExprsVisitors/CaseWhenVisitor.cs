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
        public static void VisitCaseWhen(CaseWhen caseWhen, StmtsProcessingContext context)
        {
            if(caseWhen is null)
            {
                throw new ArgumentNullException();
            }

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessCaseWhen_DirectTraversal(node);

            if(caseWhen.Xpr is not null)
            {
                VisitExpr(caseWhen.Xpr, context);
            }

            if(caseWhen.Expr is not null)
            {
                VisitExpr(caseWhen.Expr, context);
            }

            if(caseWhen.Result is not null)
            {
                VisitExpr(caseWhen.Result, context);
            }

            context.PgTreeWalker.ProcessCaseWhen_ReverseTraversal(node);
        }
    }
}
