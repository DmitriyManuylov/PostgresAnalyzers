using Analyzers.Models.DDLAnalyzer;
using Analyzers.Models.DDLAnalyzer.Enums;
using PgQuery;
using PgQueryAnalyzerLib.AnalyzeContext;
using PgQueryAnalyzerLib.GenericWalkers;
using PgQueryAnalyzerLib.Models;
using System.Reflection;

namespace DDLOpsAnalyzer.Analyzer
{
    public class DDLAnalyzer : RisingCumulativeAnalyzerBase<DDLAnalyzeModel>
    {
        List<FileCheckModel> fileCheckList = new List<FileCheckModel>();

        public DDLAnalyzer(StmtsProcessingContext context) : base(context)
        {

        }

        protected override void ProcessAlterTableStmt_DirectTraversal(PgGenericNode node)
        {
            base.ProcessAlterTableStmt_DirectTraversal(node);

            List<FileCheckModel> result = new List<FileCheckModel>();

            var alterTableStmt = node.PgSqlNode.AlterTableStmt;

            var cmdList = alterTableStmt.Cmds;

            string tableName = alterTableStmt.Relation.Relname;
            string schemaName = alterTableStmt.Relation.Schemaname;

            foreach (var cmd in cmdList)
            {
                var cmdAlterSubtype = cmd.AlterTableCmd;
                FileCheckModel? check = default;

                switch (cmdAlterSubtype.Subtype)
                {
                    case AlterTableType.AtAddColumn:
                        check = this.AnalyzeAddColumnStmt(cmdAlterSubtype);
                        break;

                    case AlterTableType.AtDropColumn:
                        check = this.AnalyzeDropColumnStmt(cmdAlterSubtype);
                        break;

                    case AlterTableType.AtAlterColumnType:
                        check = this.AnalyzeAlterColumnTypeStmt(cmdAlterSubtype);
                        break;

                    case AlterTableType.AtDropNotNull:
                        check = new FileCheckModel
                        {
                            CheckResultType = FileCheckResultType.Dangerous,
                            CheckComment = $"Удаление ограничения not null для колонки \"{cmdAlterSubtype.Name}\"",
                        };
                        break;

                    case AlterTableType.AtDropConstraint:
                        check = new FileCheckModel()
                        {
                            CheckResultType = FileCheckResultType.Dangerous,
                            CheckComment = $"Удаление ограничения {cmdAlterSubtype.Name}",
                        };
                        break;

                    default:
                        break;
                }

                if (check is not null)
                {
                    result.Add(check);
                }
            }

            foreach (var item in result)
            {
                item.CheckComment = $"Таблица \"{schemaName}.{tableName}\". {item.CheckComment}";
            }
        }

        private FileCheckModel AnalyzeAlterColumnTypeStmt(AlterTableCmd cmdAlterSubtype)
        {
            string colName = cmdAlterSubtype.Name;
            string? typeName = cmdAlterSubtype.Def.ColumnDef.TypeName.Names.LastOrDefault()?.String.Sval;

            var result = new FileCheckModel
            {
                CheckComment = $"Изменение типа данных колонки \"{colName}\"",
                CheckResultType = FileCheckResultType.Dangerous,
            };

            return result;
        }

        private FileCheckModel AnalyzeDropColumnStmt(AlterTableCmd cmdAlterSubtype)
        {
            return new FileCheckModel
            {
                CheckComment = $"Удаление колонки {cmdAlterSubtype.Name}",
                CheckResultType = FileCheckResultType.Dangerous,
            };
        }

        private FileCheckModel AnalyzeAddColumnStmt(AlterTableCmd alterTableCmd)
        {
            var colDef = alterTableCmd.Def.ColumnDef;

            var colName = colDef.Colname;
            var typeName = colDef.TypeName.Names.LastOrDefault()?.String.Sval;
            var constraints = colDef.Constraints;

            var result = new FileCheckModel()
            {
                CheckComment = $"Добавление колонки \"{colName}\"",
                CheckResultType = FileCheckResultType.Minor,
            };

            return result;
        }
    }
}
