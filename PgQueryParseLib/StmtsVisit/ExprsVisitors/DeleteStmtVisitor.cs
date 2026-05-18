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
        private static void VisitDeleteStmt(DeleteStmt deleteStmt, StmtsProcessingContext context)
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
