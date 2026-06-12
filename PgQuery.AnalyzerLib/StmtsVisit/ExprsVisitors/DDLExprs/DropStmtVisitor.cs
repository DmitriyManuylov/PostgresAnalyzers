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
        private static void VisitDropStmt(DropStmt dropStmt, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(dropStmt);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessDropStmt_DirectTraversal(node);

            foreach(var obj in dropStmt.Objects)
            {
                VisitExpr(obj, context);
            }

            context.PgTreeWalker.ProcessAlterTableStmt_ReverseTraversal(node);
        }
    }
}
