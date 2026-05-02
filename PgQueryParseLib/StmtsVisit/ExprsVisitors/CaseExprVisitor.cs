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
        public static void VisitCaseExpr(CaseExpr caseExpr, StmtsProcessingContext context)
        {
            if(caseExpr is null)
            {
                throw new ArgumentNullException();
            }

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessCaseExpr_DirectTraversal(node);

            if (caseExpr.Xpr is not null)
            {
                VisitExpr(caseExpr.Xpr, context);
            }

            if (caseExpr.Arg is not null)
            {
                VisitExpr(caseExpr.Arg, context);
            }

            if (caseExpr.Args is not null)
            {
                foreach (var arg in caseExpr.Args)
                {
                    VisitExpr(arg, context);
                }
            }

            if (caseExpr.Defresult is not null)
            {
                VisitExpr(caseExpr.Defresult, context);
            }

            context.PgTreeWalker.ProcessCaseExpr_ReverseTraversal(node);
        }
    }
}
