using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryParseLib.Services.Models.DbModels
{
    public class DBSchemaModel: BaseModel
    {
        public DBSchemaModel(int id, string name): base(id, name) 
        {
            
        }

        public DBSchemaModel(string name) : base(name)
        {

        }
    }
}
