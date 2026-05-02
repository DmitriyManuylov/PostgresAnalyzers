using PgQuery;
using PgQueryParseLib;
using System.Runtime.InteropServices;
using System.Text;
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
inner join mir.get_presctype(pr.presctype_id) pt on true
";
        static void Main(string[] args)
        {
            ParseQueries();
            //NewMethod();
        }

        private static void ParseQueries()
        {
            
            var parser = new PostgreSqlQueryParser();
            var insJson = parser.GetQueryParseTree(insert_stmt);
            var insJson2 = parser.GetQueryParseTree(insert_stmt2);

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

        private static void NewMethod()
        {
            var str = GetQueryString(query);

            unsafe
            {
                byte* result = GetQueryProtobufParseTree(str);

                byte* resJson = GetQueryParseTree(str);

                byte* res_PlPg = GetQueryParseTree(func);


                //byte* bt = (byte*)result;

                int code = *(int*)result;
                int codeJson = *(int*)resJson;
                int codePl = *(int*)res_PlPg;
                int strlength = *((int*)result + 1);
                int strlength2 = *((int*)resJson + 1);
                int strLenPl = *((int*)res_PlPg + 1);

                byte* messagePtr = result + 2 * sizeof(int);
                byte* curByte = messagePtr - 1;

                byte[] arr = new byte[strlength];
                byte[] arrJson = new byte[strlength2];
                byte[] arrPl = new byte[strLenPl];
                StringBuilder builder = new StringBuilder();


                for (int i = 0; i < strlength; i++)
                {
                    arr[i] = *++curByte;
                }

                messagePtr = resJson + 2 * sizeof(int);
                curByte = messagePtr - 1;

                for (int i = 0; i < strlength2; i++)
                {
                    arrJson[i] = *(++curByte);
                }

                messagePtr = res_PlPg + 2 * sizeof(int);
                curByte = messagePtr - 1;

                for (int i = 0; i < strLenPl; i++)
                {
                    arrPl[i] = *(++curByte);
                }

                var resJsonStr = Encoding.ASCII.GetString(arrJson);

                var resPlStr = Encoding.ASCII.GetString(arrPl);
                var resPb = ParseResult.Parser.ParseFrom(arr);

                var resPl = PgQuery.ParseResult.Parser.ParseJson(resPlStr);


                bool c = resPb.Stmts[0].Stmt.NodeCase == Node.NodeOneofCase.SelectStmt;

                NativeMemory.Free(result);
                Console.WriteLine("Hello, World!");
            }
        }

        [DllImport("PgQueryAnalyzerLib.dll", CallingConvention= CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        static extern unsafe byte* GetQueryParseTree(string query);

        [DllImport("PgQueryAnalyzerLib.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        static extern unsafe byte* GetQueryProtobufParseTree(string query);

        [DllImport("PgQueryAnalyzerLib.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        static extern unsafe byte* GetPlPgQueryJsonParseTree(string query);

        static string GetQueryString(string inputString)
        {
            string patternColon = "(?<!:):(?![:=])";

            string str = string.Empty;
            str = Regex.Replace(inputString, patternColon, "@"); 

            return str;
        }
    }
}
