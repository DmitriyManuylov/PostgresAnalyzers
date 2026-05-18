using PgQuery;
using PgQueryAnalyzerLib.AnalyzeContext;
using PgQueryAnalyzerLib.GenericWalkers.Models;
using PgQueryAnalyzerLib.Models;
using PgQueryAnalyzerLib.StmtsVisit.ExprsVisitors;
using PgQueryAnalyzerLib.StmtsVisit.StmtsVisitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PgQueryAnalyzerLib.GenericWalkers
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

        internal override void ProcessDirectTraversal(PgGenericNode node)
        {
            base.ProcessDirectTraversal(node);
        }

        internal override void ProcessReverseTraversal(PgGenericNode node)
        {
            var analyzeNode = base.NodesStack.Peek();
            if (analyzeNode.AnalyzeNodeModel.Model is null && analyzeNode.ChildrenAnalyzeNodeModelList.Count <= 0)
            {
                analyzeNode.AnalyzeNodeModel.IsNeedToPropogate = false;
            }

            base.ProcessReverseTraversal(node);
        }

        internal override void ProcessUpdateStmt_DirectTraversal(PgGenericNode node)
        {
            base.ProcessUpdateStmt_DirectTraversal(node);

            var updateStmt = node.PgSqlNode.UpdateStmt;

            var entity = updateStmt.Relation;

            var analyzeNode = base.NodesStack.Peek().AnalyzeNodeModel;

            analyzeNode.IsNeedToPropogate = true;
            analyzeNode.Model = new DMLAnalyzeModel
            {
                Schema = entity.Schemaname,
                Table = entity.Relname,
                OpType = SQLDmlType.Update,
                Fields = updateStmt.TargetList.Select(item => item.ResTarget.Name).ToList(),
            };
        }

        internal override void ProcessInsertStmt_DirectTraversal(PgGenericNode node)
        {
            base.ProcessInsertStmt_DirectTraversal(node);

            var insertStmt = node.PgSqlNode.InsertStmt;

            var entity = insertStmt.Relation;

            var analyzeNode = base.NodesStack.Peek().AnalyzeNodeModel;

            analyzeNode.IsNeedToPropogate = true;
            analyzeNode.Model = new DMLAnalyzeModel
            {
                Schema = entity.Schemaname,
                Table = entity.Relname,
                OpType = SQLDmlType.Insert,
                Fields = insertStmt.Cols.Select(item => item.ResTarget.Name).ToList(),
            };
        }

        internal override void ProcessDeleteStmt_DirectTraversal(PgGenericNode node)
        {
            base.ProcessDeleteStmt_DirectTraversal(node);

            var deleteStmt = node.PgSqlNode.DeleteStmt;

            var entity = deleteStmt.Relation;

            var analyzeNode = base.NodesStack.Peek().AnalyzeNodeModel;

            analyzeNode.IsNeedToPropogate = true;
            analyzeNode.Model = new DMLAnalyzeModel
            {
                Schema = entity.Schemaname,
                Table = entity.Relname,
                OpType = SQLDmlType.Delete
            };
        }

        internal override void ProcessUpdateStmt_ReverseTraversal(PgGenericNode node)
        {
            base.ProcessUpdateStmt_ReverseTraversal(node);
        }

        internal override void ProcessInsertStmt_ReverseTraversal(PgGenericNode node)
        {
            base.ProcessInsertStmt_ReverseTraversal(node);
        }

        internal override void ProcessDeleteStmt_ReverseTraversal(PgGenericNode node)
        {
            base.ProcessDeleteStmt_ReverseTraversal(node);
        }

        public AnalyzeTree<DMLAnalyzeNode> GetResult()
        {
            return AnalyzeTree;
        }

        //protected override void GeneralPreprocessExpr(PgGenericNode node)
        //{
        //    base.GeneralPreprocessExpr(node);
        //}
    }
}
