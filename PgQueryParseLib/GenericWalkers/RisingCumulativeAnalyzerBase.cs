using PgQueryAnalyzerLib.AnalyzeContext;
using PgQueryAnalyzerLib.GenericWalkers.Models;
using PgQueryAnalyzerLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryAnalyzerLib.GenericWalkers
{
    /// <summary>
    /// "Восходящий накопительный" анализатор:
    /// анализирует узлы и накапливает результаты 
    /// анализа в вышележащих узлах
    /// </summary>
    public abstract class RisingCumulativeAnalyzerBase<TPgAnalyzeNode> : GenericPgTreeWalkerBase where TPgAnalyzeNode:AnalyzeNodeBase, new()
    {
        protected Stack<AnalyzeTreeNode<TPgAnalyzeNode>> NodesStack { get; set; }
        protected AnalyzeTreeNode<TPgAnalyzeNode> CurrentNode
        {
            get => NodesStack.TryPeek(out var node) ? node : null;
        }

        //public override TAnalyzeResult GetResult<TAnalyzeResult>()
        //{
        //    return AnalyzeTree;
        //}

        internal AnalyzeTree<TPgAnalyzeNode> AnalyzeTree { get; private set; }
        public RisingCumulativeAnalyzerBase(StmtsProcessingContext context) : base(context)
        {
            NodesStack = new Stack<AnalyzeTreeNode<TPgAnalyzeNode>>();
            AnalyzeTree = new AnalyzeTree<TPgAnalyzeNode>();
        }

        internal override void ProcessDirectTraversal(PgGenericNode node)
        {
            var analyzeNode = new AnalyzeTreeNode<TPgAnalyzeNode>()
            {
                AnalyzeNodeModel = new TPgAnalyzeNode()
                {
                    PgGenericNode = node
                }
            };

            if (AnalyzeTree.Root is null)
            {
                AnalyzeTree.Root = analyzeNode;
            }

            analyzeNode.Parent = CurrentNode;

            NodesStack.Push(analyzeNode);
        }

        internal override void ProcessReverseTraversal(PgGenericNode node)
        {
            var analyzeNode = NodesStack.Pop();

            if (CurrentNode == null)
            {
                return;
            }

            if (!analyzeNode.AnalyzeNodeModel.IsNeedToPropogate)
            {
                return;
            }

            if (analyzeNode.AnalyzeNodeModel is not null)
            {
                CurrentNode.ChildrenAnalyzeNodeModelList.Add(analyzeNode.AnalyzeNodeModel);
            }

            if (analyzeNode.ChildrenAnalyzeNodeModelList.Count > 0)
            {
                CurrentNode.ChildrenAnalyzeNodeModelList.AddRange(analyzeNode.ChildrenAnalyzeNodeModelList);
            }

            CurrentNode.Children.Add(analyzeNode);
        }

    }
}
