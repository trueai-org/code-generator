namespace EasyCodeGeneratorApp
{
    /// <summary>
    /// Table Schema
    /// </summary>
    public class TableSchema
    {
        /// <summary>
        /// Table Name
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// Column Name
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// DataType
        /// </summary>
        public string DataTypeName { get; set; }
        /// <summary>
        /// MaxLength
        /// </summary>
        public short MaxLength { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Is Null
        /// </summary>
        public bool IsNullable { get; set; }
        /// <summary>
        /// Is Primary Key
        /// </summary>
        public bool IsPrimaryKey { get; set; }
        /// <summary>
        /// Is Identity
        /// </summary>
        public bool IsIdentity { get; set; }
        /// <summary>
        /// Column Position
        /// </summary>
        public int Position { get; set; }
        /// <summary>
        /// C# Data Type
        /// </summary>
        public string NetTypeName
        {
            get { return Util.GetTypeName(DataTypeName, IsNullable); }
        }
        /// <summary>
        /// Column And Type
        /// </summary>
        public string ColumnAndType
        {
            get
            {
                return this.ColumnName + "[" + this.DataTypeName + "]";
            }
        }
    }
}
