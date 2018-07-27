using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EasyCodeGeneratorApp
{
    public class Generator
    {
        public DbTable CurrentTable { get; set; }
        public string ModuleName { get; set; }
        public string BillName { get; set; }
        public string FileName { get; set; }

        const string TemplatePath = @".\Template\";
        const string sPathModel = TemplatePath + "Model.txt";
        const string sIRepository = TemplatePath + "IRepository.txt";
        const string sRepository = TemplatePath + "Repository.txt";
        const string sIService = TemplatePath + "IService.txt";
        const string sService = TemplatePath + "Service.txt";

        public Generator(DbTable table)
        {
            this.CurrentTable = table;
        }

        public string GenIRepository()
        {
            var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var TModel = ReadTxtFile(sIRepository);
            var RegField = new Regex(@"(^\s*#FieldsStart#\s*$)([\s\S]*)(^\s*#FieldsEnd#)", RegexOptions.Multiline);
            var Temp = RegField.Match(TModel).Groups[2].Value.TrimEnd();
            var sField = "";
            var sSpaces = "\r\n" + string.Empty.PadLeft(8, ' ') + "{0}";
            var areaName = GetAreaName(CurrentTable.TableName);
            TModel = TModel.Replace("{AreaName}", string.IsNullOrEmpty(areaName) ? "" : "." + areaName)
                .Replace("{TableName}", CurrentTable.TableName)
                .Replace("{DateNow}", "" + now);
            TModel = RegField.Replace(TModel, "        " + sField.TrimStart());
            return TModel.Replace("\r\n", "\n").Replace("\n", "\r\n");
        }

        public string GenRepository()
        {
            var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var TModel = ReadTxtFile(sRepository);
            var RegField = new Regex(@"(^\s*#FieldsStart#\s*$)([\s\S]*)(^\s*#FieldsEnd#)", RegexOptions.Multiline);
            var Temp = RegField.Match(TModel).Groups[2].Value.TrimEnd();
            var sField = "";
            var sSpaces = "\r\n" + string.Empty.PadLeft(8, ' ') + "{0}";
            var areaName = GetAreaName(CurrentTable.TableName);
            TModel = TModel.Replace("{AreaName}", string.IsNullOrEmpty(areaName) ? "" : "." + areaName)
                .Replace("{TableName}", CurrentTable.TableName)
                .Replace("{DateNow}", "" + now);
            TModel = RegField.Replace(TModel, "        " + sField.TrimStart());
            return TModel.Replace("\r\n", "\n").Replace("\n", "\r\n");
        }

        public string GenIService()
        {
            var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var TModel = ReadTxtFile(sIService);
            var RegField = new Regex(@"(^\s*#FieldsStart#\s*$)([\s\S]*)(^\s*#FieldsEnd#)", RegexOptions.Multiline);
            var Temp = RegField.Match(TModel).Groups[2].Value.TrimEnd();
            var sField = "";
            var sSpaces = "\r\n" + string.Empty.PadLeft(8, ' ') + "{0}";
            var areaName = GetAreaName(CurrentTable.TableName);
            TModel = TModel.Replace("{AreaName}", string.IsNullOrEmpty(areaName) ? "" : "." + areaName)
                .Replace("{TableName}", CurrentTable.TableName)
                .Replace("{DateNow}", "" + now);
            TModel = RegField.Replace(TModel, "        " + sField.TrimStart());
            return TModel.Replace("\r\n", "\n").Replace("\n", "\r\n");
        }

        public string GenService()
        {
            var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var TModel = ReadTxtFile(sService);
            var RegField = new Regex(@"(^\s*#FieldsStart#\s*$)([\s\S]*)(^\s*#FieldsEnd#)", RegexOptions.Multiline);
            var Temp = RegField.Match(TModel).Groups[2].Value.TrimEnd();
            var sField = "";
            var sSpaces = "\r\n" + string.Empty.PadLeft(8, ' ') + "{0}";
            var areaName = GetAreaName(CurrentTable.TableName);
            TModel = TModel.Replace("{AreaName}", string.IsNullOrEmpty(areaName) ? "" : "." + areaName)
                .Replace("{TableName}", CurrentTable.TableName)
                .Replace("{TableName2}", CurrentTable.TableName.Substring(0, 1).ToLower() + CurrentTable.TableName.Substring(1))
                .Replace("{DateNow}", "" + now);
            TModel = RegField.Replace(TModel, "        " + sField.TrimStart());
            return TModel.Replace("\r\n", "\n").Replace("\n", "\r\n");
        }

        public string GenModel()
        {
            var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var TModel = ReadTxtFile(sPathModel);
            var RegField = new Regex(@"(^\s*#FieldsStart#\s*$)([\s\S]*)(^\s*#FieldsEnd#)", RegexOptions.Multiline);
            var Temp = RegField.Match(TModel).Groups[2].Value.TrimEnd();
            var sField = "";
            var sSpaces = "\r\n" + string.Empty.PadLeft(8, ' ') + "{0}";
            foreach (var TS in CurrentTable.TableSchemas)
            {
                if (TS.ColumnName.Equals("Id", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                sField += Temp
                    .Replace("{FieldDesc}", GetDesc(TS.ColumnName, TS.Description))
                    .Replace("{FieldType}", TS.NetTypeName.Contains("SByte") ? TS.NetTypeName.Replace("S", "") : TS.NetTypeName)
                    .Replace("{FieldName}", TS.ColumnName);
            }

            var areaName = GetAreaName(CurrentTable.TableName);
            TModel = TModel.Replace("{AreaName}", string.IsNullOrEmpty(areaName) ? "" : "." + areaName)
                .Replace("{TableDesc}", CurrentTable.TableDescription)
                .Replace("{TableName}", CurrentTable.TableName)
                .Replace("{DateNow}", "" + now);
            TModel = RegField.Replace(TModel, "        " + sField.TrimStart());
            return TModel.Replace("\r\n", "\n").Replace("\n", "\r\n");
        }

        private string GetDesc(string columnName, string desc)
        {
            var str = string.Empty;

            string[] arr = new string[] {
                "Id", "Deleted", "CreatedBy", "CreatedDate", "UpdatedBy", "UpdatedDate",
                "Creator","Reviser"
            };

            if (arr.Contains(columnName)
                || string.IsNullOrEmpty(desc)
                || (columnName.EndsWith("Id", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(desc)))
            {
                str = string.Empty;
            }
            else
            {
                str = string.Format(@"        /// <summary>
        /// {0}
        /// </summary>" + "\r\n", string.Format(string.IsNullOrEmpty(desc) ? "" : desc.Replace("\r\n", "\r\n        /// ")));
            }

            var ctimeArr = new string[] { "CreateTime" };// "CREATED_DATE"
            var utimeArr = new string[] { "ReviseTime" };//, "UPDATED_DATE"
            if (ctimeArr.Contains(columnName))
            {
                str = "        [IgnoreOnInsert]\r\n";
            }
            else if (utimeArr.Contains(columnName))
            {
                str = "        [IgnoreOnInsert]\r\n";
                str += "        [IgnoreOnUpdate]\r\n";
            }
            return str;
        }

        private string GetAreaName(string tableName)
        {
            var areaName = "";
            if (tableName.StartsWith("Sys"))
            {
                areaName = "Sys";
            }
            else if (tableName.StartsWith("Circle"))
            {
                areaName = "Circle";
            }
            return areaName;
        }

        public string GenDAL()
        {
            //var TDAL = ReadTxtFile(sPathDAL);
            //TDAL = TDAL.Replace("{TableName}", CurrentTable.TableName);
            //return TDAL;
            return "";
        }

        public string GenBLL()
        {
            //var TBLL = ReadTxtFile(sPathBLL);
            //TBLL = TBLL.Replace("{TableName}", CurrentTable.TableName);
            ////TBLL = TBLL.Replace("IDOnePlusTable", CurrentTable.Identity);
            //return TBLL;
            return "";
        }

        public string GenListAspx()
        {
            //var ts = CurrentTable.TableSchemas;
            //var TWebList = ReadTxtFile(sPathWebList);
            //for (var i = 0; i < ts.Count; i++)
            //{
            //    TWebList = TWebList.Replace(String.Format("{{{0}:{1}}}", i, "ColumnName"), ts[i].ColumnName);
            //    TWebList = TWebList.Replace(String.Format("{{{0}:{1}}}", i, "Class"), "z-txt" + ts[i].TypeName == "DateTime" ? " easyui-datebox" : "");
            //}


            //TWebList = TWebList.Replace("{FileName}", String.Format("/{0}/{1}", ToStr(ModuleName), ToStr(FileName, CurrentTable.TableName)).Replace("//", ""));
            //return TWebList;
            return "";
        }

        public string GenListJs()
        {
            //var s = string.Empty;
            //const string template = "{ title: '{0}', field: '{0}', align: 'left', width: 70, formatter: function (value) { return $.formatDate(value, 'yyyy-MM-dd'); } },";
            //var ts = CurrentTable.TableSchemas;
            //var TWebList = ReadTxtFile(sPathWebList + ".js");
            //for (var i = 0; i < ts.Count; i++)
            //{
            //    TWebList = TWebList.Replace(String.Format("{{{0}:{1}}}", i, "ColumnName"), ts[i].ColumnName);

            //    var c = ts[i];
            //    s += s.Length == 0 ? "{" : "\t\t{";
            //    s += String.Format("{0}:'{1}',", "title", c.ColumnName);
            //    s += String.Format("{0}:'{1}',", "field", c.ColumnName);
            //    s += String.Format("{0}:'{1}',", "align", "left");
            //    s += String.Format("{0}:{1},", "width", 70);
            //    if (ts[i].NetTypeName.StartsWith("DateTime"))
            //        s += String.Format("{0}:{1},", "formatter", "function (value) { return $.formatDate(value, 'yyyy-MM-dd'); }");
            //    if (ts[i].NetTypeName.StartsWith("decimal"))
            //        s += String.Format("{0}:{1},", "formatter", "function (value) { return $.formatNumber(value, '#,##0.00'); }");
            //    s = s.Trim(',') + (i == ts.Count - 1 ? "}\r\n\t\t" : "},\r\n");
            //}
            //TWebList = TWebList.Replace("{cols}", s.Trim(','));
            //TWebList = TWebList.Replace("{TableName}", CurrentTable.TableName);
            //if (BillName != null) TWebList = TWebList.Replace("{BillName}", BillName);
            //TWebList = TWebList.Replace("{FileName}", String.Format("/{0}/{1}", ToStr(ModuleName), ToStr(FileName, CurrentTable.TableName)).Replace("//", ""));
            //return TWebList;
            return "";
        }

        public string GenEditAspx()
        {
            //var ts = CurrentTable.TableSchemas;
            //var TWebList = ReadTxtFile(sPathWebEdit);
            //for (var i = 0; i < ts.Count; i++)
            //{
            //    TWebList = TWebList.Replace(String.Format("{{{0}:{1}}}", i, "ColumnName"), ts[i].ColumnName);
            //    TWebList = TWebList.Replace(String.Format("{{{0}:{1}}}", i, "Class"), "z-txt" + ts[i].TypeName == "DateTime" ? " easyui-datebox" : "");
            //    TWebList = TWebList.Replace(String.Format("{{{0}:{1}}}", i, "DataCp"), ts[i].IsPrimaryKey ? "data-cp=\"equal\" readonly=\"readonly\" " : "");
            //}
            //TWebList = TWebList.Replace("{TableName}", CurrentTable.TableName);
            //TWebList = TWebList.Replace("{FileName}", String.Format("/{0}/{1}", ToStr(ModuleName), ToStr(FileName, CurrentTable.TableName)).Replace("//", ""));
            //return TWebList;
            return "";
        }

        public string GenEditJs()
        {
            //var s = string.Empty;
            //const string template = "{ title: '{0}', field: '{0}', align: 'left', width: 70, formatter: function (value) { return $.formatDate(value, 'yyyy-MM-dd'); } },";
            //var ts = CurrentTable.TableSchemas;
            //var TWebList = ReadTxtFile(sPathWebEdit + ".js");
            //for (var i = 0; i < ts.Count; i++)
            //{
            //    TWebList = TWebList.Replace(String.Format("{{{0}:{1}}}", i, "ColumnName"), ts[i].ColumnName);
            //}

            //ts = DetailTable.TableSchemas;
            //for (var i = 0; i < ts.Count; i++)
            //{
            //    var c = ts[i];
            //    s += s.Length == 0 ? "{" : "\t\t{";
            //    s += String.Format("{0}:'{1}',", "title", c.ColumnName);
            //    s += String.Format("{0}:'{1}',", "field", c.ColumnName);
            //    s += String.Format("{0}:'{1}',", "align", "left");
            //    s += String.Format("{0}:{1},", "width", 70);
            //    if (ts[i].NetTypeName.StartsWith("DateTime"))
            //        s += String.Format("{0}:{1},", "formatter", "function (value) { return $.formatDate(value, 'yyyy-MM-dd'); }");
            //    if (ts[i].NetTypeName.StartsWith("decimal"))
            //        s += String.Format("{0}:{1},", "formatter", "function (value) { return $.formatNumber(value, '#,##0.00'); }");
            //    s = s.Trim(',') + (i == ts.Count - 1 ? "}\r\n\t\t" : "},\r\n");
            //}

            //TWebList = TWebList.Replace("{DetailCols}", s.Trim(','));
            //TWebList = TWebList.Replace("{TableName}", CurrentTable.TableName);
            //TWebList = TWebList.Replace("{DetailTableName}", DetailTable.TableName);

            //return TWebList;
            return "";
        }

        public string GetTestCode()
        {
            var sb = new StringBuilder();
            var ts = CurrentTable.TableSchemas;
            for (var i = 0; i < ts.Count; i++)
            {
                sb.AppendFormat("c.{0},\r\n", ts[i].ColumnName);
            }
            return sb.ToString();
        }

        public string ReadTxtFile(string FilePath)
        {
            var content = string.Empty;
            using (var fs = new FileStream(FilePath, FileMode.Open))
            {
                using (var reader = new StreamReader(fs, Encoding.UTF8))
                {
                    string text = string.Empty;
                    while (!reader.EndOfStream)
                    {
                        text += reader.ReadLine() + "\r\n";
                        content = text;
                    }
                }
            }
            return content;
        }
    }
}
