using PgQuery;
using PgQueryParseLib.AnalyzeContext;
using PgQueryParseLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryParseLib.StmtsVisit.ExprsVisitors
{
    public static partial class ExprVisitor
    {
        public static void VisitCommonTableExpr(CommonTableExpr commonTableExpr, StmtsProcessingContext context)
        {
            if(commonTableExpr is null)
            {
                throw new Exception();
            }

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessCommonTableExpr_DirectTraversal(node);
            VisitExpr(commonTableExpr.Ctequery, context);

            context.PgTreeWalker.ProcessCommonTableExpr_ReverseTraversal(node);
        }
    }
}
