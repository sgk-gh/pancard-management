using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace Persistence
{
    static class SqlExtensions
    {
        internal static List<T> ToObjectList<T>(this DataTable dataTable)
        {
            return dataTable.Rows.Count == 0 ? null : Enumerable.Select(dataTable.AsEnumerable(), row => row.ToObject<T>()).ToList();
        }

        //internal static List<T> ToObjectList<T>(this DataTable dataTable, IEnumerable<string> resultMap)
        //{
        //    if (dataTable.Rows.Count == 0) return null;
        //    List<T> objectList = new List<T>();
        //    foreach (var row in dataTable.AsEnumerable())
        //    {
        //        var tObject = row.ToObject<T>(resultMap);
        //        objectList.Add(tObject);
        //    }
        //    return objectList;
        //}

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

        //internal static T ToObject<T>(this DataRow dataRow, IEnumerable<string> resultMap)
        //{
        //    var tObject = Activator.CreateInstance<T>();
        //    foreach (var pair in resultMap)
        //    {
        //        var property = pair.Split(':')[0];
        //        var column = pair.Split(':')[1];
        //        if (dataRow.Table.Columns.Contains(column) && dataRow[column] != null)
        //        {
        //            tObject.GetType().GetProperty(property).SetValue(tObject, dataRow[column], null);
        //        }
        //    }            
        //    return tObject;
        //}

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
                if(propertyValue == null)
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
        //internal string ClassName { get; set; }
        internal string[] Properties { get; set; }
        internal string[] DatabaseColumns { get; set; }
        //internal bool IsValid { get; set; }
        //internal string ErrorMessage { get; set; }
    }

    public class SqlHandler
    {
        readonly OleDbConnection _connection;

        public SqlHandler(string connectionString)
        {
            _connection = new OleDbConnection(connectionString);
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

        OleDbCommand CreateCommand(string query)
        {
            OpenConnection();            
            return new OleDbCommand(query, _connection);
        }

        static ResultMap ValidateAndGetResultMap(string resutlMap, object _object)
        {            
            if(string.IsNullOrEmpty(resutlMap) || resutlMap.Trim().Length == 0)
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

        static string FormateQueryWithObjectValues(string query, object _object)
        {
            if (_object == null) return query;
            var propertiesInQueryString = query.Substring(query.ToLower().IndexOf("values(#", StringComparison.Ordinal) + 7, query.ToLower().IndexOf("#)", StringComparison.Ordinal) + 1 - (query.ToLower().IndexOf("values(#", StringComparison.Ordinal) + 7)).Split(',');
            foreach(var property in propertiesInQueryString)
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

        internal int Insert(string query,object _object)
        {
            query = FormateQueryWithObjectValues(query, _object);
            var result = CreateCommand(query).ExecuteNonQuery();
            CloseConnection();
            return result;
        }

        internal int Update(string query, object _object)
        {
            query = FormateQueryWithObjectValues(query, _object);
            var result = CreateCommand(query).ExecuteNonQuery();
            CloseConnection();
            return result;
        }

        /// <summary>
        /// Get a record from database.
        /// </summary>
        /// <typeparam name="T">Result object type.</typeparam>
        /// <param name="query">Query to select.</param>
        /// <param name="_object">Object value for query.</param>
        /// <returns></returns>
        internal T GetRecord<T>(string query, object _object)
        {
            var olda = new OleDbDataAdapter(CreateCommand(query));
            var dt = new DataTable();
            olda.Fill(dt);
            CloseConnection();
            return dt.Rows.Count > 0 ? dt.Rows[0].ToObject<T>() : default(T);
        }

        /// <summary>
        /// Get a record from database.
        /// </summary>
        /// <typeparam name="T">Result object type.</typeparam>
        /// <param name="query">Query to select.</param>
        /// <param name="resultMap">Result map for query result.</param>
        /// <param name="_object">Object value for query.</param>
        /// <returns></returns>
        internal T GetRecord<T>(string query, string resultMap, object _object)
        {
            if (resultMap == null || !resultMap.Any()) return GetRecord<T>(query, _object);
            var olda = new OleDbDataAdapter(CreateCommand(query));
            var dt = new DataTable();
            olda.Fill(dt);
            CloseConnection();
            return dt.Rows.Count > 0 ? dt.Rows[0].ToObject<T>(ValidateAndGetResultMap(resultMap, Activator.CreateInstance<T>())) : default(T);
        }

        /// <summary>
        /// Get records from database.
        /// </summary>
        /// <typeparam name="T">Result object type.</typeparam>
        /// <param name="query">Query to select.</param>
        /// <param name="_object">Object value for query.</param>
        /// <returns></returns>
        internal List<T> GetRecords<T>(string query, object _object)
        {
            var olda = new OleDbDataAdapter(CreateCommand(query));
            var dt = new DataTable();
            olda.Fill(dt);
            CloseConnection();
            return dt.Rows.Count > 0 ? dt.ToObjectList<T>() : null;
        }

        /// <summary>
        /// Get records from database.
        /// </summary>
        /// <typeparam name="T">Result object type.</typeparam>
        /// <param name="query">Query to select.</param>
        /// <param name="startRecordIndex">The zero-based record number to start with.</param>
        /// <param name="maxRecords">The maximum number of records to retrive.</param>
        /// <param name="_object">Object value for query.</param>
        /// <returns></returns>
        internal List<T> GetRecords<T>(string query, int startRecordIndex, int maxRecords, object _object)
        {
            var olda = new OleDbDataAdapter(CreateCommand(query));
            var dt = new DataTable();
            olda.Fill(startRecordIndex, maxRecords, dt);
            CloseConnection();
            return dt.Rows.Count > 0 ? dt.ToObjectList<T>() : null;
        }

        /// <summary>
        /// Get records from database.
        /// </summary>
        /// <typeparam name="T">Result object type.</typeparam>
        /// <param name="query">Query to select.</param>
        /// <param name="resultMap">Result map for query result.</param>
        /// <param name="_object">Object value for query.</param>
        /// <returns></returns>
        internal List<T> GetRecords<T>(string query, string resultMap, object _object)
        {
            if (resultMap == null || !resultMap.Any()) return GetRecords<T>(query, _object);
            var olda = new OleDbDataAdapter(CreateCommand(query));
            var dt = new DataTable();
            olda.Fill(dt);
            CloseConnection();
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
        /// <param name="_object">Object value for query.</param>
        /// <returns></returns>
        internal List<T> GetRecords<T>(string query, int startRecordIndex, int maxRecords, string resultMap, object _object)
        {
            var olda = new OleDbDataAdapter(CreateCommand(query));
            var dt = new DataTable();
            olda.Fill(startRecordIndex, maxRecords, dt);
            CloseConnection();
            return dt.Rows.Count > 0 ? dt.ToObjectList<T>(ValidateAndGetResultMap(resultMap, Activator.CreateInstance<T>())) : null;
        }

    }
}
