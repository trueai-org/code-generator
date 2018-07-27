using Dapper;
using EasyCodeGeneratorApp.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace EasyCodeGeneratorApp.Providers
{
    public class GenTables
    {
        public static List<DbTable> GetTables(string dbType, string conString)
        {
            IUnitOfWork db;
            ISqlGen gen;

            switch (dbType)
            {
                case "mysql":
                    gen = new MySqlGen();
                    db = new UnitOfWork(new MySql.Data.MySqlClient.MySqlConnection(), conString);
                    break;
                case "oracle":
                    throw new NotImplementedException();
                //gen = new OracleGen();
                //break;
                default:
                    gen = new SqlServerGen();
                    db = new UnitOfWork(new SqlConnection(), conString);
                    break;
            }

            var sql = gen.SqlGetTableNames();
            var tables = db.Connection.Query<DbTable>(sql).ToList();
            if (tables != null)
            {
                var tableSchemas = db.Connection.Query<TableSchema>(gen.SqlGetTableSchemas("")).ToList();
                foreach (var table in tables)
                {
                    table.TableSchemas = tableSchemas.Where(c => c.TableName == table.TableName).OrderBy(c => c.Position).ToList();
                }
            }
            return tables;
        }
    }
}
