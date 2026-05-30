using PgQueryAnalyzerLib.AnalyzeContext;
using PgQueryAnalyzerLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryAnalyzerLib.GenericWalkers
{
    public abstract class GenericPgTreeWalkerBase
    {
        public StmtsProcessingContext Context { get; set; }

        public GenericPgTreeWalkerBase()
        {

        }
        public GenericPgTreeWalkerBase(StmtsProcessingContext context)
        {
            Context = context;
        }

        //public abstract TAnalyzeResult GetResult();

        #region Прямой проход
        public abstract void ProcessDirectTraversal(PgGenericNode node);


        public virtual void ProcessSelectStmt_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessUpdateStmt_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessInsertStmt_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessDeleteStmt_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessJoinExpr_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessResTarget_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessCommonTableExpr_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessFuncCall_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessColumnRef_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessAssignStmt_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessBlockStmt_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessExecSqlStmt_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessForiStmt_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessForsStmt_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessIfStmt_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessPerformStmt_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessRaiseStmt_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessReturnNextStmt_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessReturnStmt_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessWithClause_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessAExpr_DirectTraversal(PgGenericNode node)
        {

        }
        public virtual void ProcessTypeCast_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessString_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessBoolExpr_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessSubLink_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessCaseExpr_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessCaseWhen_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessRangeVar_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessRangeSubselect_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessRangeFunction_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessCaseStmt_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessAlterTableStmt_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessAlterTableCmd_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessDropStmt_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessRenameStmt_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessMultiAssignRef_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessMultiAssignRef_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessList_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessParamRef_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessCoalesceExpr_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessNullTest_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessNullIfExpr_DirectTraversal(PgGenericNode node)
        {

        }

        #endregion

        #region Обратный проход
        public abstract void ProcessReverseTraversal(PgGenericNode node);

        public virtual void ProcessSelectStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessUpdateStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessInsertStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessDeleteStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessJoinExpr_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessResTarget_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessCommonTableExpr_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessFuncCall_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessColumnRef_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessAssignStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessBlockStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessExecSqlStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessForiStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessForsStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessIfStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessPerformStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessRaiseStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessReturnNextStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessReturnStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessWithClause_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessAExpr_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessTypeCast_ReverseTraversal(PgGenericNode node)
        {

        }
        public virtual void ProcessString_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessBoolExpr_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessSubLink_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessCaseExpr_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessCaseWhen_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessRangeVar_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessRangeSubselect_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessRangeFunction_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessCaseStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessAlterTableStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessAlterTableCmd_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessDropStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessRenameStmt_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessRowExpr_DirectTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessRowExpr_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessList_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessParamRef_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessCoalesceExpr_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessNullTest_ReverseTraversal(PgGenericNode node)
        {

        }

        public virtual void ProcessNullIfExpr_ReverseTraversal(PgGenericNode node)
        {

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
