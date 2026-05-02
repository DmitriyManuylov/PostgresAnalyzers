using PgQuery;
using PgQueryParseLib.AnalyzeContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryParseLib.StmtsVisit.ExprsVisitors
{
    public static partial class ExprVisitor
    {
        public static void VisitInsertStmt(InsertStmt insertStmt, StmtsProcessingContext context)
        {
            if (insertStmt == null)
            {
                throw new ArgumentNullException();
            }

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessInsertStmt_DirectTraversal(node);

            foreach (var col in insertStmt.Cols)
            {
                VisitExpr(col, context);
            }

            foreach(var cte in insertStmt.WithClause.Ctes)
            {
                VisitExpr(cte, context);
            }



            context.PgTreeWalker.ProcessInsertStmt_ReverseTraversal(node);
        }
    }
}
