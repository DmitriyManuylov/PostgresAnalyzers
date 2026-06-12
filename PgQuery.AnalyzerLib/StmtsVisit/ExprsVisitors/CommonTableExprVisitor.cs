using PgQuery;
using PgQueryAnalyzerLib.AnalyzeContext;
using PgQueryAnalyzerLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryAnalyzerLib.StmtsVisit.ExprsVisitors
{
    public static partial class ExprVisitor
    {
        private static void VisitCommonTableExpr(CommonTableExpr commonTableExpr, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(commonTableExpr);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessCommonTableExpr_DirectTraversal(node);
            VisitExpr(commonTableExpr.Ctequery, context);

            context.PgTreeWalker.ProcessCommonTableExpr_ReverseTraversal(node);
        }
    }
}
