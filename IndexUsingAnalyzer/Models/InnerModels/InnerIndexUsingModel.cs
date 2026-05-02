using PgQuery;
using PgQueryParseLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PgQuery.Node;

namespace IndexUsingAnalyzer.Models.InnerModels
{
    internal class InnerIndexUsingModel
    {
        private List<NodeOneofCase> _exprTypesList;

        public InnerIndexUsingModel()
        {
            _exprTypesList = new List<NodeOneofCase>
            {
                NodeOneofCase.SelectStmt,
                NodeOneofCase.UpdateStmt,
            };
        }
        /// <summary>
        /// Select, Update, содержащие join'ы в блоке From
        /// </summary>
        private Stack<JoinParent> _joinParents;

        public JoinParent JoinsParentStackHead
        {
            get
            {
                return _joinParents.Peek();
            }
        }

        public void AddNode(PgGenericNode node)
        {
            if (!_exprTypesList.Contains(node.PgSqlNode.NodeCase))
            {
                throw new NotSupportedException();
            }

            _joinParents.Push(new JoinParent(node));
        }

        public void AddJoinToLastNode(PgGenericNode node)
        {
            JoinsParentStackHead.AddJoinNode(node);
        }

        public JoinParent PopNode()
        {
            return _joinParents.Pop();
        }
    }
}
