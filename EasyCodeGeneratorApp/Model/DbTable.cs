using System.Collections.Generic;
using System.Linq;

namespace EasyCodeGeneratorApp
{
    /// <summary>
    /// Databse Table Schemas
    /// </summary>
    public class DbTable
    {
        /// <summary>
        /// Table Name
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// Table Description
        /// </summary>
        public string TableDescription { get; set; }
        /// <summary>
        /// Table Schemas
        /// </summary>
        public List<TableSchema> TableSchemas { get; set; }
        /// <summary>
        /// Table Keys
        /// </summary>
        public List<string> TableKeys
        {
            get
            {
                if (this.TableSchemas != null && this.TableSchemas.Count > 0 && this.TableSchemas.Exists(c => c.IsPrimaryKey))
                {
                    return this.TableSchemas.Where(c => c.IsPrimaryKey).OrderBy(c => c.Position).ThenBy(c => c.ColumnName).Select(c => c.ColumnName).ToList();
                }
                return new List<string>();
            }
        }
        /// <summary>
        /// Identity(first)
        /// </summary>
        public string Identity
        {
            get
            {
                if (this.TableSchemas != null && this.TableSchemas.Count > 0 && this.TableSchemas.Exists(c => c.IsIdentity))
                {
                    return this.TableSchemas.Where(c => c.IsIdentity).OrderBy(c => c.Position).ThenBy(c => c.ColumnName).First().ColumnName;
                }
                return string.Empty;
            }
        }
    }
}
