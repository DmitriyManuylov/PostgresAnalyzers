using PgQueryAnalyzerLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryAnalyzerLib.GenericWalkers.Models
{
    public class AnalyzeTreeNode<TPgAnalyzeNode> where TPgAnalyzeNode : AnalyzeNodeBase, new()
    {
        public TPgAnalyzeNode AnalyzeNodeModel { get; set; }
        public AnalyzeTreeNode<TPgAnalyzeNode> Parent { get; set; }

        public List<AnalyzeTreeNode<TPgAnalyzeNode>> Children { get; set; } = new();

        /// <summary>
        /// Полный список результатов анализа дочерних узлов данного узла.
        /// </summary>
        public List<TPgAnalyzeNode> ChildrenAnalyzeNodeModelList { get; set; } = new();

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
