using DataChangeAnalyzer.Models.DBModels;
using PgQuery;
using PgQueryParseLib.GenericWalkers;
using PgQueryParseLib.Models;
using PgQueryParseLib.Services.Models.DbModels.PlainModels;
using PgQueryParseLib.StmtsVisit.ExprsVisitors;
using PgQueryParseLib.StmtsVisit.StmtsVisitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgQueryParseLib.AnalyzeContext
{

    public class StmtsProcessingContext
    {

        public StmtsProcessingContext()
        {

        }

        public GenericPgTreeWalker PgTreeWalker { get; private set; }

        public Stack<PgGenericNode> PgGenericNodes { get; private set; }

        Dictionary<TableModel, TableModel> DBTablesList { get; set; }
        public List<DBFunctionPlainModel> DBFunctionList { get; set; }
        public List<DBTriggerPlainModel> DbTriggerList { get; set; }
        public TableModel GetDBTableModel(string nspName, string tableName)
        {
            if(DBTablesList.TryGetValue(new TableModel(tableName, nspName), out var tableModel))
            {
                return tableModel;
            }

            throw new Exception($"Модель таблицы {nspName}.{tableName} не найдена");
        }

        public DBFunctionPlainModel GetDBFunctionPlainModel(string nspName, string funcName)
        {
            var result = DBFunctionList.Where(item => item.NspName == nspName && item.FuncName == funcName).FirstOrDefault();

            if(result is null)
            {
                throw new Exception($"Модель функции {nspName}.{funcName} не найдена");
            }

            if(result.ParsedStmt is not null)
            {
                return result;
            }

            PostgreSqlQueryParser parser = new PostgreSqlQueryParser();

            var plPgParseTree = parser.GetPlPgQueryJsonParseTree(result.FuncDef!);

            var parseResult = FunctionWrapper.Parser.ParseJson(plPgParseTree);

            if (parseResult.PLpgSQLFunction is null)
            {
                throw new Exception("Ошибка разбора тела функции");
            }

            result.ParsedStmt = parseResult.PLpgSQLFunction;


            return result;
        }
        public TPgTreeWalker GetTreeWalkerByType<TPgTreeWalker>() where TPgTreeWalker: GenericPgTreeWalkerBase
        {
            return PgTreeWalker.GetTreeWalkerByType<TPgTreeWalker>();
        }

        public void ProcessDirectTraversal(PgGenericNode node)
        {
            PgGenericNodes.Push(node);
        }

        public PgGenericNode ProcessReverseTraversal(PgGenericNode node)
        {
            return PgGenericNodes.Pop();
        }


    }
}
