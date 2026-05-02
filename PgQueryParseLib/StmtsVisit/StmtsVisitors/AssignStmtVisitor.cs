using PgQuery;
using PgQueryParseLib.AnalyzeContext;
using PgQueryParseLib.Models;
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
        public static void VisitAssignStatement(PLpgSQL_stmt_assign assignStmt, StmtsProcessingContext context)
        {
            SetStmtParseTree(assignStmt.Expr.PLpgSQLExpr);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessAssignStmt_DirectTraversal(node);

            ExprVisitor.VisitExpr(assignStmt.Expr.PLpgSQLExpr.ParsedStmt.Stmt, context);

            context.PgTreeWalker.ProcessAssignStmt_ReverseTraversal(node);
        }
    }
}
