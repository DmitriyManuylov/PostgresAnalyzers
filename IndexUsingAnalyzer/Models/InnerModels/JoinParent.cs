using PgQuery;
using PgQueryAnalyzerLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PgQuery.Node;

namespace IndexUsingAnalyzer.Models.InnerModels
{
    /// <summary>
    /// Узел, содержащий в блоке From узлы Join (Select, Update)
    /// </summary>
    internal class JoinParent
    {
        public List<TableInnerModel> TablesFrom { get; set; }
        public JoinParent(PgGenericNode node)
        {
            Node = node;
            _joins = new Stack<PgGenericNode>();
        }
        public PgGenericNode Node { get; private set; }

        private Stack<PgGenericNode> _joins { get; set; }

        public PgGenericNode CurrentJoinsHead
        {
            get
            {
                return _joins.Peek();
            }
        }

        public void AddJoinNode(PgGenericNode node)
        {


            if(node.PgSqlNode.NodeCase != NodeOneofCase.JoinExpr)
            {
                throw new NotSupportedException();
            }

            _joins.Push(node);

        }

        public PgGenericNode PopJoinNode()
        {
            return _joins.Pop();
        }
    }
}
