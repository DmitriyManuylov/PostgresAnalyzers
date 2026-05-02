using PgQuery;
using PgQueryParseLib.AnalyzeContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PgQueryParseLib.StmtsVisit.ExprsVisitors
{
    public static partial class ExprVisitor
    {
        public static void VisitDeleteStmt(DeleteStmt deleteStmt, StmtsProcessingContext context)
        {
            if(deleteStmt is null)
            {
                throw new ArgumentNullException();
            }

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessDeleteStmt_DirectTraversal(node);

            VisitExpr(deleteStmt.WhereClause, context);

            foreach (var cte in deleteStmt.WithClause.Ctes)
            {
                VisitExpr(cte, context);
            }

            foreach (var item in deleteStmt.ReturningList)
            {
                VisitExpr(item, context);
            }

            context.PgTreeWalker.ProcessDeleteStmt_ReverseTraversal(node);
        }
    }
}
