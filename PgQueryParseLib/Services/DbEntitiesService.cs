using Dapper;
using DataChangeAnalyzer.Models.DBModels;
using Npgsql;
using PgQueryAnalyzerLib.Services.Models.DbModels;
using PgQueryAnalyzerLib.Services.Models.DbModels.PlainModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PgQueryAnalyzerLib.Services
{
    class DbEntitiesService
    {
        IDbConnection connection;
        string _connectionString = ConfigurationManager.ConnectionStrings["PostgresDB"].ConnectionString;
        List<string> _foreignKeys;
        Dictionary<string, string[]> _schemasDictionary;
        List<ForeignKeyMappingModel> _foreignKeyMappingModels = new List<ForeignKeyMappingModel>();

        const string _foreignKeysStructureQuery = @"
select
    -- таблица, содержащая внешний ключ
      dependent_namespace.oid		   	as dependentTableSchemaOid
    , dependent_namespace.nspname	   	as dependentTableSchemaName
    , dependent_table.oid 	   			as dependentTableOid 		--oid
    , dependent_table.relname  			as dependentTableName		--name	
    , dependent_namespace.nspname 
      || '.' || 
      dependent_table.relname 			as dependentTableFullName	
    -- столбец, для которого задано ограничение
    , attr.attnum 			   			as dependentColumnNum
    , attr.attname 			   			as dependentColumnName
    , dependent_type.typname            as dependentColumnDatatype
    -- информация об ограничении (внешнем ключе)
    , contsr.oid 			   			as constraintOid
    , contsr.conname		   			as constraintName
    , contsr.contype					as constraintType
    , contsr.conkey			   			as constraintDependentNum --номер столбца с ограничением(для внешнего ключа - номер в зависимой таблице)
    , contsr.confkey 					as constraintRelatedNum   --номер столбца
    -- связанная таблица
    , rel_namespace.oid		   			as relatedTableSchemaOid
    , rel_namespace.nspname	   			as relatedTableSchemaName
    , rel_table.oid 		   			as relatedTableOid
    , rel_table.relname 	  			as relatedTableName
    , rel_namespace.nspname 
      || '.' || 
      rel_table.relname 				as relatedTableFullName
    -- столбец-первичный ключ связанной таблицы
    , rel_attr.attnum					as relatedColumnNum
    , rel_attr.attname 					as relatedColumnName
    , rel_type.typname 					as relatedColumnDatatype
from pg_catalog.pg_attribute attr
    inner join pg_catalog.pg_type dependent_type on dependent_type.oid = attr.atttypid
	inner join pg_catalog.pg_constraint contsr on attr.attnum = any(contsr.conkey) and attr.attrelid = contsr.conrelid --and contsr.contype = 'f'
	inner join pg_class dependent_table on dependent_table.oid = contsr.conrelid
	inner join pg_namespace dependent_namespace on dependent_namespace.oid = dependent_table.relnamespace
	inner join pg_catalog.pg_class rel_table on rel_table.oid = contsr.confrelid
	left join pg_catalog.pg_attribute rel_attr on rel_attr.attrelid = rel_table.oid and rel_attr.attnum = any(contsr.confkey)
	left join pg_catalog.pg_type rel_type on rel_type.oid = rel_attr.atttypid 
	left join pg_catalog.pg_namespace rel_namespace on rel_namespace.oid = rel_table.relnamespace
where attr.attrelid in (select oid from pg_catalog.pg_class tbl where
	tbl.oid in (select oid from pg_class tables where tables.relkind = 'r'
	--and tables.relnamespace in (select oid from pg_namespace pg_nsp where pg_nsp.nspname = any(:Schemas))))
order by 
	  dependent_table.relname
	, contsr.conkey
	, contsr.confkey";

        public DbEntitiesService()
        {

            if (!File.Exists("Schemas.json"))
            {
                return;
            }

            FileStream fileStream = new FileStream("Schemas.json", FileMode.Open);
            using (fileStream)
            {
                _schemasDictionary = JsonSerializer.Deserialize<Dictionary<string, string[]>>(fileStream);
            }
        }

        public async Task<List<ForeignKeyMappingModel>> DownloadDBStructureAsync()
        {
            IEnumerable<ForeignKeyMappingModel> foreignKeys;

            string sql = _foreignKeysStructureQuery;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                foreignKeys = await connection.QueryAsync<ForeignKeyMappingModel>(_foreignKeysStructureQuery, new { Schemas = _schemasDictionary["Schemas"] });

                var fks = connection.Query<ForeignKeyMappingModel>(sql);

                DynamicParameters parameters = new DynamicParameters();

                var obj = new { innerObj = new { i = 5, j = 1 }, Q = 6 };

                parameters.Add("Schemas", _schemasDictionary["Schemas"], DbType.Object);

                var fks2 = connection.Query<ForeignKeyMappingModel>(sql, parameters);
            }
            return foreignKeys.ToList();

        }

        public async Task<List<DBFunctionPlainModel>> DownloadDBFunctionsAsync()
        {
            #region sql
            const string sql = @"
select
    fun_nsp.NspName,
    fun_nsp.oid as NspOid,
    functions.oid as FuncOid,
    functions.proname as FuncName,
    pg_get_functiondef(fun_nsp.oid) as FuncDef
from pg_catalog.pg_proc functions
inner join pg_catalog.pg_namespace fun_nsp on fun_nsp.oid = functions.pronamespace
where
    functions.prokind = 'f'
";
            #endregion
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var result = await connection.QueryAsync<DBFunctionPlainModel>(sql);
                return result.ToList();
            }
        }

        public async Task<List<DBTriggerPlainModel>> DownloadDBTriggersAsync()
        {
            const string sql = @"
select
	trg.tgname as TriggerName,
    nsp_pk.nspname as PKTableNsp,
    nsp_fk.nspname as FKTableNsp,
    tbl_pk.relname as PKTableName,
    tbl_fk.relname as FKTableName,
    constr.conname as ConstraintName,
    column_fk.attname as FKColumn,
    col_pk.attname as PKColumn,
    fk_type.typname as PKColumnType
    fk_type.typname as FKColumnType,
    ind_class.relname as IndexName,
    trigger_proc.oid as TriggerProcOid,
    trigger_proc.proname as TriggerProcName,
    tr_proc_nsp.oid as TriggerProcNspOid,
    tr_proc_nsp.nspname as TriggerProcNspName,
    CASE WHEN (tgtype::int::bit(7) & b'0000001')::int = 0 THEN 'STATEMENT' ELSE 'EACH ROW' END
                                     as TriggerScope,
    COALESCE(
      CASE WHEN (tgtype::int::bit(7) & b'0000010')::int = 0 THEN NULL ELSE 'BEFORE' END,
      CASE WHEN (tgtype::int::bit(7) & b'0000000')::int = 0 THEN 'AFTER' ELSE NULL END,
      CASE WHEN (tgtype::int::bit(7) & b'1000000')::int = 0 THEN NULL ELSE 'INSTEAD' END,
      ''
    )::text                           as TriggerTiming, 
      (CASE WHEN (tgtype::int::bit(7) & b'0000100')::int = 0 THEN '' ELSE ' INSERT' END) ||
      (CASE WHEN (tgtype::int::bit(7) & b'0001000')::int = 0 THEN '' ELSE ' DELETE' END) ||
      (CASE WHEN (tgtype::int::bit(7) & b'0010000')::int = 0 THEN '' ELSE ' UPDATE' END) ||
      (CASE WHEN (tgtype::int::bit(7) & b'0100000')::int = 0 THEN '' ELSE ' TRUNCATE' END)
                                       as TriggerAction
from pg_catalog.pg_trigger trg
inner join pg_catalog.pg_class tbl_pk on tbl_pk.oid = trg.tgrelid
inner join pg_catalog.pg_namespace nsp_pk on nsp_pk.oid = tbl_pk.relnamespace
left join pg_catalog.pg_class tbl_fk on tbl_fk.oid = trg.tgconstrrelid
left join pg_catalog.pg_namespace nsp_fk on nsp_fk.oid = tbl_fk.relnamespace
left join pg_catalog.pg_constraint constr on constr.conrelid = trg.tgconstrrelid and constr.confrelid = tbl_pk.oid
left join pg_catalog.pg_attribute column_fk on column_fk.attrelid = constr.conrelid and column_fk.attnum = any(constr.conkey)-- and constr.contype = 'f'
left join pg_catalog.pg_attribute col_pk on col_pk.attrelid = constr.confrelid  and col_pk.attnum = any(constr.confkey)
left join pg_catalog.pg_index ind on column_fk.attnum = any(ind.indkey) and ind.indrelid = tbl_fk.oid
left join pg_catalog.pg_class ind_class on ind_class.oid = ind.indexrelid
inner join pg_catalog.pg_type pk_type on pk_type.oid = col_pk.atttypid
left join pg_catalog.pg_type fk_type on fk_type.oid = column_fk.atttypid
left join pg_catalog.pg_proc trigger_proc on trigger_proc.oid = trg.tgfoid
left join pg_catalog.pg_namespace tr_proc_nsp on tr_proc_nsp.oid = trigger_proc.pronamespace
";

            using (var dbConnection = new NpgsqlConnection(_connectionString))
            {
                await dbConnection.OpenAsync();

                var result = await dbConnection.QueryAsync<DBTriggerPlainModel>(sql);

                return result.ToList();
            }
        }

        public async Task<Dictionary<TableModel, TableModel>> BuildGraph()
        {
            _foreignKeyMappingModels = await DownloadDBStructureAsync();
            //var funcs = await DownloadDBFunctionsAsync();

            var triggers = await DownloadDBTriggersAsync();

            if(_foreignKeyMappingModels is null)
            {
                throw new ArgumentNullException();
            }

            Dictionary<TableModel, TableModel> dict = new Dictionary<TableModel, TableModel>();

            LinkedList<(List<TableModel>, List<ForeignKeyMappingModel>)> nodes = new LinkedList<(List<TableModel>, List<ForeignKeyMappingModel>)>();
            //List<GraphEdge<TableModel, ForeignKeyMappingModel>> edges = new List<GraphEdge<TableModel, ForeignKeyMappingModel>>(_foreignKeyMappingModels.Count);
            LinkedList<int> nodesIndices = new LinkedList<int>();
            LinkedList<LinkedList<int>> indicesLinks = new LinkedList<LinkedList<int>>();
            int insertingNodeIndex = 0;
            foreach(var model in _foreignKeyMappingModels)
            {
                //var model = _foreignKeyMappingModels[i];
                TableModel dependentTable = new TableModel(model.DependentTableOid, model.DependentTableName!, model.DependentTableSchemaOid, model.DependentTableSchemaName!);

                TableModel relatedTable = new TableModel(model.RelatedTableOid, model.RelatedTableName!, model.RelatedTableSchemaOid, model.RelatedTableSchemaName!);


                if (!dict.TryGetValue(dependentTable, out TableModel existedDependentTableModel))
                {
                    dict.Add(dependentTable, dependentTable);
                    dependentTable.LinkedTables = new List<(TableModel LinkedTable, ForeignKeyMappingModel ForeignKeyMappingModel)>();
                    dependentTable.ForeignKeyList = new LinkedList<ForeignKeyMappingModel>();

                    dependentTable.TableTriggers.AddRange(triggers.Where(item => item.PKTableNsp == dependentTable.DBSchemaModel.Name && item.PKTableName == dependentTable.Name));
                }
                else
                {
                    dependentTable = existedDependentTableModel;
                }

                dependentTable.ForeignKeyList.AddLast(model);

                if (!dict.TryGetValue(relatedTable, out TableModel existedRelatedTableModel))
                {
                    dict.Add(relatedTable, relatedTable);
                    relatedTable.LinkedTables = new List<(TableModel LinkedTable, ForeignKeyMappingModel ForeignKeyMappingModel)>();
                    relatedTable.ForeignKeyList = new LinkedList<ForeignKeyMappingModel>();

                    relatedTable.TableTriggers.AddRange(triggers.Where(item => item.PKTableNsp == relatedTable.DBSchemaModel.Name && item.PKTableName == relatedTable.Name));
                }
                else
                {
                    relatedTable = existedRelatedTableModel;
                }
                dependentTable.LinkedTables.Add((relatedTable, model));

            }

            return dict;
        }


    }
}
