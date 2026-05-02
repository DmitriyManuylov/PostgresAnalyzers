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
        public static void VisitExecSqlStmt(PLpgSQL_stmt_execsql execSqlStmt, StmtsProcessingContext context)
        {
            if (execSqlStmt == null)
            {
                throw new ArgumentNullException();
            }

            SetStmtParseTree(execSqlStmt.Sqlstmt.PLpgSQLExpr);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessExecSqlStmt_DirectTraversal(node);

            ExprVisitor.VisitExpr(execSqlStmt.Sqlstmt.PLpgSQLExpr.ParsedStmt.Stmt, context);

            context.PgTreeWalker.ProcessExecSqlStmt_ReverseTraversal(node);

            if (!execSqlStmt.Into)
            {
                return;
            }
        }
    }
}
