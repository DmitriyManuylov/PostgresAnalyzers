using PgQueryParseLib.AnalyzeContext;
using PgQueryParseLib.GenericWalkers.Models;
using PgQueryParseLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryParseLib.GenericWalkers
{
    /// <summary>
    /// 
    /// </summary>
    public class DMLAnalyzer:RisingCumulativeAnalyzerBase<DMLAnalyzeNode>
    {
        private LinkedList<PgGenericNode> pgGenericNodes = new LinkedList<PgGenericNode>();

        public DMLAnalyzer(StmtsProcessingContext context) : base(context)
        {
        }

        //public override void ProcessDirectTraversal(PgGenericNode node)
        //{
        //    base.ProcessDirectTraversal(node);
        //}

        //public override void ProcessReverseTraversal(PgGenericNode node)
        //{
        //    base.ProcessDirectTraversal(node);
        //}

        public override void ProcessUpdateStmt_DirectTraversal(PgGenericNode node)
        {
            base.ProcessUpdateStmt_DirectTraversal(node);
        }

        public override void ProcessInsertStmt_DirectTraversal(PgGenericNode node)
        {
            base.ProcessInsertStmt_DirectTraversal(node);
        }

        public override void ProcessDeleteStmt_DirectTraversal(PgGenericNode node)
        {
            base.ProcessDeleteStmt_DirectTraversal(node);
        }

        public override void ProcessUpdateStmt_ReverseTraversal(PgGenericNode node)
        {
            base.ProcessUpdateStmt_ReverseTraversal(node);
        }

        public override void ProcessInsertStmt_ReverseTraversal(PgGenericNode node)
        {
            base.ProcessInsertStmt_ReverseTraversal(node);
        }

        public override void ProcessDeleteStmt_ReverseTraversal(PgGenericNode node)
        {
            base.ProcessDeleteStmt_ReverseTraversal(node);
        }

        //protected override void GeneralPreprocessExpr(PgGenericNode node)
        //{
        //    base.GeneralPreprocessExpr(node);
        //}
    }
}
