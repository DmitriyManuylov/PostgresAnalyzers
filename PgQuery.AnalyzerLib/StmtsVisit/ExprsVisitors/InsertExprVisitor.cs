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
        private static void VisitInsertStmt(InsertStmt insertStmt, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(insertStmt);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessInsertStmt_DirectTraversal(node);

            foreach (var col in insertStmt.Cols)
            {
                VisitExpr(col, context);
            }

            if (insertStmt.WithClause is not null)
            {
                foreach (var cte in insertStmt.WithClause.Ctes)
                {
                    VisitExpr(cte, context);
                }
            }

            if (insertStmt.SelectStmt is not null)
            {
                VisitExpr(insertStmt.SelectStmt, context);
            }

            context.PgTreeWalker.ProcessInsertStmt_ReverseTraversal(node);
        }
    }
}
