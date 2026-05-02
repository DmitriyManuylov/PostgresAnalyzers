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
        public static void VisitForsStatement(PLpgSQL_stmt_fors forsStmt, StmtsProcessingContext context)
        {
            SetStmtParseTree(forsStmt.Query.PLpgSQLExpr);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessForsStmt_DirectTraversal(node);

            ExprVisitor.VisitExpr(forsStmt.Query.PLpgSQLExpr.ParsedStmt.Stmt, context);

            foreach(PLpgSQL_stmt stmt in forsStmt.Body)
            {
                VisitStmt(stmt, context);
            }

            context.PgTreeWalker.ProcessForsStmt_ReverseTraversal(node);
        }
    }
}
