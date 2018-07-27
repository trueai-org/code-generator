namespace EasyCodeGeneratorApp
{
    public class Util
    {
        /// <summary>
        /// database type to c# type
        /// </summary>
        /// <param name="SqlTypeName"></param>
        /// <param name="IsNullable"></param>
        /// <returns></returns>
        public static string GetTypeName(string SqlTypeName, bool IsNullable)
        {
            string str = "";
            switch (SqlTypeName.ToLower())
            {
                case "char":
                case "nchar":
                case "ntext":
                case "text":
                case "nvarchar":
                case "varchar":
                case "xml": str = "string"; break;//String
                case "smallint": str = "short"; break;//Int16
                case "int": str = "int"; break;//Int32
                case "bigint": str = "long"; break;//Int64
                case "binary":
                case "image":
                case "varbinary":
                case "timestamp": str = "byte[]"; break;//Byte[]
                case "tinyint": str = "SByte"; break;//SByte
                case "bit": str = "bool"; break;//Boolean
                case "float": str = "double"; break;//Double
                case "real": str = "Guid"; break;//Single
                case "uniqueidentifier": str = "Guid"; break;//Guid
                case "sql_variant": str = "object"; break;//Object
                case "decimal":
                case "numeric":
                case "money":
                case "smallmoney": str = "decimal"; break;//Decimal
                case "datetime":
                case "smalldatetime": str = "DateTime"; break;//DateTime
                default: str = SqlTypeName; break;
            }
            if (IsNullable)
            {
                switch (str)
                {
                    case "short":
                    case "int":
                    case "long":
                    case "SByte":
                    case "bool":
                    case "double":
                    case "Guid":
                    case "decimal":
                    case "DateTime":
                        str += "?";
                        break;
                    default:
                        break;
                }
            }
            return str;
        }
    }
}
