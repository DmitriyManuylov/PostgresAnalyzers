
using PgQueryAnalyzerLib.Services.Models.DbModels;
using PgQueryAnalyzerLib.Services.Models.DbModels.PlainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataChangeAnalyzer.Models.DBModels
{
    public class TableModel: BaseModel
    {
        public TableModel(int id, string name, int nspId, string nspName): base(id, name) 
        {
            DBSchemaModel = new DBSchemaModel(nspId, nspName);
        }
        public TableModel(string name, string nspName) : base(name)
        {
            DBSchemaModel = new DBSchemaModel(nspName);
        }
        public string Description { get; set; }
        public LinkedList<ForeignKeyMappingModel> ForeignKeyList { get; set; }
        //public List<TableModel> LinkedTables { get; set; }
        public List<(TableModel LinkedTable, ForeignKeyMappingModel ForeignKeyMappingModel)> LinkedTables { get; set; }
        public List<DBTriggerPlainModel> TableTriggers { get; set; }
        public DBSchemaModel DBSchemaModel { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not TableModel o)
            {
                throw new NotSupportedException("Тип объекта сравнения не совпадает с текущим");
            }

            return o.Name == Name && o.DBSchemaModel.Name == DBSchemaModel.Name;
        }

        public override int GetHashCode()
        {
            int hash = Name.GetHashCode();
            hash = 31*hash + DBSchemaModel.Name.GetHashCode();
            return hash;
        }
    }
}
