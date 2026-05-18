using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryAnalyzerLib.Models
{
    /// <summary>
    /// Модель сущности бд (таблицы, cte, подзапроса)
    /// </summary>
    public class DbEntityModel
    {
        /// <summary>
        /// Наименование схемы.
        /// </summary>
        public string SchemaName { get; set; }

        /// <summary>
        /// Наименование таблицы.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Псевдоним сущности.
        /// </summary>
        public string TableAlias { get; set; }
    }
}
