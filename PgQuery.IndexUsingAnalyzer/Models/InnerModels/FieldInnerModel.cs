using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexUsingAnalyzer.Models.InnerModels
{
    internal class FieldInnerModel
    {
        public string TypeNsp { get; set; }

        public string TypeName { get; set; }

        public TableInnerModel TableModel { get; set; }

        public string FieldName { get; set; }
    }
}
