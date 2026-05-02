using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryParser.LibPgQueryModels
{
    public class ParseError
    {
        public string Message { get; set; }
        public string Funcname { get; set; }
        public string Filename { get; set; }
        public int Lineno { get; set; }
        public int Cursorpos { get; set; }
        public string Context { get; set; }
    }
}
