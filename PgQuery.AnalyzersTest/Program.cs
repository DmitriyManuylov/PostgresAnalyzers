using AnalyzeManagers;
using DMLOpsAnalyzer.Analyzer;
using Google.Protobuf;
using PgQuery;
using PgQueryAnalyzerLib;
using PgQueryAnalyzerLib.AnalyzeContext;
using PgQueryAnalyzerLib.GenericWalkers;
using PgQueryParser;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace PostgresAnalyzersTest
{
    internal class Program
    {
        const string query1 = "select dependent_namespace.oid		   	as dependentTableSchemaOid, dependent_namespace.nspname	   	as dependentTableSchemaName, dependent_table.oid 	   			as dependentTableOid, dependent_table.relname  			as dependentTableName, dependent_namespace.nspname || '.' || dependent_table.relname 			as dependentTableFullName, attr.attnum 			   			as dependentColumnNum, attr.attname 			   			as dependentColumnName, contsr.oid 			   			as constraintOid, contsr.conname		   			as constraintName, contsr.contype					as constraintType, contsr.conkey			   			as constraintDependentNum, contsr.confkey 					as constraintRelatedNum, rel_namespace.oid		   			as relatedTableSchemaOid, rel_namespace.nspname	   			as relatedTableSchemaName, rel_table.oid 		   			as relatedTableOid , rel_table.relname 	  			as relatedTableName , rel_namespace.nspname || '.' || rel_table.relname 				as relatedTableFullName , rel_attr.attnum					as relatedColumnNum , rel_attr.attname 					as relatedColumnName , rel_type.typname 					as relatedColumnDatatype from pg_catalog.pg_attribute attr inner join pg_catalog.pg_constraint contsr on attr.attnum = any(contsr.conkey) and attr.attrelid = contsr.conrelid inner join pg_class dependent_table on dependent_table.oid = contsr.conrelid inner join pg_namespace dependent_namespace on dependent_namespace.oid = dependent_table.relnamespace inner join pg_catalog.pg_class rel_table on rel_table.oid = contsr.confrelid left join pg_catalog.pg_attribute rel_attr on rel_attr.attrelid = rel_table.oid and rel_attr.attnum = any(contsr.confkey) left join pg_catalog.pg_type rel_type on rel_type.oid = rel_attr.atttypid left join pg_catalog.pg_namespace rel_namespace on rel_namespace.oid = rel_table.relnamespace where attr.attrelid in (select oid from pg_catalog.pg_class tbl where tbl.oid in (select oid from pg_class tables where tables.relkind = 'r' and tables.relnamespace in (select oid from pg_namespace pg_nsp where pg_nsp.nspname = any(@Schemas)))) order by dependent_table.relname, contsr.conkey, contsr.confkey";
        const string query = @"select
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
	inner join pg_catalog.pg_constraint contsr on attr.attnum = any(contsr.conkey) and attr.attrelid = contsr.conrelid --and contsr.contype = 'f'
	inner join pg_class dependent_table on dependent_table.oid = contsr.conrelid
	inner join pg_namespace dependent_namespace on dependent_namespace.oid = cast(dependent_table.relnamespace as varchar)
	inner join pg_catalog.pg_class rel_table on rel_table.oid = contsr.confrelid
	left join pg_catalog.pg_attribute rel_attr on rel_attr.attrelid = rel_table.oid and rel_attr.attnum = any(contsr.confkey)
	left join pg_catalog.pg_type rel_type on rel_type.oid = rel_attr.atttypid
	left join pg_catalog.pg_namespace rel_namespace on rel_namespace.oid = rel_table.relnamespace
where attr.attrelid in (select oid from pg_catalog.pg_class tbl where
	tbl.oid in (select oid from pg_class tables where tables.relkind = 'r'
	and tables.relnamespace in (select oid from pg_namespace pg_nsp where pg_nsp.nspname = any(:Schemas))))
order by
	  dependent_table.relname
	, contsr.conkey
	, contsr.confkey";

        const string func = @"
CREATE OR REPLACE FUNCTION get_all_foo() RETURNS SETOF foo AS
$BODY$
DECLARE
    r foo%rowtype;
    num integer;
    num2 integer;
BEGIN
    num := mir.do_something() + 8;

    perform mir.do_anything();
    num2 := 7;
    FOR r IN 1..200
        --SELECT * FROM foo WHERE fooid > 0
    LOOP
        if num > 5
        then
            continue;
        end if;
        -- can do some processing here
        num := 44;
        RETURN NEXT (select p.id, p.pt_id from mir.presc p); -- return current row of SELECT
    END LOOP;
    RETURN;

END
$BODY$
LANGUAGE plpgsql;";

        const string func_if = @"
CREATE OR REPLACE FUNCTION get_all_foo() RETURNS SETOF foo AS
$BODY$
DECLARE
    r foo%rowtype;
    num integer;
    num2 integer;
BEGIN
    num := mir.do_something() + 8;

    perform mir.do_anything();
    num2 := 7;
    if num > 7 then
        begin
            num2 = 10;
            select mir.fun() into num;
        end;
    elsif num > 15 then
        begin
            num2 = 15;
            select p.oid, p.pt into num, num2 from mir.presc p;
        end;
    elsif num > 20 then
        RAISE EXCEPTION '%1, %2', 124, 'QWERTY';
        raise notice '%1, %2', 124, 'QWERTY';
    else 
        case 
            when num > mir.get_num('qw')
                then num = 111;
            when num >= 5
                then num = 1 + 1;

            else
                num = 7;
                num = num + 3;
        end case;
        update mir.presc set pt_id = 'gfgf' where id = 'gjfg';
        num = 23;
    end if;
    RETURN;

END
$BODY$
LANGUAGE plpgsql;";

        const string f3 = "\r\nDECLARE\r\n    r foo%rowtype;\r\n    num integer;\r\n    num2 integer;\r\nBEGIN\r\n    num = do_something();\r\n    num2 = 7;\r\n    FOR r IN\r\n        SELECT * FROM foo WHERE fooid > 0\r\n    LOOP\r\n        -- can do some processing here\r\n        RETURN NEXT r -- return current row of SELECT\r\n    END LOOP;\r\n    RETURN;\r\n\r\nEND\r\n";
        const string q = "tre.teryt.ytry.f.num := mir.do_something(7, sch.my_func('q', 5))";
        const string q2 = "select mir.do_something() into variable";

        const string select_examp = @"
select 
cast(p.id as varchar(36)) id_1,
p.id::varchar(36) id_2,
p.pt_id,
p.*
from mir.presc p
inner join lateral(
    select 
        pt.oid, 
        pt.name 
    from mir.presctype pt 
    where
        pt.oid =  p.presctype_id) t on true
where
    p.id = 'fgsd'
";

        const string update_examp = @"
update mir.presc
set
    pt.pt_id = 'gtdf',
    upd_dt = '35434'
where 
    id = 'wtrw'
returning
    id,pt.pt_id,upd_dt
";

        const string insert_stmt = @"
insert into mir.presc
    (id, pt_id)
values
    ('fdgsdf', 'fdgdfj')
";

        const string insert_stmt2 = @"
insert into mir.presc
    (id, pt_id)
select
    id,
    pt_id
from mir.presc
    where uid = '324'
";

        const string cte_select = @"
with cte as (
    select
        p.id,
        p.pt_id
    from mir.presc p)

select 
    id as presc_id,
    pt_id as presctype_id
from cte
";

        const string query_sum = @"
select ((1 + 5) / 6) = 1";

        const string query_case = @"
select 
    case @p1
        when 1 then 2
        when 2 then 3
        else 5
    end as p1,
    case when @p2 = 1 then 2
        when @p2 = 2 then 3
        else 5
    end as p2
";

        const string selectFromFUnc = @"
select
    pr.id,
    pr.presctype_id,
    mdoc.num,
    pt.name
from mir.mdoc mdoc
inner join mir.get_prescs(mdoc.id) pr on true
--inner join mir.get_presctype(pr.presctype_id) pt on true
left join mir.presc presc on presc.mdoc_id = mdoc.id
left join lateral (
    select visit.id, visit.cr_dt 
    from mir.visit visit
    where visit.mdoc_id = mdoc.id) on true
where
    pr.id = :presc::char(36)
    and pr.pt_id = any (cast(:pt_id as char(36)[]))
    and pr.visit = cast(:visit as char(36))
";

        const string select = @"
select
    pr.id,
    pr.presctype_id,
    mdoc.num,
    pt.name,
    :visit
from mir.mdoc mdoc
inner join mir.get_prescs pr on true
--inner join mir.get_presctype pt on true
left join mir.presc presc on presc.mdoc_id = mdoc.id
left join lateral (
    select visit.id, visit.cr_dt 
    from mir.visit visit
    where visit.mdoc_id = mdoc.id
    and visit.people = :people) on true
where
    pr.id = :presc::char(36)
    and pr.pt_id = any (cast(:pt_id as char(36)[]))
    and mdoc.id = any(:mdoc_id::char(36)[])
    and pr.visit = cast(:visit as char(36))
    and pr.mdoc_id = :mdoc_id
";

        const string insertStmt3 = @"
insert into mir.presc
values
    (:id, :pt_id, :visit_id)";

        const string dfs = @"
do $$
begin
update mir.presc
set
    pt.pt_id = 'gtdf',
    upd_dt = '35434'
where 
    id = 'wtrw'
returning
    id,pt.pt_id,upd_dt;
end;
$$;";

        const string upd = @"
                
 
                SELECT sotr.oid,
                       people.lastname || ' ' || coalesce(substr(people.firstname, 1, 1) || '.', '') || ' ' || coalesce(substr(people.middlename, 1, 1) || '.', '') AS Fullname,
                       post.name AS postName,
                       otdel.name AS otdelName
                FROM mir.sotr sotr
                     INNER JOIN mir.otdel otdel ON otdel.oid = sotr.otdel
                     INNER JOIN mir.post post ON post.oid = sotr.post
                     INNER JOIN mir.people people ON people.oid = sotr.sysuser
                     INNER JOIN mir.sysuser sysuser ON sysuser.oid = sotr.sysuser AND sysuser.isactive = 1
                     LEFT JOIN auth.sotr_access_group sotr_access ON sotr_access.sotr = sotr.oid AND sotr_access.access_group =:qwert
                WHERE 1=1
			          AND coalesce(sotr.date_post_end, now()) >= now()
                      AND case when cast (:lpu as varchar) is not null then otdel.lpu = :lpu else 1 = 1 end
                      AND case when cast (:filter as varchar) is not null 
                          then  (people.lastname ilike('%' || :filter || '%')
                                 OR people.lastname ilike('%' || :filter || '%')
                                 OR people.firstname ilike('%' || :filter || '%')
                                 OR post.name ilike('%' || :filter || '%')
                                 OR otdel.name ilike('%' || :filter || '%')
                                 )
                          else 1 = 1 end
                      AND case when coalesce(:is_select, false) 
                          then sotr_access.access_group is null 
                          else sotr_access.access_group = :qwert
                          end  

";

        const string c = @"

 

DO $$
declare 
txt varchar;
BEGIN


with t_table as (
  SELECT 

  't' || row_number() over ( order by target_namespace.nspname,
           target.relname,
           target_columns.attname ) AS prefix,
  o.conname AS constraint_name,
         source_namespace.nspname source_schema,
         source.relname AS source_table,
         source_columns.attname AS source_column,
         target_namespace.nspname target_schema,
         target.relname AS target_table,
         target_columns.attname AS target_column,
         o.confdeltype ref_delete,
         o.confupdtype ref_update,
         target_columns.attnotnull AS target_column_not_null,
         (
           SELECT pg_attribute.attname
           FROM pg_index,
                pg_class,
                pg_attribute,
                pg_namespace
           WHERE 1 = 1
                 AND nspname = target_namespace.nspname
                 AND pg_class.relname = target.relname
                 AND indrelid = pg_class.oid
                 AND pg_class.relnamespace = pg_namespace.oid
                 AND pg_attribute.attrelid = pg_class.oid
                 AND pg_attribute.attnum = ANY (pg_index.indkey)
                 AND indisprimary
           LIMIT 1
         ) AS target_column_pk
  FROM pg_constraint o
       LEFT JOIN pg_class source ON source.oid = o.confrelid
       LEFT JOIN pg_class target ON target.oid = o.conrelid
       LEFT JOIN pg_namespace source_namespace ON source_namespace.oid = source.relnamespace
       LEFT JOIN pg_namespace target_namespace ON target_namespace.oid = target.relnamespace
       LEFT JOIN pg_attribute source_columns ON source_columns.attrelid = o.confrelid AND source_columns.attnum = ANY (o.confkey)
       LEFT JOIN pg_attribute target_columns ON target_columns.attrelid = o.conrelid AND target_columns.attnum = ANY (o.conkey)
  WHERE 1 = 1
        AND o.contype = 'f'
        AND source_namespace.nspname = 'anket'
        AND source.relname = 'ankets_result'
        AND not target.relname = 'ankets_result_answer'
  ORDER BY target_namespace.nspname,
           target.relname,
           target_columns.attname
)

, t_join as (
select 
  ' LEFT JOIN '|| target_schema || '.' || target_table ||' ' || prefix || ' on ' || prefix || '.'  || target_column || ' = ' || source_table || '.' || source_column
  as jointxt,
  
  ' and  '|| prefix || '.' || target_column ||' is null '
  as wheretxt  

from t_table
)


select 
'  delete from  anket.ankets_result where oid in ( '
' select ankets_result.oid  from anket.ankets_result ankets_result '
|| string_agg(jointxt, ' ')
|| ' where 1=1 ' || string_agg(wheretxt, ' ')
|| ' )'

  as selecttxt

from t_join
INTO txt;

execute txt;

END;
$$ 


";

        const string qq = @"SELECT r.oid as value,
       r.name as text
FROM mir.receptiontype r
    left join mir.lpu lpu ON lpu.oid = r.lpu
WHERE case when @lpu::bpchar(36) is not null then r.lpu = @lpu::bpchar(36) else 1 = 1 end
    and case when @name::varchar(36) is not null then name ilike ('%' || @name || '%')::varchar(36) else 1 = 1 end
    and case when @oid::bpchar(36) is not null then r.oid = @oid::bpchar(36) else 1=1 end
ORDER BY r.name";


        const string createIndex = @"
CREATE INDEX ix_services_name_group ON mir.services USING btree (service_group, upper((defaultname)::text))";

        const string sel = @"
select * from mir.presc where id = any (unnest(string_to_array(:pr, ',')::text[]))";

        const string sel2 = @"
SELECT diag_presc_model.*
                    FROM mir.diag_presc_model diag_presc_model
                         LEFT JOIN mir.diag diag ON diag.id = diag_presc_model.diag
                         LEFT JOIN std.model_mkb10 model_mkb10 ON model_mkb10.mkb10 = diag.icd10_id AND model_mkb10.model = diag_presc_model.model
                         LEFT JOIN std.model_mkb10 model_mkb10_ch ON model_mkb10_ch.model = model_mkb10.model
                    WHERE 1 = 1
                          AND diag = @diag
                          AND case when not cast(@newmkb10 as varchar) is null then model_mkb10_ch.mkb10 = @newmkb10 else 1=1 end";

        const string upd2 = @"
update mir.card_shelving
      set number = :number
    where oid = :oid
returning number";

        static void Main(string[] args)
        {
            TestDMLAnalyzer();
            AnalyzeParametersCast(upd2);
            //ParseQueries();
            //AnalyzeDMLOperations(dfs);
            //ParseQueries();
            //NewMethod();
        }

        private static void AnalyzeDMLOperations(string queryText)
        {
            AnalyzeManager manager = new AnalyzeManager(queryText);
            manager.AddDMLOperationsAnalyzer();
            manager.Analyze();

            var dmlAnalyzeResult = manager.GetDMLOperationsResult();
        }

        private static void AnalyzeParametersCast(string queryText)
        {
            AnalyzeManager manager = new AnalyzeManager(queryText);
            manager.AddParametersTypeCastAnalyzer();

            manager.Analyze();

            var analyzeRes = manager.GetParameterTypeCastAnalyzeResult();
        }

        private static void TestDMLAnalyzer()
        {
            string sql = @"

with del as 
    (delete from mir.insurance_service_place isp where isp.insurance = cast(@Insurance as char(36)) returning oid)
    insert into mir.insurance_service_place(oid,service_place_insurance,insurance)
    select public.generate_uuid_v4(),q.oid, @Insurance from (select unnest(cast(@ServPlaces as char(36)[])) as oid) q 
";

            AnalyzeManager manager = new AnalyzeManager(sql);
            manager.AddAnalyzer<DMLAnalyzer>();

            manager.Analyze();
            var result = manager.GetDMLOperationsResult();
        }

        private static void ParseQueries()
        {
            
            var parser = new PostgreSqlQueryParser();
            var insJson = parser.GetQueryParseTree(insert_stmt);
            var insJson2 = parser.GetQueryParseTree(insert_stmt2);
            var insJson3 = parser.GetQueryParseTree(insertStmt3);

            var qsum = parser.GetQueryParseTree(query_sum);

            var casequery = parser.GetQueryParseTree(query_case);

            var cte_selectJson = parser.GetQueryParseTree(cte_select);

            var selectJson = parser.GetQueryParseTree(select_examp);
            var updateJson = parser.GetQueryParseTree(update_examp);
            var selectFromFunJson = parser.GetQueryParseTree(selectFromFUnc);
            var selectArr = parser.GetQueryProtobufParseTree(select_examp);

            var selectPtb = ParseResult.Parser.ParseFrom(selectArr);

            //var pf3 = parser.GetPlPgQueryJsonParseTree(func);

            string assign = parser.GetQueryParseTreeWithOptions(q, 3);

            var assignPtb = ParseResult.Parser.ParseJson(assign);

            string queryPTree = parser.GetQueryParseTree(query);

            /*var queryProtobuf = PgQuery.ParseResult.Parser.ParseFrom(parser.GetQueryProtobufParseTree(query));*/

            var funcDef = parser.GetQueryParseTree(func_if);

            var funBody = parser.GetPlPgQueryJsonParseTree(func);

            var defPtb = PgQuery.ParseResult.Parser.ParseJson(funcDef);
            string trimmed = funBody.Trim('\n', '[', ']');
            var bodyPtb = PgQuery.FunctionWrapper.Parser.ParseJson(trimmed);

            var assignExpr = bodyPtb.PLpgSQLFunction.Action.PLpgSQLStmtBlock.Body[2].PLpgSQLStmtAssign.Expr.PLpgSQLExpr;

            var assignExprParseTreePtb = parser
                .GetQueryProtobufParseTreeWithOptions<ParseResult>(assignExpr.Query, (int)assignExpr.ParseMode);
            //var defBody = parser.GetQueryParseTreeWithOptions(defPtb.Stmts[0].Stmt.CreateFunctionStmt.Options[0].DefElem.Arg.List.Items[0].String.Sval, 0);
            //var bodyPtb = ParseResult.Parser.ParseJson(funBody.Trim('\n', '[', ']'));



            /*var funcPtb = ParseResult.Parser.ParseJson(funcDef.Trim('[', ']'));

            string s = funBody.Trim('[', ']');

            Console.WriteLine(funBody);
            Console.WriteLine(funcPtb);

            //var qt = parser.GetPlPgQueryJsonParseTree(q);
            var qt2 = parser.GetQueryParseTree(q2);*/
        }
    }
}
