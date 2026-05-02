using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryParseLib.GenericWalkers.Models
{
    public class AnalyzeTree<TPgAnalyzeNode> where TPgAnalyzeNode : AnalyzeNodeBase, new()
    {
        public AnalyzeTreeNode<TPgAnalyzeNode> Root { get; set; }

        public AnalyzeTreeNode<TPgAnalyzeNode> CurrentNode { get; set; }

        public void DFS(Action<AnalyzeTreeNode<TPgAnalyzeNode>> handler)
        {
            Root.DFS(handler);
        }
    }
}
