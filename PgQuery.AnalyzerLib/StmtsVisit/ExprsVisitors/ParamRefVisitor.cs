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
        private static void VisitParamRef(ParamRef paramRef, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(paramRef);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessParamRef_DirectTraversal(node);

            context.PgTreeWalker.ProcessParamRef_ReverseTraversal(node);
        }
    }
}
