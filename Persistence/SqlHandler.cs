using System;
using System.Data.Common;
using System.Linq;
using System.Collections.Generic;
using System.Data;

namespace Persistence
{
    static class SqlExtensions
    {
        internal static List<T> ToObjectList<T>(this DataTable dataTable)
        {
            return dataTable.Rows.Count == 0 ? null : Enumerable.Select(dataTable.AsEnumerable(), row => row.ToObject<T>()).ToList();
        }

        internal static List<T> ToObjectList<T>(this DataTable dataTable, ResultMap aResultMap)
        {
            return dataTable.Rows.Count == 0 ? null : Enumerable.Select(dataTable.AsEnumerable(), row => row.ToObject<T>(aResultMap)).ToList();
        }

        internal static T ToObject<T>(this DataRow dataRow)
        {
            var tObject = Activator.CreateInstance<T>();
            foreach (var prop in tObject.GetType().GetProperties().Where(prop => dataRow.Table.Columns.Contains(prop.Name) && dataRow[prop.Name] != null))
            {
                prop.SetValue(tObject, dataRow[prop.Name], null);
            }
            return tObject;
        }

        internal static T ToObject<T>(this DataRow dataRow, ResultMap aResultMap)
        {
            var tObject = Activator.CreateInstance<T>();
            for (var i = 0; i < aResultMap.Properties.Length; i++)
            {
                var property = aResultMap.Properties[i];
                var column = aResultMap.DatabaseColumns[i];
                if (!dataRow.Table.Columns.Contains(column))
                {
                    throw new Exception("Unkown column name '" + column + "' in result.");
                }
                if (dataRow[column] == null) continue;
                if (property.Contains('.'))
                {
                    SetNestedProperty(property, tObject, dataRow[column]);
                }
                else
                {
                    tObject.GetType().GetProperty(property).SetValue(tObject, dataRow[column], null);
                }
            }
            return tObject;
        }

        static void SetNestedProperty(string property, object _object, object value)
        {
            var bits = property.Split('.');
            for (var i = 0; i < bits.Length - 1; i++)
            {
                var propertyToGet = _object.GetType().GetProperty(bits[i]);
                var propertyValue = propertyToGet.GetValue(_object, null);
                if (propertyValue == null)
                {
                    var newObject = Activator.CreateInstance(propertyToGet.PropertyType);
                    propertyToGet.SetValue(_object, newObject, null);
                }
                _object = propertyToGet.GetValue(_object, null);
            }
            var propertyToSet = _object.GetType().GetProperty(bits.Last());
            propertyToSet.SetValue(_object, value, null);
        }
    }


    internal class ResultMap
    {
        internal string[] Properties { get; set; }
        internal string[] DatabaseColumns { get; set; }
    }

    public class SqlHandler
    {
        readonly DbConnection _connection;
        readonly DbDataAdapter _dbDataAdapter;
        public SqlHandler(string provider, string connectionString)
        {
            _connection = CreateDbConnection(provider, connectionString);
            _dbDataAdapter = CreateDataAdapter(provider);
        }


        // Given a provider name and connection string, 
        // create the DbProviderFactory and DbConnection.
        // Returns a DbConnection on success; null on failure.
        private static DbConnection CreateDbConnection(string providerName, string connectionString)
        {
            // Assume failure.
            DbConnection connection = null;

            // Create the DbProviderFactory and DbConnection.
            if (connectionString != null)
            {
                try
                {
                    var factory = DbProviderFactories.GetFactory(providerName);
                    connection = factory.CreateConnection();
                    connection.ConnectionString = connectionString;
                }
                catch (Exception ex)
                {
                    // Set the connection to null if it was created.
                    if (connection != null)
                    {
                        connection = null;
                    }
                    Console.WriteLine(ex.Message);
                }
            }
            // Return the connection.
            return connection;
        }

        void OpenConnection()
        {
            if (_connection == null) throw new Exception("Unable to open connection. Connection Object is null.");
            if (_connection.State == ConnectionState.Open) return;
            _connection.Open();
        }

        void CloseConnection()
        {
            if (_connection == null || _connection.State == ConnectionState.Closed) return;
            _connection.Close();
        }

        DbCommand CreateCommand(string query)
        {
            OpenConnection();
            var command = _connection.CreateCommand();
            command.CommandText = query;
            return command;
        }

        static DbDataAdapter CreateDataAdapter(string connection)
        {
            return DbProviderFactories.GetFactory(connection).CreateDataAdapter();
        }

        static ResultMap ValidateAndGetResultMap(string resutlMap, object _object)
        {
            if (string.IsNullOrEmpty(resutlMap) || resutlMap.Trim().Length == 0)
            {
                throw new Exception("Invalid result map.");
            }
            return PopulatePropertiesAndDataBaseColumns(resutlMap.Trim().Split(','), _object);
        }

        static ResultMap PopulatePropertiesAndDataBaseColumns(IEnumerable<string> propertiesAndDataBaseColumns, object _object)
        {
            IList<string> propertiesList = new List<string>();
            IList<string> columnList = new List<string>();
            foreach (var propertiesAndColumn in propertiesAndDataBaseColumns.Select(propertiesAndDataBaseColumn => propertiesAndDataBaseColumn.Split(':')))
            {
                if (propertiesAndColumn.Length == 1) throw new Exception("Invalid result map.");
                var property = propertiesAndColumn[0];
                var column = propertiesAndColumn[1];
                if (string.IsNullOrEmpty(property) || property.Trim().Length == 0 || string.IsNullOrEmpty(column) || column.Trim().Length == 0)
                {
                    throw new Exception("Invalid result map.");
                }
                property = property.Trim();
                column = column.Trim();
                if ((property.Contains('.') && !IsValidNestedProperty(property, _object)) || (!property.Contains('.') && _object.GetType().GetProperty(property) == null))
                {
                    throw new Exception("'" + _object.GetType().FullName + "' does not contain a property with the name '" + property + "'.");
                }
                propertiesList.Add(property);
                columnList.Add(column);
            }
            return new ResultMap { Properties = propertiesList.ToArray(), DatabaseColumns = columnList.ToArray() };
        }

        static bool IsValidNestedProperty(string property, object _object)
        {
            return (GetNestedProperty(property, _object, false) != null);
        }

        static object GetNestedProperty(string property, object _object, bool isGetValue)
        {
            var bits = property.Split('.');
            for (var i = 0; i < bits.Length - 1; i++)
            {
                var propertyToGet = _object.GetType().GetProperty(bits[i]);
                if (propertyToGet == null)
                {
                    throw new Exception("'" + _object.GetType().FullName + "' does not contain a property with the name '" + bits[i] + "'.");
                }
                var propertyValue = propertyToGet.GetValue(_object, null);
                if (propertyValue == null)
                {
                    var newObject = Activator.CreateInstance(propertyToGet.PropertyType);
                    propertyToGet.SetValue(_object, newObject, null);
                }
                _object = propertyToGet.GetValue(_object, null);
            }
            if (_object.GetType().GetProperty(bits.Last()) == null)
            {
                throw new Exception("'" + _object.GetType().FullName + "' does not contain a property with the name '" + bits.Last() + "'.");
            }
            return isGetValue ? _object.GetType().GetProperty(bits.Last()).GetValue(_object, null) : _object.GetType().GetProperty(bits.Last());
        }

        static string FormateQueryWithObjectValues(string query, object _object, bool isInsert)
        {
            if (_object == null) return query;
            IEnumerable<string> propertiesInQueryString;
            if (isInsert)
            {
                var propertyList = query.Substring(query.ToLower().IndexOf("values(", StringComparison.Ordinal) + 7, query.ToLower().LastIndexOf(")", StringComparison.Ordinal) - (query.ToLower().IndexOf("values(", StringComparison.Ordinal) + 7)).Split(',');
                propertiesInQueryString = propertyList.Where(property => property.StartsWith("#") && property.EndsWith("#"));
            }
            else
            {
                var filedAndPropertyList = query.Substring(query.ToLower().IndexOf(" set ", StringComparison.Ordinal) + 5, (query.ToLower().Contains(" where ") ? query.ToLower().IndexOf(" where ", StringComparison.Ordinal) : (query.ToLower().LastIndexOf("#", StringComparison.Ordinal) + 1)) - (query.ToLower().IndexOf(" set ", StringComparison.Ordinal) + 5)).Split(',');
                propertiesInQueryString = filedAndPropertyList.Select(fieldAndProperty => fieldAndProperty.Split('=')[1]).Where(property => property.StartsWith("#") && property.EndsWith("#"));
            }
            foreach (var property in propertiesInQueryString.Select(property => property.Trim()))
            {
                if (property.Contains('.'))
                {
                    var aPropertyValue = GetNestedProperty(property.Replace("#", ""), _object, true);
                    query = query.Replace(property, "'" + aPropertyValue + "'");
                }
                else
                {
                    query = query.Replace(property, "'" + _object.GetType().GetProperty(property.Replace("#", "")).GetValue(_object, null) + "'");
                }
            }
            return query;
        }

        public string AddConditionToQuery(string query, IEnumerable<string> conditions)
        {
            foreach (var condition in conditions)
            {
                if (!query.ToLower().Contains(" where "))
                {
                    query += " WHERE " + condition;
                }
                else
                {
                    query += " AND " + condition;
                }
            }
            return query;
        }

        int Write(string query, object _object, bool isInsert)
        {
            query = FormateQueryWithObjectValues(query, _object, isInsert);
            var result = CreateCommand(query).ExecuteNonQuery();
            CloseConnection();
            return result;
        }

        public int Insert(string query, object _object)
        {
            return Write(query, _object, true);
        }

        public int Update(string query, object _object)
        {
            return Write(query, _object, false);
        }

        DataTable Read(string query)
        {
            _dbDataAdapter.SelectCommand = CreateCommand(query);
            var dt = new DataTable();
            _dbDataAdapter.Fill(dt);
            CloseConnection();
            return dt;
        }

        DataTable Read(string query, int startRecordIndex, int maxRecords)
        {
            _dbDataAdapter.SelectCommand = CreateCommand(query);
            var dt = new DataTable();
            _dbDataAdapter.Fill(startRecordIndex, maxRecords, dt);
            CloseConnection();
            return dt;
        }

        /// <summary>
        /// Get a record from database.
        /// </summary>
        /// <typeparam name="T">Result object type.</typeparam>
        /// <param name="query">Query to select.</param>
        /// <returns></returns>
        public T GetRecord<T>(string query)
        {
            var dt = Read(query);
            return dt.Rows.Count > 0 ? dt.Rows[0].ToObject<T>() : default(T);
        }

        /// <summary>
        /// Get a record from database.
        /// </summary>
        /// <typeparam name="T">Result object type.</typeparam>
        /// <param name="query">Query to select.</param>
        /// <param name="resultMap">Result map for query result.</param>
        /// <returns></returns>
        public T GetRecord<T>(string query, string resultMap)
        {
            if (resultMap == null || resultMap.Trim().Length == 0) return GetRecord<T>(query);
            var dt = Read(query);
            return dt.Rows.Count > 0 ? dt.Rows[0].ToObject<T>(ValidateAndGetResultMap(resultMap, Activator.CreateInstance<T>())) : default(T);
        }

        /// <summary>
        /// Get records from database.
        /// </summary>
        /// <typeparam name="T">Result object type.</typeparam>
        /// <param name="query">Query to select.</param>
        /// <returns></returns>
        public List<T> GetRecords<T>(string query)
        {
            var dt = Read(query);
            return dt.Rows.Count > 0 ? dt.ToObjectList<T>() : null;
        }

        /// <summary>
        /// Get records from database.
        /// </summary>
        /// <typeparam name="T">Result object type.</typeparam>
        /// <param name="query">Query to select.</param>
        /// <param name="startRecordIndex">The zero-based record number to start with.</param>
        /// <param name="maxRecords">The maximum number of records to retrive.</param>
        /// <returns></returns>
        public List<T> GetRecords<T>(string query, int startRecordIndex, int maxRecords)
        {
            var dt = Read(query, startRecordIndex, maxRecords);
            return dt.Rows.Count > 0 ? dt.ToObjectList<T>() : null;
        }

        /// <summary>
        /// Get records from database.
        /// </summary>
        /// <typeparam name="T">Result object type.</typeparam>
        /// <param name="query">Query to select.</param>
        /// <param name="resultMap">Result map for query result.</param>
        /// <returns></returns>
        public List<T> GetRecords<T>(string query, string resultMap)
        {
            if (resultMap == null || resultMap.Trim().Length == 0) return GetRecords<T>(query);
            var dt = Read(query);
            return dt.Rows.Count > 0 ? dt.ToObjectList<T>(ValidateAndGetResultMap(resultMap, Activator.CreateInstance<T>())) : null;
        }

        /// <summary>
        /// Get records from database.
        /// </summary>
        /// <typeparam name="T">Result object type.</typeparam>
        /// <param name="query">Query to select.</param>
        /// <param name="startRecordIndex">The zero-based record number to start with.</param>
        /// <param name="maxRecords">The maximum number of records to retrive.</param>
        /// <param name="resultMap">Result map for query result.</param>
        /// <returns></returns>
        public List<T> GetRecords<T>(string query, int startRecordIndex, int maxRecords, string resultMap)
        {
            var dt = Read(query, startRecordIndex, maxRecords);
            return dt.Rows.Count > 0 ? dt.ToObjectList<T>(ValidateAndGetResultMap(resultMap, Activator.CreateInstance<T>())) : null;
        }

    }
}
