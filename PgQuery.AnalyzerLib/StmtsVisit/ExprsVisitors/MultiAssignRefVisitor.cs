using PgQuery;
using PgQueryAnalyzerLib.AnalyzeContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PgQueryAnalyzerLib.StmtsVisit.ExprsVisitors
{
    public static partial class ExprVisitor
    {
        public static void VisitMultiAssignRef(MultiAssignRef multiAssignRef, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(multiAssignRef);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessMultiAssignRef_DirectTraversal(node);

            if (multiAssignRef.Source is not null)
            {
                VisitExpr(multiAssignRef.Source, context);
            }

            context.PgTreeWalker.ProcessMultiAssignRef_ReverseTraversal(node);
        }
    }
}
