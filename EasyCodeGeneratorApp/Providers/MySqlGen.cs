using System;

namespace EasyCodeGeneratorApp.Providers
{
    public class MySqlGen : ISqlGen
    {
        public string SqlGetTableNames()
        {
            return @"
SELECT 
    table_name AS TableName,TABLE_COMMENT AS TableDescription
FROM
    information_schema.tables
WHERE
    table_schema = (select database())
        AND table_type = 'BASE TABLE'
ORDER BY table_name
";
        }
        public string SqlGetTableSchemas(string tableName = "")
        {
            var sql = String.Format(@"
SELECT
	TABLE_NAME AS TableName,
	COLUMN_NAME AS ColumnName,
CASE
	DATA_TYPE 
	WHEN 'tinyint' THEN
	( CASE COLUMN_TYPE WHEN 'tinyint(1)' THEN 'bool' ELSE 'int' END ) ELSE DATA_TYPE 
	END AS DataTypeName,
	COLUMN_COMMENT AS Description,
CASE
	IS_NULLABLE 
	WHEN 'YES' THEN
TRUE ELSE FALSE 
	END AS IsNullable,
CASE
	COLUMN_KEY 
	WHEN 'PRI' THEN
TRUE ELSE FALSE 
	END AS IsPrimaryKey,
	ORDINAL_POSITION AS Position 
FROM
	information_schema.COLUMNS 
WHERE
	table_schema = ( SELECT DATABASE ( ) )
/*
        AND table_name = '{0}'
ORDER BY
	ORDINAL_POSITION */
", tableName);

            return sql;
        }
        public string SqlGetTableKeys(string tableName)
        {
            return String.Format("sp_pkeys '{0}'", tableName);
        }
    }
}
