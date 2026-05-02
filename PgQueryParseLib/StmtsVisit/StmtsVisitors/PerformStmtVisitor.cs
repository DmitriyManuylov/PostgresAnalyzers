using PgQuery;
using PgQueryParseLib.AnalyzeContext;
using PgQueryParseLib.StmtsVisit.ExprsVisitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryParseLib.StmtsVisit.StmtsVisitors
{
    public static partial class StmtVisitor
    {
        public static void VisitPerformStatement(PLpgSQL_stmt_perform performStmt, StmtsProcessingContext context)
        {
            SetStmtParseTree(performStmt.Expr.PLpgSQLExpr);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessPerformStmt_DirectTraversal(node);

            ExprVisitor.VisitExpr(performStmt.Expr.PLpgSQLExpr.ParsedStmt.Stmt, context);

            context.PgTreeWalker.ProcessPerformStmt_ReverseTraversal(node);
        }
    }
}
