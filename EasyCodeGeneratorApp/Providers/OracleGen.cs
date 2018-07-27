using System;

namespace EasyCodeGeneratorApp.Providers
{
    public class OracleGen : ISqlGen
    {
        public string SqlGetTableNames()
        {
            return "SELECT table_name as TableName FROM user_tables order by table_name";
        }
        public string SqlGetTableSchemas(string TableName)
        {
            var sql = String.Format(@"
SELECT utc.column_name                       AS ColumnName
        ,utc.data_type                       AS DataTypeName
        ,utc.DATA_LENGTH                     AS MaxLength
        ,(case when utc.NULLABLE = 'Y' then 1 else 0 end)                        AS IsNullable
        ,(case when exists(select 1 
                    from user_constraints au, user_cons_columns ucc 
                    where ucc.table_name = au.table_name 
                    and ucc.constraint_name = au.constraint_name 
                    and au.constraint_type = 'P' 
                    and utc.TABLE_NAME = ucc.table_name 
                    AND    utc.COLUMN_NAME = ucc.column_name(+)) then 1 else 0 end) AS IsPrimaryKey
FROM   user_tab_columns utc
WHERE utc.table_name = '{0}'", TableName);

            return sql;
        }
        public string SqlGetTableKeys(string TableName)
        {

            var sql = String.Format(@"
select b.column_name
  from dba_constraints a, user_cons_columns b
 where a.table_name = '{0}'
   AND a.CONSTRAINT_TYPE = 'P'
   and a.constraint_name = b.constraint_name
   AND a.owner           = b.owner", TableName.ToUpper());//tableName要大写

            return sql;
        }
    }
}
