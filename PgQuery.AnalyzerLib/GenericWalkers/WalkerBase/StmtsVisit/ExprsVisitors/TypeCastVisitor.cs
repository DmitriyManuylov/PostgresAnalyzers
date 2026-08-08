using PgQuery;
using PgQueryAnalyzerLib.AnalyzeContext;
using PgQueryAnalyzerLib.GenericWalkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQuery.AnalyzerLib.GenericWalkers.WalkerBase
{
    public abstract partial class GenericPgTreeWalkerBase
    {
        private void VisitTypeCast(TypeCast typeCast, StmtsProcessingContext context)
        {

            ArgumentNullException.ThrowIfNull(typeCast);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessTypeCast_DirectTraversal(node);

            VisitExpr(typeCast.Arg, context);

            context.PgTreeWalker.ProcessTypeCast_ReverseTraversal(node);
        }
    }
}
