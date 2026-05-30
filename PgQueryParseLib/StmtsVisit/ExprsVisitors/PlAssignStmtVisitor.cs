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
        private static void VisitPlAssignStmt(PLAssignStmt plAssignStmt, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(plAssignStmt);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessDeleteStmt_DirectTraversal(node);

            if (plAssignStmt.Val is not null)
            {
                var selectStmt = new Node()
                {
                    SelectStmt = plAssignStmt.Val
                };

                VisitExpr(selectStmt, context);
            }

            context.PgTreeWalker.ProcessDeleteStmt_ReverseTraversal(node);
        }
    }
}
