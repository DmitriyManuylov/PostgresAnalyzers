using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryAnalyzerLib.Services.Models.DbModels
{
    public class BaseModel
    {
        public BaseModel(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public BaseModel(string name)
        {
            Name = name;
        }
        public readonly int Id;
        public string Name { get; set; }
    }
}
