using PgQuery;
using PgQueryAnalyzerLib.AnalyzeContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PgQuery.AnalyzerLib.GenericWalkers.WalkerBase
{
    public abstract partial class GenericPgTreeWalkerBase
    {
        private void VisitParamRef(ParamRef paramRef, StmtsProcessingContext context)
        {
            ArgumentNullException.ThrowIfNull(paramRef);

            var node = context.PgGenericNodes.Peek();

            context.PgTreeWalker.ProcessParamRef_DirectTraversal(node);

            context.PgTreeWalker.ProcessParamRef_ReverseTraversal(node);
        }
    }
}
