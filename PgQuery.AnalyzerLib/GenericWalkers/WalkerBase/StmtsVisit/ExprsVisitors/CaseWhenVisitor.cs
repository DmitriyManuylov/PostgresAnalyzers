using PgQuery;
using PgQueryAnalyzerLib.AnalyzeContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQuery.AnalyzerLib.GenericWalkers.WalkerBase
{
    public abstract partial class GenericPgTreeWalkerBase
    {
        private void VisitCaseWhen(CaseWhen caseWhen, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(caseWhen);

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
