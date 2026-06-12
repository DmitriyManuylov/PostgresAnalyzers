using PgQueryAnalyzerLib.Utilities;

namespace PgQueryAnalyzerLib.Services.Models.DbModels.PlainModels
{
    public class ForeignKeyMappingModel
    {
        public int DependentTableSchemaOid { get; set; }
        public string? DependentTableSchemaName { get; set; }
        public int DependentTableOid { get; set; }
        public string? DependentTableName { get; set; }
        public string? DependentColumnDatatype { get; set; }


        public string? DependentTableFullName { get; set; }

        public int DependentColumnNum { get; set; }
        public string? DependentColumnName { get; set; }

        public int ConstraintOid { get; set; }
        public string? ConstraintName { get; set; }
        public string? ConstraintType { get; set; }
        public short[] ConstraintDependentNum { get; set; }
        public short[] ConstraintRelatedNum { get; set; }

        public int RelatedTableSchemaOid { get; set; }
        public string? RelatedTableSchemaName { get; set; }
        public int RelatedTableOid { get; set; }
        public string? RelatedTableName { get; set; }


        public string? RelatedTableFullName { get; set; }
        public int RelatedColumnNum { get; set; }
        public string? RelatedColumnName { get; set; }
        public string? RelatedColumnDatatype { get; set; }

        public override string ToString()
        {
            return this.ConvertObjectToString();
        }
    }
}