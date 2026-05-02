using PgQuery;
using PgQueryParseLib.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryParseLib.Models
{
    public enum PgNodeDialectType
    {
        PgSql,
        PlPgSql
    }

    public class PgGenericNode
    {
        private Node pgSqlNode;
        private PLpgSQL_stmt pLpgSQL_Stmt;
        public PgNodeDialectType PgNodeDialectType { get; private set; }
        public Node PgSqlNode
        {
            get
            {
                return pgSqlNode;
            }
            set
            {
                if (pLpgSQL_Stmt is not null)
                {
                    throw new PgNodeConstructException("Экземпляр может представлять только 1 тип узла");
                }

                PgNodeDialectType = PgNodeDialectType.PgSql;

                pgSqlNode = value;
            }
        }
        public PLpgSQL_stmt PLpgSQL_Stmt
        {
            get
            {
                return pLpgSQL_Stmt;
            }
            set
            {
                if(pgSqlNode is not null)
                {
                    throw new PgNodeConstructException("Экземпляр может представлять только 1 тип узла");
                }

                PgNodeDialectType = PgNodeDialectType.PlPgSql;

                pLpgSQL_Stmt = value;
            }
        }

        public PgGenericNode Parent { get; set; }
    }
}
