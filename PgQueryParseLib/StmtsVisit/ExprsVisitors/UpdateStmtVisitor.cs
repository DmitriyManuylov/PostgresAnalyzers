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
        private static void VisitUpdateStmt(UpdateStmt updateStmt, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(updateStmt);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessUpdateStmt_DirectTraversal(node);

            node.SubOperation = nameof(UpdateStmt.TargetList);

            foreach (var targetWrap in updateStmt.TargetList)
            {
                VisitExpr(targetWrap, context);
            }

            if (updateStmt.ReturningList is not null)
            {
                node.SubOperation = nameof(UpdateStmt.ReturningList);

                foreach (var ret in updateStmt.ReturningList)
                {
                    VisitExpr(ret, context);
                }
            }

            if (updateStmt.WhereClause is not null)
            {
                node.SubOperation = nameof(UpdateStmt.WhereClause);

                VisitExpr(updateStmt.WhereClause, context);
            }

            node.SubOperation = null;

            context.PgTreeWalker.ProcessUpdateStmt_ReverseTraversal(node);
        }
    }
}
