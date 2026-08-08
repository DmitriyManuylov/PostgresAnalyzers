using PgQuery.AnalyzerLib.GenericWalkers.WalkerBase;
using PgQueryAnalyzerLib.AnalyzeContext;
using PgQueryAnalyzerLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryAnalyzerLib.GenericWalkers
{
    public class GenericPgTreeWalker : GenericPgTreeWalkerBase
    {
        public GenericPgTreeWalker()
        {

        }

        public GenericPgTreeWalker(StmtsProcessingContext context) : base(context)
        {
            context.PgTreeWalker = this;
        }


        //protected void VisitGenericNode(PgGenericNode node, StmtsProcessingContext context)
        //{
        //    switch (node.PgNodeDialectType)
        //    {
        //        case PgNodeDialectType.PgSql:
        //            node.PgSqlNode
        //            break;

        //        case PgNodeDialectType.PlPgSql:
        //            break;
        //    }
        //}


        private List<GenericPgTreeWalkerBase> PgTreeWalkerList { get; set; } = new List<GenericPgTreeWalkerBase>();

        //protected override TAnalyzeResult GetResult()
        //{
        //    throw new NotImplementedException();
        //}

        public void AddWalker(GenericPgTreeWalkerBase walker)
        {
            walker.Context = this.Context;
            PgTreeWalkerList.Add(walker);
        }

        public bool IsWalkerListNotEmpty()
        {
            return PgTreeWalkerList.Any();
        }

        //protected override void ProcessDirectTraversal(PgGenericNode node)
        //{
        //    foreach (var item in PgTreeWalkerList)
        //    {
        //        item.ProcessDirectTraversalInternal(node);
        //    }
        //}

        //protected override void ProcessReverseTraversal(PgGenericNode node)
        //{
        //    foreach (var item in PgTreeWalkerList)
        //    {
        //        item.ProcessReverseTraversalInternal(node);
        //    }
        //}

        public TPgTreeWalker GetTreeWalkerByType<TPgTreeWalker>() where TPgTreeWalker : GenericPgTreeWalkerBase
        {
            if (this.PgTreeWalkerList is null || this.PgTreeWalkerList.Count < 1)
            {
                throw new Exception("Не задано ни одного обработчика дерева запроса");
            }

            foreach (var walker in this.PgTreeWalkerList)
            {
                if (typeof(TPgTreeWalker).Equals(walker.GetType()))
                {
                    return (TPgTreeWalker)walker;
                }
            }

            throw new Exception("Не найден обработчик указанного типа");
        }

        #region Прямой проход


        protected override void ProcessSelectStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessSelectStmt_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessUpdateStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessUpdateStmt_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessInsertStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessInsertStmt_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessDeleteStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessDeleteStmt_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessJoinExpr_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessJoinExpr_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessResTarget_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessResTarget_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessCommonTableExpr_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessCommonTableExpr_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessFuncCall_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessFuncCall_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessList_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessList_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessColumnRef_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessColumnRef_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessAssignStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessAssignStmt_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessBlockStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessBlockStmt_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessExecSqlStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessExecSqlStmt_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessForiStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessForiStmt_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessForsStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessForsStmt_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessIfStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessIfStmt_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessPerformStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessPerformStmt_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessRaiseStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessRaiseStmt_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessReturnNextStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessReturnNextStmt_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessReturnStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessReturnStmt_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessWithClause_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessWithClause_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessAExpr_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessAExpr_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessTypeCast_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessTypeCast_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessString_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessString_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessBoolExpr_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessBoolExpr_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessSubLink_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessSubLink_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessCaseExpr_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessCaseExpr_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessCaseWhen_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessCaseWhen_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessRangeVar_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessRangeVar_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessRangeSubselect_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessRangeSubselect_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessRangeFunction_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessRangeFunction_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessCaseStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessCaseStmt_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessAlterTableStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessAlterTableStmt_DirectTraversalInternal(node);
            }
        }
        protected override void ProcessAlterTableCmd_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessAlterTableCmd_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessDropStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessDropStmt_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessRenameStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessDropStmt_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessParamRef_DirectTraversal(PgGenericNode node)
        {
            foreach(var item in PgTreeWalkerList)
            {
                item.ProcessParamRef_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessCoalesceExpr_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessCoalesceExpr_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessNullTest_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessNullTest_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessNullIfExpr_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessNullIfExpr_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessDirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessDirectTraversalInternal(node);
            }
        }

        protected override void ProcessRowExpr_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessRowExpr_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessMultiAssignRef_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessMultiAssignRef_DirectTraversalInternal(node);
            }
        }
        protected override void ProcessIndexStmt_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessIndexStmt_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessIndexElem_DirectTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessIndexElem_DirectTraversalInternal(node);
            }
        }

        #endregion

        #region Обратный проход

        protected override void ProcessSelectStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessSelectStmt_ReverseTraversalInternal(node);
            }
        }

        internal void fed()
        {

        }

        protected override void ProcessUpdateStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessUpdateStmt_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessInsertStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessInsertStmt_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessDeleteStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessDeleteStmt_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessJoinExpr_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessJoinExpr_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessResTarget_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessResTarget_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessCommonTableExpr_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessCommonTableExpr_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessFuncCall_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessFuncCall_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessColumnRef_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessColumnRef_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessAssignStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessAssignStmt_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessBlockStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessBlockStmt_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessExecSqlStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessExecSqlStmt_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessForiStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessForiStmt_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessForsStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessForsStmt_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessIfStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessIfStmt_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessPerformStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessPerformStmt_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessRaiseStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessRaiseStmt_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessReturnNextStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessReturnNextStmt_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessReturnStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessReturnStmt_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessWithClause_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessWithClause_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessAExpr_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessAExpr_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessTypeCast_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessTypeCast_DirectTraversalInternal(node);
            }
        }

        protected override void ProcessString_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessString_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessBoolExpr_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessBoolExpr_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessSubLink_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessSubLink_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessCaseExpr_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessCaseExpr_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessCaseWhen_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessCaseWhen_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessRangeVar_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessRangeVar_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessRangeSubselect_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessRangeSubselect_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessRangeFunction_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessRangeFunction_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessCaseStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessCaseStmt_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessAlterTableStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessAlterTableStmt_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessAlterTableCmd_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessAlterTableCmd_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessDropStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessDropStmt_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessRenameStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessDropStmt_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessMultiAssignRef_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessMultiAssignRef_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessReverseTraversalInternal(node);
            }
        }

        protected override void ProcessRowExpr_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessRowExpr_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessList_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessList_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessParamRef_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessParamRef_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessCoalesceExpr_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessCoalesceExpr_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessNullTest_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessNullTest_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessNullIfExpr_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessNullIfExpr_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessIndexStmt_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessIndexStmt_ReverseTraversalInternal(node);
            }
        }

        protected override void ProcessIndexElem_ReverseTraversal(PgGenericNode node)
        {
            foreach (var item in PgTreeWalkerList)
            {
                item.ProcessIndexElem_ReverseTraversalInternal(node);
            }
        }
        #endregion
    }
}
