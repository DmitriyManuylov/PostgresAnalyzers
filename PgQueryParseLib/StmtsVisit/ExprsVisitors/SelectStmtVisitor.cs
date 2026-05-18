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
            if(selectStmt is null)
            {
                throw new ArgumentNullException();
            }

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessSelectStmt_DirectTraversal(node);

            if (selectStmt.TargetList != null)
            {
                foreach (var target in selectStmt.TargetList)
                {
                    VisitExpr(target, context);
                }
            }

            if (selectStmt.ValuesLists != null)
            {
                foreach (var value in selectStmt.ValuesLists)
                {
                    VisitExpr(value, context);
                }
            }

            if (selectStmt.FromClause != null)
            {
                foreach(var fromItem in selectStmt.FromClause)
                {
                    VisitExpr(fromItem, context);
                }
            }

            if (selectStmt.WithClause != null)
            {
                foreach(var cte in selectStmt.WithClause.Ctes)
                {
                    VisitExpr(cte, context);
                }
            }


            context.PgTreeWalker.ProcessSelectStmt_ReverseTraversal(node);
        }

    }
}
