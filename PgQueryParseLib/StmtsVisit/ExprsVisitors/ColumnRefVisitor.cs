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
        private static void VisitColumnRef(ColumnRef columnRef, StmtsProcessingContext context)
        {
            if(columnRef is null)
            {
                throw new ArgumentNullException();
            }

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
