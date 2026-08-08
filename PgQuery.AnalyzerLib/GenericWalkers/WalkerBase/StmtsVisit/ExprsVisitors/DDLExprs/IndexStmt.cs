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
        private void VisitIndexStmt(IndexStmt indexStmt, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(indexStmt);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessIndexStmt_DirectTraversal(node);

            if (indexStmt.IndexParams is not null)
            {
                foreach (var param in indexStmt.IndexParams)
                {
                    VisitExpr(param, context);
                }
            }

            if (indexStmt.IndexIncludingParams is not null)
            {
                foreach (var param in indexStmt.IndexIncludingParams)
                {
                    VisitExpr(param, context);
                }
            }

            context.PgTreeWalker.ProcessIndexStmt_ReverseTraversal(node);
        }
    }
}
