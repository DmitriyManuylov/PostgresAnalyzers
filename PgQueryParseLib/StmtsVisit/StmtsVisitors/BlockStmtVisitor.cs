using PgQuery;
using PgQueryParseLib.AnalyzeContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryParseLib.StmtsVisit.StmtsVisitors
{
    public static partial class StmtVisitor
    {
        public static void VisitBlockStatement(PLpgSQL_stmt_block blockStmt, StmtsProcessingContext context)
        {
            if (blockStmt is null)
            {
                throw new ArgumentNullException();
            }

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessBlockStmt_DirectTraversal(node);

            foreach (var stmt in blockStmt.Body)
            {
                VisitStmt(stmt, context);
            }

            context.PgTreeWalker.ProcessBlockStmt_ReverseTraversal(node);

        }
    }
}
