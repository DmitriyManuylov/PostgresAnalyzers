using PgQueryParseLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryParseLib.GenericWalkers.Models
{
    public class AnalyzeTreeNode<TPgAnalyzeNode> where TPgAnalyzeNode : AnalyzeNodeBase, new()
    {
        public TPgAnalyzeNode AnalyzeNodeModel { get; set; }
        public AnalyzeTreeNode<TPgAnalyzeNode> Parent { get; set; }

        public List<AnalyzeTreeNode<TPgAnalyzeNode>> Children { get; set; }

        public void DFS(Action<AnalyzeTreeNode<TPgAnalyzeNode>> handler)
        {

            foreach(var item in Children)
            {
                DFS(handler);
            }

            handler(this);
        }
    }
}
