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

            foreach(var targetWrap in updateStmt.TargetList)
            {
                var target = targetWrap.ResTarget;

                VisitExpr(targetWrap, context);

            }

            foreach(var ret in updateStmt.ReturningList)
            {
                VisitExpr(ret, context);
            }

            if(updateStmt.WhereClause is not null)
            {
                VisitExpr(updateStmt.WhereClause, context);
            }

            context.PgTreeWalker.ProcessUpdateStmt_ReverseTraversal(node);
        }
    }
}
