using DataChangeAnalyzer.Models.DBModels;
using Google.Protobuf;
using PgQuery;
using PgQueryAnalyzerLib.GenericWalkers;
using PgQueryAnalyzerLib.Models;
using PgQueryAnalyzerLib.Services.Models.DbModels.PlainModels;
using PgQueryAnalyzerLib.StmtsVisit.ExprsVisitors;
using PgQueryAnalyzerLib.StmtsVisit.StmtsVisitors;
using PgQueryParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PgQueryAnalyzerLib.AnalyzeContext
{

    public class StmtsProcessingContext
    {

        public StmtsProcessingContext(List<string> queryParameters)
        {
            PgGenericNodes = new();
            QueryParameters = queryParameters;
        }

        public List<string> QueryParameters { get; private set; }

        public GenericPgTreeWalker PgTreeWalker { get; set; }

        public Stack<PgGenericNode> PgGenericNodes { get; private set; }

        HashSet<TableModel> DBTablesList { get; set; }
        public List<DBFunctionPlainModel> DBFunctionList { get; set; } = new List<DBFunctionPlainModel>();
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
            var result = DBFunctionList.FirstOrDefault(item => item.NspName == nspName && item.FuncName == funcName);

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
        public TPgTreeWalker GetTreeWalkerByType<TPgTreeWalker>() where TPgTreeWalker : GenericPgTreeWalkerBase
        {
            return PgTreeWalker.GetTreeWalkerByType<TPgTreeWalker>();
        }

        public void ProcessDirectTraversal(PgGenericNode node)
        {
            PgGenericNodes.Push(node);
            PgTreeWalker.ProcessDirectTraversal(node);
        }

        public PgGenericNode ProcessReverseTraversal(PgGenericNode node)
        {
            PgTreeWalker.ProcessReverseTraversal(node);
            return PgGenericNodes.Pop();
        }

        public PgGenericNode GetNearestNodeByType(Node.NodeOneofCase nodeOneofCase)
        {
            List<PgGenericNode> list = this.PgGenericNodes.ToList();

            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].PgSqlNode?.NodeCase == nodeOneofCase)
                {
                    return list[i];
                }
            }

            return null;
        }

        public PgGenericNode GetNearestNodeByType(PLpgSQL_stmt.StmtOneofCase stmtOneofCase)
        {
            List<PgGenericNode> list = this.PgGenericNodes.ToList();

            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].PLpgSQL_Stmt?.StmtCase == stmtOneofCase)
                {
                    return list[i];
                }
            }

            return null;
        }

        public PgGenericNode GetNearestNodeByType(Predicate<PgGenericNode> predicate, Node.NodeOneofCase? nodeOneofCase = null)
        {
            List<PgGenericNode> list = this.PgGenericNodes.ToList();

            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (nodeOneofCase is not null && list[i].PgSqlNode?.NodeCase != nodeOneofCase)
                {
                    continue;
                }

                if (predicate(list[i]))
                {
                    return list[i];
                }

            }

            return null;
        }

        public PgGenericNode GetNearestNodeByType(Predicate<PgGenericNode> predicate, PLpgSQL_stmt.StmtOneofCase? stmtOneofCase)
        {
            List<PgGenericNode> list = this.PgGenericNodes.ToList();

            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (stmtOneofCase is not null && list[i].PLpgSQL_Stmt?.StmtCase != stmtOneofCase)
                {
                    continue;
                }

                if (predicate(list[i]))
                {
                    return list[i];
                }

            }

            return null;
        }
    }
}
