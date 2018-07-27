namespace EasyCodeGeneratorApp.Providers
{
    public interface ISqlGen
    {
        /// <summary>
        /// Table Names
        /// </summary>
        /// <returns></returns>
        string SqlGetTableNames();
        /// <summary>
        /// Table Schemas
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        string SqlGetTableSchemas(string tableName);
        /// <summary>
        /// Table Keys
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        string SqlGetTableKeys(string tableName);
    }
}
