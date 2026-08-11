using DataChangeAnalyzer.Models.DBModels;
using Google.Protobuf;
using PgQuery;
using PgQuery.AnalyzerLib.GenericWalkers.WalkerBase;
using PgQuery.AnalyzerLib.Services.Models.DbModels.PlainModels;
using PgQueryAnalyzerLib.GenericWalkers;
using PgQueryAnalyzerLib.Models;
using PgQueryAnalyzerLib.Services.Models.DbModels.PlainModels;
using PgQueryParser;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.Json;
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

        public bool IgnoreFuncCalls { get; set; } = bool.TryParse(ConfigurationManager.AppSettings.Get("IgnoreFuncCalls"), out bool value) ? value : false;

        public List<string> QueryParameters { get; private set; }

        public GenericPgTreeWalker PgTreeWalker { get; set; }

        public Stack<PgGenericNode> PgGenericNodes { get; private set; }

        public HashSet<TableModel> DBTablesList { get; set; }
        public List<DBFunctionPlainModel> DBFunctionList { get; set; } = new List<DBFunctionPlainModel>();
        public Dictionary<string, DBFunctionPlainModel> DBFunctionDictionary { get; set; }
        public List<DBTriggerPlainModel> DbTriggerList { get; set; }
        public TableModel GetDBTableModel(string nspName, string tableName)
        {
            if (DBTablesList.TryGetValue(new TableModel(tableName, nspName), out var tableModel))
            {
                return tableModel;
            }

            throw new Exception($"Модель таблицы {nspName}.{tableName} не найдена");
        }

        public DBFunctionPlainModel GetDBFunctionPlainModel(string nspName, string funcName)
        {
            var result = DBFunctionDictionary.GetValueOrDefault($"{nspName}.{funcName}");

            if (result is null)
            {
                throw new Exception($"Модель функции {nspName}.{funcName} не найдена");
            }

            if (result.ParsedStmt is not null)
            {
                return result;
            }

            PostgreSqlQueryParser parser = new PostgreSqlQueryParser();

            var plPgParseTree = parser.GetPlPgQueryJsonParseTree(result.FuncDef!);
            List<JsonDocument> list = JsonSerializer.Deserialize<List<JsonDocument>>(plPgParseTree);
            var parsedPlPgSqlStmts = list.Select(item => PLpgSQL_stmt.Parser.ParseJson(item.RootElement.ToString())).ToList();
            var parseResult = parsedPlPgSqlStmts.First();

            if (parseResult.PLpgSQLFunction is null)
            {
                throw new Exception("Ошибка разбора тела функции");
            }

            result.ParsedStmt = parseResult;


            return result;
        }
        public TPgTreeWalker GetTreeWalkerByType<TPgTreeWalker>() where TPgTreeWalker : GenericPgTreeWalkerBase
        {
            return PgTreeWalker.GetTreeWalkerByType<TPgTreeWalker>();
        }

        public void ProcessDirectTraversal(PgGenericNode node)
        {
            PgGenericNodes.Push(node);
            PgTreeWalker.ProcessDirectTraversalInternal(node);
        }

        public PgGenericNode ProcessReverseTraversal(PgGenericNode node)
        {
            PgTreeWalker.ProcessReverseTraversalInternal(node);
            return PgGenericNodes.Pop();
        }

        public PgGenericNode GetNearestNodeByType(Node.NodeOneofCase nodeOneofCase)
        {
            List<PgGenericNode> list = this.PgGenericNodes.ToList();

            for (int i = 0; i < list.Count; i++)
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

            for (int i = 0; i < list.Count; i++)
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

            for (int i = 0; i < list.Count; i++)
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

            for (int i = 0; i < list.Count; i++)
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

        /// <summary>
        /// Проверка наличия циклических вызовов функций.
        /// </summary>
        /// <param name="nspName">Пространство имен функции.</param>
        /// <param name="funcName">Название функции.</param>
        /// <param name="node">Текущий узел дерева</param>
        /// <returns></returns>
        public bool CheckExistsFuncCallCycle(string nspName, string funcName, PgGenericNode node)
        {
            List<PgGenericNode> list = this.PgGenericNodes
                .Where(item =>
                    item.PgSqlNode?.NodeCase == Node.NodeOneofCase.FuncCall
                        &&
                    item.PgSqlNode.FuncCall.Funcname.Count == 2
                        &&
                    item != node
                    ).ToList();

            var names = list.Select(item => new
            {
                nspName = item.PgSqlNode.FuncCall.Funcname[0].String.Sval,
                funcName = item.PgSqlNode.FuncCall.Funcname[1].String.Sval
            });

            return names.Any(item => item.nspName == nspName && item.funcName == funcName);
        }


    }
}
