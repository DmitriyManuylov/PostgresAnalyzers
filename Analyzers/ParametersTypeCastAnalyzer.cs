using DataChangeAnalyzer.Models.DBModels;
using PgQuery;
using PgQuery.AnalyzerLib.GenericWalkers;
using PgQuery.AnalyzerLib.GenericWalkers.Models.SemanticAnalyzer;
using PgQuery.Analyzers.Models.ParametersTypeCastAnalyzer;
using PgQueryAnalyzerLib.AnalyzeContext;
using PgQueryAnalyzerLib.GenericWalkers;
using PgQueryAnalyzerLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzers
{
    public class ParametersTypeCastAnalyzer : GenericQuerySemanticAnalyzer
    {
        private static List<string> Operations = new List<string>()
        {
            "=",
            "<>"
        };

        private List<ParameterTypeCastAnalyzeModel> analyzeModels = new();

        public ParametersTypeCastAnalyzer()
        {
        }

        public ParametersTypeCastAnalyzer(StmtsProcessingContext context) : base(context)
        {
        }

        public override void ProcessDirectTraversal(PgGenericNode node)
        {

        }

        public override void ProcessReverseTraversal(PgGenericNode node)
        {

        }

        public override void ProcessParamRef_DirectTraversal(PgGenericNode node)
        {
            base.ProcessParamRef_DirectTraversal(node);

            var scope = CurrentScope as QueryScope;

            var whereClause = Context.GetNearestNodeByType(node => new List<string>
            {
                nameof(SelectStmt.WhereClause),
                nameof(SelectStmt.FromClause)
            }
            .Contains(node.SubOperation));

            if (whereClause is null)
            {
                return;
            }

            var paramRef = node.PgSqlNode.ParamRef;

            var stackList = Context.PgGenericNodes.ToList();

            TypeCast typeCastStmt = null;
            string parameterName = Context.QueryParameters[paramRef.Number - 1];
            TableModel comparableTable = null;

            Node nodeToCompare = null;
            ColumnRef columnRefToCompare = null;
            string columnRefTable = null;
            string columnRefName = null;


            int depth = 0;
            var currentNode = stackList[depth++];
            var parentNode = stackList[depth];

            while (depth < stackList.Count) 
            {
                nodeToCompare = null;

                switch (parentNode?.PgSqlNode.NodeCase)
                {
                    case Node.NodeOneofCase.TypeCast:
                        typeCastStmt = parentNode.PgSqlNode.TypeCast;
                        break;
                    case Node.NodeOneofCase.AExpr:
                        A_Expr expr = parentNode.PgSqlNode.AExpr;
                        var lexpr = expr.Lexpr;
                        var rexpr = expr.Rexpr;
                        string opname = expr.Name.First().String.Sval;

                        nodeToCompare = expr.Rexpr == currentNode.PgSqlNode ? expr.Lexpr : expr.Rexpr;
                        columnRefToCompare = nodeToCompare.NodeCase == Node.NodeOneofCase.ColumnRef ? nodeToCompare.ColumnRef : null;

                        break;
                    case Node.NodeOneofCase.SubLink:
                        SubLink subLink = parentNode.PgSqlNode.SubLink;

                        nodeToCompare = subLink.Subselect == currentNode.PgSqlNode ? subLink.Testexpr : null;
                        columnRefToCompare = nodeToCompare?.NodeCase == Node.NodeOneofCase.ColumnRef ? nodeToCompare.ColumnRef : null;

                        break;

                    default:
                        break;
                }

                if (++depth >= stackList.Count)
                {
                    break;
                }

                currentNode = parentNode;
                parentNode = stackList[depth];

                if (columnRefToCompare is null)
                    continue;

                var columnRef = columnRefToCompare;
                var refNameList = columnRef.Fields.Select(field => field.String.Sval).ToList();

                switch (refNameList.Count)
                {
                    case 2:
                        columnRefName = refNameList[1];
                        comparableTable = base.SearchTableInScopeStack(refNameList[0], columnRefName);
                        break;
                    case 1:
                        columnRefName = refNameList[0];
                        comparableTable = base.SearchTableInScopeStack(null, columnRefName);
                        break;
                }

                break;
            }

            if (comparableTable is null)
            {
                return;
            }

            var indices = comparableTable?.Indices.Where(index => index.Columns.Any(col => col.ColumnName == columnRefName));
            var column = comparableTable.Columns.FirstOrDefault(col => col.ColumnName == columnRefName);

            var parameterTypeCastAnalyzeModel = new ParameterTypeCastAnalyzeModel
            {
                ParameterName = parameterName,
                ComparableDBColumn = new PgQuery.AnalyzerLib.GenericWalkers.Models.SemanticAnalyzer.Results.ComparableDBColumn
                {
                    SchemaName = comparableTable.DBSchemaModel.Name,
                    TableName = comparableTable.Name,
                    ColumnName = column.ColumnName,
                    ColumnType = column.TypeName,
                    TypeMod = column.TypeMode,
                    HasIndex = indices.Any(),
                },
            };

            if (typeCastStmt != null)
            {
                parameterTypeCastAnalyzeModel.HasCast = true;
                parameterTypeCastAnalyzeModel.TypeCastName = typeCastStmt.TypeName.Names.LastOrDefault()?.String.Sval;
                parameterTypeCastAnalyzeModel.TypeCastMod = typeCastStmt.TypeName.Typmods.FirstOrDefault()?.AConst?.Ival.Ival.ToString();
                parameterTypeCastAnalyzeModel.IsArray = typeCastStmt.TypeName.ArrayBounds.Any();
            }
            else
            {
                parameterTypeCastAnalyzeModel.HasCast = false;
            }

            analyzeModels.Add(parameterTypeCastAnalyzeModel);
        }

        public override void ProcessAExpr_DirectTraversal(PgGenericNode node)
        {
            base.ProcessAExpr_DirectTraversal(node);


        }

        public List<ParameterTypeCastAnalyzeModel> GetResult()
        {
            return analyzeModels;
        }
    }
}
