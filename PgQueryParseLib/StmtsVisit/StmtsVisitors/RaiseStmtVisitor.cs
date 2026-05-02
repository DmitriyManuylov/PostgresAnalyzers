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
        public static void VisitRaiseStatement(PLpgSQL_stmt_raise raiseStmt, StmtsProcessingContext context)
        {
            if (raiseStmt is null)
            {
                throw new ArgumentNullException();
            }

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessRaiseStmt_DirectTraversal(node);

            foreach(var item in raiseStmt.Params)
            {
                SetStmtParseTree(item.PLpgSQLExpr);

                ExprVisitor.VisitExpr(item.PLpgSQLExpr.ParsedStmt.Stmt, context);
            }

            context.PgTreeWalker.ProcessRaiseStmt_ReverseTraversal(node);
        }
    }
}
