using PgQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryAnalyzerLib.Services.Models.DbModels.PlainModels
{
    public class DBTriggerPlainModel
    {
        public string TriggerName { get; set; }
        public string TriggerScope { get; set; }
        public string TriggerTiming { get; set; }
        public string TriggerAction{ get; set; }
        public string PKTableNsp { get; set; }
        public string FKTableNsp { get; set; }
        public string PKTableName { get; set; }
        public string FKTableName { get; set; }
        public string ConstraintName { get; set; }
        public string FKColumn { get; set; }
        public string PKColumn { get; set; }
        public string PKColumnType { get; set; }
        public string FKColumnType { get; set; }
        public string IndexName { get; set; }
        public string TriggerProcOid { get; set; }
        public string TriggerProcName { get; set; }
        public string TriggerProcNspOid { get; set; }
        public string TriggerProcNspName { get; set; }
    }
}
