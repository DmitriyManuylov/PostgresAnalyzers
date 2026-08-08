using PgQueryAnalyzerLib.AnalyzeContext;
using PgQueryAnalyzerLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQuery.AnalyzerLib.GenericWalkers.WalkerBase
{
    public abstract partial class GenericPgTreeWalkerBase
    {

        #region Прямой проход
        internal void ProcessDirectTraversalInternal(PgGenericNode node)
        {
            this.ProcessDirectTraversal(node);
        }


        internal void ProcessSelectStmt_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessSelectStmt_DirectTraversal(node);
        }

        internal void ProcessUpdateStmt_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessUpdateStmt_DirectTraversal(node);
        }

        internal void ProcessInsertStmt_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessInsertStmt_DirectTraversal(node);
        }

        internal void ProcessDeleteStmt_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessDeleteStmt_DirectTraversal(node);
        }

        internal void ProcessJoinExpr_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessJoinExpr_DirectTraversal(node);
        }

        internal void ProcessResTarget_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessResTarget_DirectTraversal(node);
        }

        internal void ProcessCommonTableExpr_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessCommonTableExpr_DirectTraversal(node);
        }

        internal void ProcessFuncCall_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessFuncCall_DirectTraversal(node);
        }

        internal void ProcessColumnRef_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessColumnRef_DirectTraversal(node);
        }

        internal void ProcessAssignStmt_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessAssignStmt_DirectTraversal(node);
        }

        internal void ProcessBlockStmt_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessBlockStmt_DirectTraversal(node);
        }

        internal void ProcessExecSqlStmt_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessExecSqlStmt_DirectTraversal(node);
        }

        internal void ProcessForiStmt_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessForiStmt_DirectTraversal(node);
        }

        internal void ProcessForsStmt_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessForsStmt_DirectTraversal(node);
        }

        internal void ProcessIfStmt_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessIfStmt_DirectTraversal(node);
        }

        internal void ProcessPerformStmt_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessPerformStmt_DirectTraversal(node);
        }

        internal void ProcessRaiseStmt_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessRaiseStmt_DirectTraversal(node);
        }

        internal void ProcessReturnNextStmt_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessReturnNextStmt_DirectTraversal(node);
        }

        internal void ProcessReturnStmt_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessReturnStmt_DirectTraversal(node);
        }

        internal void ProcessWithClause_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessWithClause_DirectTraversal(node);
        }

        internal void ProcessAExpr_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessAExpr_DirectTraversal(node);
        }
        internal void ProcessTypeCast_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessTypeCast_DirectTraversal(node);
        }

        internal void ProcessString_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessString_DirectTraversal(node);
        }

        internal void ProcessBoolExpr_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessBoolExpr_DirectTraversal(node);
        }

        internal void ProcessSubLink_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessSubLink_DirectTraversal(node);
        }

        internal void ProcessCaseExpr_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessCaseExpr_DirectTraversal(node);
        }

        internal void ProcessCaseWhen_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessCaseWhen_DirectTraversal(node);
        }

        internal void ProcessRangeVar_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessRangeVar_DirectTraversal(node);
        }

        internal void ProcessRangeSubselect_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessRangeSubselect_DirectTraversal(node);
        }

        internal void ProcessRangeFunction_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessRangeFunction_DirectTraversal(node);
        }

        internal void ProcessCaseStmt_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessCaseStmt_DirectTraversal(node);
        }

        internal void ProcessAlterTableStmt_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessAlterTableStmt_DirectTraversal(node);
        }

        internal void ProcessAlterTableCmd_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessAlterTableCmd_DirectTraversal(node);
        }

        internal void ProcessDropStmt_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessDropStmt_DirectTraversal(node);
        }

        internal void ProcessRenameStmt_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessRenameStmt_DirectTraversal(node);
        }

        internal void ProcessMultiAssignRef_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessMultiAssignRef_DirectTraversal(node);
        }

        internal void ProcessMultiAssignRef_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessMultiAssignRef_ReverseTraversal(node);
        }

        internal void ProcessList_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessList_DirectTraversal(node);
        }

        internal void ProcessParamRef_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessParamRef_DirectTraversal(node);
        }

        internal void ProcessCoalesceExpr_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessCoalesceExpr_DirectTraversal(node);
        }

        internal void ProcessNullTest_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessNullTest_DirectTraversal(node);
        }

        internal void ProcessNullIfExpr_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessNullIfExpr_DirectTraversal(node);
        }

        internal void ProcessIndexStmt_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessIndexStmt_DirectTraversal(node);
        }

        internal void ProcessIndexElem_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessIndexElem_DirectTraversal(node);
        }

        #endregion

        #region Обратный проход
        internal void ProcessReverseTraversalInternal(PgGenericNode node)
        {
            this.ProcessReverseTraversal(node);
        }

        internal void ProcessSelectStmt_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessSelectStmt_ReverseTraversal(node);
        }

        internal void ProcessUpdateStmt_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessUpdateStmt_ReverseTraversal(node);
        }

        internal void ProcessInsertStmt_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessInsertStmt_ReverseTraversal(node);
        }

        internal void ProcessDeleteStmt_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessDeleteStmt_ReverseTraversal(node);
        }

        internal void ProcessJoinExpr_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessJoinExpr_ReverseTraversal(node);
        }

        internal void ProcessResTarget_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessResTarget_ReverseTraversal(node);
        }

        internal void ProcessCommonTableExpr_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessCommonTableExpr_ReverseTraversal(node);
        }

        internal void ProcessFuncCall_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessFuncCall_ReverseTraversal(node);
        }

        internal void ProcessColumnRef_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessColumnRef_ReverseTraversal(node);
        }

        internal void ProcessAssignStmt_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessAssignStmt_ReverseTraversal(node);
        }

        internal void ProcessBlockStmt_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessBlockStmt_ReverseTraversal(node);
        }

        internal void ProcessExecSqlStmt_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessExecSqlStmt_ReverseTraversal(node);
        }

        internal void ProcessForiStmt_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessForiStmt_ReverseTraversal(node);
        }

        internal void ProcessForsStmt_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessForsStmt_ReverseTraversal(node);
        }

        internal void ProcessIfStmt_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessIfStmt_ReverseTraversal(node);
        }

        internal void ProcessPerformStmt_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessPerformStmt_ReverseTraversal(node);
        }

        internal void ProcessRaiseStmt_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessRaiseStmt_ReverseTraversal(node);
        }

        internal void ProcessReturnNextStmt_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessReturnNextStmt_ReverseTraversal(node);
        }

        internal void ProcessReturnStmt_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessReturnStmt_ReverseTraversal(node);
        }

        internal void ProcessWithClause_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessWithClause_ReverseTraversal(node);
        }

        internal void ProcessAExpr_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessAExpr_ReverseTraversal(node);
        }

        internal void ProcessTypeCast_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessTypeCast_ReverseTraversal(node);
        }
        internal void ProcessString_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessString_ReverseTraversal(node);
        }

        internal void ProcessBoolExpr_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessBoolExpr_ReverseTraversal(node);
        }

        internal void ProcessSubLink_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessSubLink_ReverseTraversal(node);
        }

        internal void ProcessCaseExpr_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessCaseExpr_ReverseTraversal(node);
        }

        internal void ProcessCaseWhen_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessCaseWhen_ReverseTraversal(node);
        }

        internal void ProcessRangeVar_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessRangeVar_ReverseTraversal(node);
        }

        internal void ProcessRangeSubselect_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessRangeSubselect_ReverseTraversal(node);
        }

        internal void ProcessRangeFunction_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessRangeFunction_ReverseTraversal(node);
        }

        internal void ProcessCaseStmt_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessCaseStmt_ReverseTraversal(node);
        }

        internal void ProcessAlterTableStmt_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessAlterTableStmt_ReverseTraversal(node);
        }

        internal void ProcessAlterTableCmd_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessAlterTableCmd_ReverseTraversal(node);
        }

        internal void ProcessDropStmt_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessDropStmt_ReverseTraversal(node);
        }

        internal void ProcessRenameStmt_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessRenameStmt_ReverseTraversal(node);
        }

        internal void ProcessRowExpr_DirectTraversalInternal(PgGenericNode node)
        {
        	this.ProcessRowExpr_DirectTraversal(node);
        }

        internal void ProcessRowExpr_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessRowExpr_ReverseTraversal(node);
        }

        internal void ProcessList_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessList_ReverseTraversal(node);
        }

        internal void ProcessParamRef_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessParamRef_ReverseTraversal(node);
        }

        internal void ProcessCoalesceExpr_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessCoalesceExpr_ReverseTraversal(node);
        }

        internal void ProcessNullTest_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessNullTest_ReverseTraversal(node);
        }

        internal void ProcessNullIfExpr_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessNullIfExpr_ReverseTraversal(node);
        }

        internal void ProcessIndexStmt_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessIndexStmt_ReverseTraversal(node);
        }

        internal void ProcessIndexElem_ReverseTraversalInternal(PgGenericNode node)
        {
        	this.ProcessIndexElem_ReverseTraversal(node);
        }

        #endregion


        //protected virtual void GeneralPreprocessExpr(PgGenericNode node)
        //{

        //}

        //protected virtual void GeneralPostprocessExpr(PgGenericNode node)
        //{

        //}
    }
}
