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
            ArgumentNullException.ThrowIfNull(deleteStmt);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessDeleteStmt_DirectTraversal(node);

            if (deleteStmt.WhereClause is not null)
            {
                node.SubOperation = "WhereClause";
                VisitExpr(deleteStmt.WhereClause, context);
                node.SubOperation = null;
            }

            if (deleteStmt.WithClause?.Ctes is not null)
            {
                node.SubOperation = nameof(deleteStmt.WithClause);
                foreach (var cte in deleteStmt.WithClause.Ctes)
                {
                    VisitExpr(cte, context);
                }
                node.SubOperation = null;
            }

            if (deleteStmt.ReturningList is not null)
            {
                node.SubOperation = nameof(DeleteStmt.ReturningList);
                foreach (var item in deleteStmt.ReturningList)
                {
                    VisitExpr(item, context);
                }
                node.SubOperation = null;
            }

            context.PgTreeWalker.ProcessDeleteStmt_ReverseTraversal(node);
        }
    }
}
