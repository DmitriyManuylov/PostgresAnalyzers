using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryAnalyzerLib.Services.Models.DbModels
{
    public class DBFunctionModel: BaseModel
    {
        public DBSchemaModel Schema { get; set; }
        public DBFunctionModel(int id, string name) : base(id, name)
        {
            
        }
    }
}
