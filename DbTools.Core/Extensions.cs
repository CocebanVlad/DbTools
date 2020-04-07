using System.Data;
using System.Data.SqlClient;

namespace DbTools.Core
{
    public static class Extensions
    {
        /// <summary>
        /// Fill table with data returned from command execution
        /// </summary>
        /// <param name="table">Table to fill</param>
        /// <param name="cmd">SQL command</param>
        /// <returns>Data table</returns>
        public static void Fill(this DataTable table, SqlCommand cmd)
        {
            using (var adapter = new SqlDataAdapter(cmd))
                adapter.Fill(table);
        }

        /// <summary>
        /// Get value by column name
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="row">Data row</param>
        /// <param name="columnName">Column name</param>
        /// <returns>Value</returns>
        public static T GetValue<T>(this DataRow row, string columnName)
        {
            return (T)row[columnName];
        }

        /// <summary>
        /// Get nullable value by column name
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="row">Data row</param>
        /// <param name="columnName">Column name</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>Value</returns>
        public static T GetNullableValue<T>(this DataRow row, string columnName, T defaultValue)
        {
            return row.IsNull(columnName) ? defaultValue : (T)row[columnName];
        }

        /// <summary>
        /// Get string value by column name
        /// </summary>
        /// <param name="row">Data row</param>
        /// <param name="columnName">Column name</param>
        /// <returns>String</returns>
        public static string GetStringValue(this DataRow row, string columnName)
        {
            return row.GetNullableValue<string>(columnName, null);
        }

        /// <summary>
        /// Get int value by column name
        /// </summary>
        /// <param name="row">Data row</param>
        /// <param name="columnName">Column name</param>
        /// <returns>Int</returns>
        public static int GetIntValue(this DataRow row, string columnName)
        {
            return row.GetValue<int>(columnName);
        }

        /// <summary>
        /// Get bool value by column name
        /// </summary>
        /// <param name="row">Data row</param>
        /// <param name="columnName">Column name</param>
        /// <returns>Bool</returns>
        public static bool GetBoolValue(this DataRow row, string columnName)
        {
            return row.GetValue<bool>(columnName);
        }
    }
}
