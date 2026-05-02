using PgQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryParseLib.Services.Models.DbModels.PlainModels
{
    public class DBFunctionPlainModel
    {
        public string? NspOid { get; set; }
        public string? NspName { get; set; }
        public string? FuncOid { get; set; }
        public string? FuncName { get; set; }
        public string? FuncDef{ get; set; }
        public PLpgSQL_function? ParsedStmt { get; set; }
    }
}
