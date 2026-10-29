using System.Text.RegularExpressions;
using System.Data;
using System;

namespace EmailSenderUtilityWebConsole.Infrastructure
{
    public static class StaticMethod
    {
        public static DataTable RemoveEscapeSequences(DataTable dataTable)
        {
            if (dataTable != null && dataTable.Columns != null && dataTable.Columns.Count > 0)
            {
                foreach (DataColumn column in dataTable.Columns)
                {
                    column.ColumnName = Regex.Unescape(column.ColumnName);
                }
            }
            return dataTable;
        }

        public static string GetValue(DataRow dataRow, string columnName)
        {
            string value = string.Empty;
            if (dataRow != null && dataRow.Table != null && dataRow.Table.Columns != null)
            {
                if (dataRow.Table.Columns.Contains(columnName) && dataRow[columnName] != DBNull.Value)
                    value = Convert.ToString(dataRow[columnName]).Trim();
            }
            return value;
        }

        public static string GenerateGUID(int length)
        {
            string guid = Guid.NewGuid().ToString("N");
            string safeGuid = guid.Substring(0, Math.Min(length, guid.Length));
            return safeGuid;
        }
    }
}
