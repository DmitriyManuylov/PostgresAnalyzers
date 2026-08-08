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
        private void VisitNullTest(NullTest nullTest, StmtsProcessingContext context)
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
