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
        public static void VisitUpdateStmt(UpdateStmt updateStmt, StmtsProcessingContext context)
        {
            if(updateStmt == null)
            {
                throw new ArgumentNullException();
            }

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessUpdateStmt_DirectTraversal(node);

            foreach(var targetWrap in updateStmt.TargetList)
            {
                var target = targetWrap.ResTarget;

                var fieldName = target.Name;

                var indirection = target.Indirection;

                var val = target.Val;

                VisitExpr(targetWrap, context);

               // var ret = target.

                //target.
            }

            foreach(var ret in updateStmt.ReturningList)
            {
                VisitExpr(ret, context);
            }

            context.PgTreeWalker.ProcessUpdateStmt_ReverseTraversal(node);
        }
    }
}
