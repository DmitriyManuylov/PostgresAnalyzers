using PgQuery;
using PgQueryAnalyzerLib.AnalyzeContext;
using PgQueryAnalyzerLib.StmtsVisit.ExprsVisitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryAnalyzerLib.StmtsVisit.StmtsVisitors
{
    public static partial class StmtVisitor
    {
        private static void VisitReturnNextStatement(PLpgSQL_stmt_return_next returnNextStmt, StmtsProcessingContext context)
        {
            if(returnNextStmt is null)
            {
                throw new ArgumentNullException();
            }

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessReturnNextStmt_DirectTraversal(node);

            SetStmtParseTree(returnNextStmt.Expr.PLpgSQLExpr);

            ExprVisitor.VisitExpr(returnNextStmt.Expr.PLpgSQLExpr.ParsedStmt.Stmt, context);

            context.PgTreeWalker.ProcessReturnNextStmt_ReverseTraversal(node);
        }
    }
}
