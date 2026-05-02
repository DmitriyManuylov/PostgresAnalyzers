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
    /// "Восходящий накопительный" анализатор:
    /// анализирует узлы и накапливает результаты 
    /// анализа в вышележащих узлах
    /// </summary>
    public abstract class RisingCumulativeAnalyzerBase<TPgAnalyzeNode> : GenericPgTreeWalkerBase where TPgAnalyzeNode:AnalyzeNodeBase, new()
    {
        protected Stack<AnalyzeTreeNode<TPgAnalyzeNode>> NodesStack { get; set; }
        protected AnalyzeTreeNode<TPgAnalyzeNode> CurrentNode
        {
            get => NodesStack.Peek();
        }

        public AnalyzeTree<TPgAnalyzeNode> AnalyzeTree { get; private set; }
        public RisingCumulativeAnalyzerBase(StmtsProcessingContext context): base(context)
        {
            NodesStack = new Stack<AnalyzeTreeNode<TPgAnalyzeNode>>();
            AnalyzeTree = new AnalyzeTree<TPgAnalyzeNode>();
        }

        public override void ProcessDirectTraversal(PgGenericNode node)
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

        public override void ProcessReverseTraversal(PgGenericNode node)
        {

            if (CurrentNode == AnalyzeTree.Root)
            {
                return;
            }

            var analyzeNode = NodesStack.Pop();

            if (analyzeNode.AnalyzeNodeModel.IsNeedToPropogate)
            {
                CurrentNode.Children.Add(analyzeNode);
            }
        }

    }
}
