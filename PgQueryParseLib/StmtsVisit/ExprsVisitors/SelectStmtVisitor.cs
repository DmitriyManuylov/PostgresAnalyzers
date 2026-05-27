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
        private static void VisitSelectStmt(SelectStmt selectStmt, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(selectStmt);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessSelectStmt_DirectTraversal(node);

            if (selectStmt.TargetList != null)
            {
                node.SubOperation = nameof(SelectStmt.TargetList);

                foreach (var target in selectStmt.TargetList)
                {
                    VisitExpr(target, context);
                }
            }

            if (selectStmt.ValuesLists != null)
            {
                node.SubOperation = nameof(SelectStmt.ValuesLists);

                foreach (var value in selectStmt.ValuesLists)
                {
                    VisitExpr(value, context);
                }
            }

            if (selectStmt.FromClause != null)
            {
                node.SubOperation = nameof(SelectStmt.FromClause);

                foreach (var fromItem in selectStmt.FromClause)
                {
                    VisitExpr(fromItem, context);
                }
            }

            if (selectStmt.WithClause != null)
            {
                node.SubOperation = nameof(SelectStmt.WithClause);

                foreach (var cte in selectStmt.WithClause.Ctes)
                {
                    VisitExpr(cte, context);
                }
            }

            if (selectStmt.WhereClause is not null)
            {
                node.SubOperation = nameof(SelectStmt.WhereClause);

                VisitExpr(selectStmt.WhereClause, context);
            }

            node.SubOperation = null;

            context.PgTreeWalker.ProcessSelectStmt_ReverseTraversal(node);
        }

    }
}
