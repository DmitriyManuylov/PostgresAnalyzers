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
        private static void VisitNullTest(NullTest nullTest, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(nullTest);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessNullTest_DirectTraversal(node);

            if (nullTest.Arg is not null)
            {
                VisitExpr(nullTest.Arg, context);
            }

            context.PgTreeWalker.ProcessNullTest_ReverseTraversal(node);
        }
    }
}
