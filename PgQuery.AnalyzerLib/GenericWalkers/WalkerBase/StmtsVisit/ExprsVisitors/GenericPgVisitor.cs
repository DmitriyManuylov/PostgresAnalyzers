using PgQueryAnalyzerLib.AnalyzeContext;
using PgQueryAnalyzerLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQuery.AnalyzerLib.GenericWalkers.WalkerBase.StmtsVisit.ExprsVisitors
{
    public abstract class GenericPgVisitor
    {
        public PgGenericNode Node { get; set; }

        public abstract PgGenericNode VisitNode(PgGenericNode node, StmtsProcessingContext context);


    }
}
