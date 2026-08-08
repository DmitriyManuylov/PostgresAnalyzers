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
        private void VisitColumnRef(ColumnRef columnRef, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(columnRef);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessColumnRef_DirectTraversal(node);

            foreach (var field in columnRef.Fields) 
            {
                VisitExpr(field, context);
            }

            context.PgTreeWalker.ProcessColumnRef_ReverseTraversal(node);
        }
    }
}
