using System;

namespace EasyCodeGeneratorApp.Providers
{
    public class SqlServerGen : ISqlGen
    {
        public string SqlGetTableNames()
        {
            return "SELECT Name as TableName FROM sys.tables order by Name";
        }
        public string SqlGetTableSchemas(string tableName)
        {
            var sql = String.Format(@"
SELECT	
OBJECT_NAME(sys.columns.object_id) as TableName,
sys.columns.name						AS ColumnName, 
		type_name(sys.columns.system_type_id)	AS DataTypeName,
		sys.columns.max_length					AS MaxLength,
		sys.columns.is_nullable					AS IsNullable,
		sys.columns.is_identity					AS IsIdentity,
		(case when exists(select 1  
						 from   syscolumns 
						 join   sysindexkeys on syscolumns.id  =sysindexkeys.id and syscolumns.colid=sysindexkeys.colid and syscolumns.id=sys.columns.object_id 
						 join   sysindexes   on syscolumns.id  =sysindexes.id   and sysindexkeys.indid=sysindexes.indid   
						 join   sysobjects   on sysindexes.name=sysobjects.name and sysobjects.xtype= 'PK '
						 where syscolumns.name = sys.columns.name) then 1 else 0 end) AS IsPrimaryKey,
		(select value from sys.extended_properties where sys.extended_properties.major_id = sys.columns.object_id and sys.extended_properties.minor_id = sys.columns.column_id) as Description,
		sys.columns.column_id as Position
FROM sys.columns  
-- WHERE sys.columns.object_id = object_id('{0}')
-- ORDER BY sys.columns.column_id
", tableName);

            return sql;
        }
        public string SqlGetTableKeys(string tableName)
        {
            return String.Format("sp_pkeys '{0}'", tableName);
        }
    }
}
