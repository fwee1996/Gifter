using System;
using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace Gifter.Utils
{
    /// <summary>
    ///  A set of useful function for interacting with ADO.NET
    /// </summary>
    public static class DbUtils
    {
        /// <summary>
        ///  Get a string from a data reader object and gracefully handle NULL values
        /// </summary>
        /// <param name="reader">A SqlDataReader that has not exhausted it's result set.</param>
        /// <param name="column">The name of the column from the result set refereed to by the reader.</param>
        /// <returns>The value of the given column or null.</returns>
        public static string GetString(SqlDataReader reader, string column)
        {
            var ordinal = reader.GetOrdinal(column);
            if (reader.IsDBNull(ordinal))
            {
                return null;
            }

            return reader.GetString(ordinal);
        }

        /// <summary>
        ///  Get an int from a data reader object.
        ///  This method assumes the value is not NULL.
        /// </summary>
        /// <param name="reader">A SqlDataReader that has not exhausted it's result set.</param>
        /// <param name="column">The name of the column from the result set refereed to by the reader.</param>
        /// <returns>The value of the given column.</returns>
        /// 
        //above are  XML documentation comments,generate documentation
        //<param name="reader">: Describes the reader parameter, which is an instance of SqlDataReader that is used to read data from a SQL result set.
        //<param name = "column" >: Describes the column parameter, which is the name of the column from which the integer value will be extracted.
        //<returns>: Describes the return value of the method, which is the integer value from the specified column.
        //"static"-can call this method without creating an instance of the class.
        public static int GetInt(SqlDataReader reader, string column)//string column: The name of the column in the result set from which to retrieve the integer value.
        {
            return reader.GetInt32(reader.GetOrdinal(column));//GetOrdinal(column): Finds the column index based on its name.
            //GetInt32(index): Retrieves the integer value from that column index.
        }

        /// <summary>
        ///  Get a DateTime from a data reader object.
        ///  This method assumes the value is not NULL.
        /// </summary>
        /// <param name="reader">A SqlDataReader that has not exhausted it's result set.</param>
        /// <param name="column">The name of the column from the result set refereed to by the reader.</param>
        /// <returns>The value of the given column.</returns>
        public static DateTime GetDateTime(SqlDataReader reader, string column)
        {
            return reader.GetDateTime(reader.GetOrdinal(column)); 
        }

        /// <summary>
        ///  Get an int? (nullable int) from a data reader object and gracefully handle NULL values
        /// </summary>
        /// <param name="reader">A SqlDataReader that has not exhausted it's result set.</param>
        /// <param name="column">The name of the column from the result set refereed to by the reader.</param>
        /// <returns>The value of the given column or null.</returns>
        public static int? GetNullableInt(SqlDataReader reader, string column)
        {
            var ordinal = reader.GetOrdinal(column);
            if (reader.IsDBNull(ordinal))
            {
                return null;
            }

            return reader.GetInt32(ordinal);
        }

        /// <summary>
        ///  Get a DateTime? (nullable DateTime) from a data reader object and gracefully handle NULL values
        /// </summary>
        /// <param name="reader">A SqlDataReader that has not exhausted it's result set.</param>
        /// <param name="column">The name of the column from the result set refereed to by the reader.</param>
        /// <returns>The value of the given column or null.</returns>
        public static DateTime? GetNullableDateTime(SqlDataReader reader, string column)
        {
            var ordinal = reader.GetOrdinal(column);
            if (reader.IsDBNull(ordinal))
            {
                return null;
            }

            return reader.GetDateTime(ordinal);
        }

        /// <summary>
        ///  Determine if the value a given column is NULL
        /// </summary>
        /// <param name="reader">A SqlDataReader that has not exhausted it's result set.</param>
        /// <param name="column">The name of the column from the result set refereed to by the reader.</param>
        /// <returns>true if "column" is NULL in the database otherwise false.</returns>
        public static bool IsDbNull(SqlDataReader reader, string column)
        {
            return reader.IsDBNull(reader.GetOrdinal(column));
        }

        /// <summary>
        ///  Determine if the value a given column is not NULL
        /// </summary>
        /// <param name="reader">A SqlDataReader that has not exhausted it's result set.</param>
        /// <param name="column">The name of the column from the result set refereed to by the reader.</param>
        /// <returns>true if "column" is not NULL in the database otherwise false.</returns>
        public static bool IsNotDbNull(SqlDataReader reader, string column)
        {
            return !IsDbNull(reader, column);
        }

        /// <summary>
        ///  Add a parameter to the given SqlCommand object and gracefully handle null values.
        /// </summary>
        /// <param name="cmd">The command to which to add the parameter.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter. May be null.</param>
        public static void AddParameter(SqlCommand cmd, string name, object value)
        {
            if (value == null)
            {
                cmd.Parameters.AddWithValue(name, DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue(name, value);
            }
        }
    }
}