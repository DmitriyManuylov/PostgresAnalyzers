using PgQuery;
using PgQueryAnalyzerLib.AnalyzeContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQuery.AnalyzerLib.GenericWalkers.WalkerBase
{
    public abstract partial class GenericPgTreeWalkerBase
    {
        private void VisitExecSqlStmt(PLpgSQL_stmt_execsql execSqlStmt, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(execSqlStmt);

            SetStmtParseTree(execSqlStmt.Sqlstmt.PLpgSQLExpr);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessExecSqlStmt_DirectTraversal(node);

            VisitExpr(execSqlStmt.Sqlstmt.PLpgSQLExpr.ParsedStmt.Stmt, context);

            context.PgTreeWalker.ProcessExecSqlStmt_ReverseTraversal(node);

            if (!execSqlStmt.Into)
            {
                return;
            }
        }
    }
}
