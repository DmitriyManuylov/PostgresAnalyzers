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
        private static void VisitReturnStatement(PLpgSQL_stmt_return returnStmt, StmtsProcessingContext context)
        {
            if(returnStmt is null)
            {
                throw new ArgumentNullException();
            }

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessReturnStmt_DirectTraversal(node);

            if (returnStmt.Expr is not null)
            {
                SetStmtParseTree(returnStmt.Expr.PLpgSQLExpr);

                ExprVisitor.VisitExpr(returnStmt.Expr.PLpgSQLExpr.ParsedStmt.Stmt, context);
            }

            context.PgTreeWalker.ProcessReturnStmt_ReverseTraversal(node);
        }
    }
}
