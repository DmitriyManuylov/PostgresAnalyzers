using Google.Protobuf;
using PgQuery;
using PgQueryAnalyzerLib.AnalyzeContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PgQuery.AnalyzerLib.GenericWalkers.WalkerBase
{
    public abstract partial class GenericPgTreeWalkerBase
    {
        private void VisitDefElem(DefElem defElem, StmtsProcessingContext context)
        {
            switch (defElem.Defname)
            {
                case "as":
                    var text = defElem.Arg.List.Items.First().String.Sval;
                    break;

                case "language":

                    break;
            }
        }
    }
}
