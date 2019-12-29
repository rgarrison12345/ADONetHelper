#region Licenses
/*MIT License
Copyright(c) 2019
Robert Garrison

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.*/
#endregion
#region Using Statements
using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
#endregion

namespace ADONetHelper.Core
{
    /// <summary>
    /// Static utility class for assembly
    /// </summary>
    public static class Utilities
    {
        #region Async Methods
        /// <summary>
        /// Gets the query values coming out of the passed in <paramref name="reader"/> for each row retrieved
        /// </summary>
        /// <param name="token">Propagates notification that operations should be canceled</param>
        /// <param name="reader">An instance of <see cref="DbDataReader"/> that has the results from a SQL query</param>
        /// <returns>Returns a <see cref="List{T}"/> of <see cref="Dictionary{TKey, TValue}"/> from the results of a sql query</returns>
        public static async Task<List<IDictionary<string, object>>> GetDynamicResultsListAsync(DbDataReader reader, CancellationToken token)
        {
            List<IDictionary<string, object>> results = new List<IDictionary<string, object>>();

            //Keep reading records while there are records to read
            while (await reader.ReadAsync(token).ConfigureAwait(false) == true)
            {
                Dictionary<string, object> obj = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

                //Loop through all fields in this row
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    object value = null;

                    //Don't try and set a value if we know it's null
                    if (await reader.IsDBNullAsync(i, token).ConfigureAwait(false) == false)
                    {
                        value = await reader.GetFieldValueAsync<object>(i, token).ConfigureAwait(false);
                    }

                    //Add this into the dictionary
                    obj.Add(reader.GetName(i), value);
                }

                //Add this item to the Array
                results.Add(obj);
            }

            //Return this back to the caller
            return results;
        }
        /// <summary>
        /// Gets the query values coming out of the passed in <paramref name="reader"/> for each row retrieved
        /// </summary>
        /// <param name="token"></param>
        /// <param name="reader">An instance of <see cref="DbDataReader"/> that has the results from a SQL query</param>
        /// <returns>Returns a <see cref="List{T}"/> of <see cref="Dictionary{TKey, TValue}"/> from the results of a sql query</returns>
        public async static IAsyncEnumerable<IDictionary<string, object>> GetDynamicResultsEnumerableAsync(DbDataReader reader, [EnumeratorCancellation] CancellationToken token = default)
        {
            //Keep reading records while there are records to read
            while (await reader.ReadAsync(token) == true)
            {
                Dictionary<string, object> obj = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

                //Loop through all fields in this row
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    object value = null;

                    //Need to check for db null to ensure that db null doesn't make it into the dictionary
                    if (await reader.IsDBNullAsync(i, token).ConfigureAwait(false) == false)
                    {
                        value = await reader.GetFieldValueAsync<object>(i, token).ConfigureAwait(false);
                    }

                    //Add this into the dictionary
                    obj.Add(reader.GetName(i), value);
                }

                //Add this item to the Array
                yield return obj;
            }
        }
        /// <summary>
        /// Gets the query values coming out of the passed in <paramref name="reader"/> for each row retrieved
        /// </summary>
        /// <param name="token"></param>
        /// <param name="reader">An instance of <see cref="DbDataReader"/> that has the results from a SQL query</param>
        /// <returns>Returns a <see cref="List{T}"/> of <see cref="Dictionary{TKey, TValue}"/> from the results of a sql query</returns>
        public static async Task<IDictionary<string, object>> GetDynamicResultAsync(DbDataReader reader, CancellationToken token = default)
        {
            //Check if the reader has rows
            if (await reader.ReadAsync(token) == true)
            {
                Dictionary<string, object> obj = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

                //Loop through all fields in this row
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    object value = null;

                    //Need to check for db null to ensure that db null doesn't make it into the dictionary
                    if (await reader.IsDBNullAsync(i, token).ConfigureAwait(false) == false)
                    {
                        value = await reader.GetFieldValueAsync<object>(i, token).ConfigureAwait(false);
                    }

                    //Add this into the dictionary
                    obj.Add(reader.GetName(i), value);
                }

                //Add this item to the Array
                return obj;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Opens the passed in <see cref="DbConnection"/> if the <see cref="DbConnection.State"/> is <see cref="ConnectionState.Closed"/>
        /// </summary>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="connection">An instance of the <see cref="DbConnection"/> class</param>
        public async static Task OpenDbConnectionAsync(DbConnection connection, CancellationToken token = default)
        {
            //Check if we need to open the passed in connection
            if (connection.State == ConnectionState.Closed)
            {
                //Await the task
                await connection.OpenAsync(token).ConfigureAwait(false);
            }
        }
        #endregion
        #region Sync Methods
        /// <summary>
        /// Gets the query values coming out of the passed in <paramref name="reader"/> for each row retrieved
        /// </summary>
        /// <param name="reader">An instance of <see cref="DbDataReader"/> that has the results from a SQL query</param>
        /// <returns>Returns a <see cref="IEnumerable{T}"/> of <see cref="Dictionary{TKey, TValue}"/> from the results of a sql query</returns>
        public static IEnumerable<IDictionary<string, object>> GetDynamicResultsEnumerable(DbDataReader reader)
        {
            //Keep reading records while there are records to read
            while (reader.Read() == true)
            {
                Dictionary<string, object> obj = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

                //Loop through all fields in this row
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    object value = null;

                    //Need to check for db null to ensure that db null doesn't make it into the dictionary
                    if (reader.IsDBNull(i) == false)
                    {
                        value = reader.GetValue(i);
                    }

                    //Add this into the dictionary
                    obj.Add(reader.GetName(i), value);
                }

                //Add this item to the Array
                yield return obj;
            }
        }
        /// <summary>
        /// Gets the query values coming out of the passed in <paramref name="reader"/> for each row retrieved
        /// </summary>
        /// <param name="reader">An instance of <see cref="DbDataReader"/> that has the results from a SQL query</param>
        /// <returns>Returns a <see cref="List{T}"/> of <see cref="Dictionary{TKey, TValue}"/> from the results of a sql query</returns>
        public static List<IDictionary<string, object>> GetDynamicResultsList(DbDataReader reader)
        {
            List<IDictionary<string, object>> returnList = new List<IDictionary<string, object>>();

            //Keep reading records while there are records to read
            while (reader.Read() == true)
            {
                Dictionary<string, object> obj = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

                //Loop through all fields in this row
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    object value = null;

                    //Need to check for db null to ensure that db null doesn't make it into the dictionary
                    if (reader.IsDBNull(i) == false)
                    {
                        value = reader.GetValue(i);
                    }

                    //Add this into the dictionary
                    obj.Add(reader.GetName(i), value);

                    returnList.Add(obj);
                }
            }

            //Return this back to the caller
            return returnList;
        }
        /// <summary>
        /// Gets the query values coming out of the passed in <paramref name="reader"/> for each row retrieved
        /// </summary>
        /// <param name="reader">An instance of <see cref="DbDataReader"/> that has the results from a SQL query</param>
        /// <returns>Returns a <see cref="List{T}"/> of <see cref="Dictionary{TKey, TValue}"/> from the results of a sql query</returns>
        public static IDictionary<string, object> GetDynamicResult(DbDataReader reader)
        {
            //Check if the reader has rows
            if (reader.Read() == true)
            {
                Dictionary<string, object> obj = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

                //Loop through all fields in this row
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    object value = null;

                    //Need to check for db null to ensure that db null doesn't make it into the dictionary
                    if (reader.IsDBNull(i) == false)
                    {
                        value = reader.GetFieldValue<object>(i);
                    }

                    //Add this into the dictionary
                    obj.Add(reader.GetName(i), value);
                }

                //Add this item to the Array
                return obj;
            }

            return null;
        }
        /// <summary>
        /// Gets the dyamic type list.
        /// </summary>
        /// <param name="results">The results.</param>
        /// <returns></returns>
        public static List<T> GetDynamicTypeList<T>(List<IDictionary<string, object>> results)
        {
            List<T> returnList = new List<T>();

            //Keep going through the results
            foreach (IDictionary<string, object> result in results)
            {
                returnList.Add(GetSingleDynamicType<T>(result));
            }

            //Return this back to the caller
            return returnList;
        }
        /// <summary>
        /// Gets the type of the single dynamic.
        /// </summary>
        /// <typeparam name="T">A type that will be generated from the results of a sql query</typeparam>
        /// <param name="results">The results.</param>
        /// <returns></returns>
        public static T GetSingleDynamicType<T>(IDictionary<string, object> results)
        {
            //Check for null first
            if (results == null)
            {
                //Return the default for the type
                return default;
            }

            //Get an instance of the object passed in
            object returnType = Activator.CreateInstance(typeof(T));
            Type type = returnType.GetType();

            //Loop through all properties
            foreach (PropertyInfo p in type.GetProperties())
            {
                DbField field = null;
                bool ignoredField = false;
                bool shouldSkip = !(p.CanWrite == true);

                //Check if we can write the property
                if (shouldSkip == true)
                {
                    //Don't go any further we cannot write to this property
                    continue;
                }

                //Get the dbfield attribute
                foreach (object attribute in p.GetCustomAttributes(false))
                {
                    //Check if this is an ignored field
                    if (attribute is DbFieldIgnore)
                    {
                        //Break out we're done
                        ignoredField = true;
                        break;
                    }
                    //Check if this is a dbfield
                    else if (attribute is DbField)
                    {
                        field = (DbField)attribute;
                    }
                }

                //Check if we should go on to the next loop
                if (ignoredField == true)
                {
                    continue;
                }

                string fieldName = p.Name;
                object value = null;

                //Check if the cast took place
                if (field != null && !string.IsNullOrEmpty(field.DatabaseFieldName))
                {
                    fieldName = field.DatabaseFieldName;
                }

                //Check if the field is present in the dynamic object
                if (results.ContainsKey(fieldName) == true)
                {
                    //Get the current property value
                    value = results[fieldName];

                    //Check if DBNull
                    if (field != null && (value == null || value == DBNull.Value))
                    {
                        //Set new value
                        value = field.DefaultValueIfNull;
                    }

                    //Check if this is a nullable type
                    if (IsNullableGenericType(p.PropertyType))
                    {
                        if (value == null)
                        {
                            p.SetValue(returnType, null, null);
                        }
                        else
                        {
                            p.SetValue(returnType, Convert.ChangeType(value, Nullable.GetUnderlyingType(p.PropertyType)), null);
                        }
                    }
                    //Check if an enum
                    else if (p.PropertyType.GetTypeInfo().IsEnum)
                    {
                        if (value != null)
                        {
                            p.SetValue(returnType, Enum.Parse(p.PropertyType, value.ToString()), null);
                        }
                    }
                    else
                    {
                        //This is a normal property
                        p.SetValue(returnType, Convert.ChangeType(value, p.PropertyType), null);
                    }
                }
            }

            //Return this back to the caller
            return (T)returnType;
        }
        /// <summary>
        /// Opens the passed in <see cref="DbConnection"/> if the <see cref="DbConnection.State"/> is <see cref="ConnectionState.Closed"/>
        /// </summary>
        /// <param name="connection">An instance of the <see cref="DbConnection"/> class</param>
        public static void OpenDbConnection(DbConnection connection)
        {
            //Check if we need to open the passed in connection
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }
        #endregion
        #region Helper Methods
        /// <summary>
        /// Checks if the passed in type is a generic type that is nullable
        /// </summary>
        /// <param name="type">The .NET type to check for nullable</param>
        /// <returns>Returns true if the passed in type is nullable, false otherwise</returns>
        public static bool IsNullableGenericType(Type type)
        {
            TypeInfo info = type.GetTypeInfo();

            //Return this back to the caller
            return (info.IsGenericType && info.GetGenericTypeDefinition() == typeof(Nullable<>));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsStruct(this Type source)
        {
            return source.IsValueType && !source.IsPrimitive && !source.IsEnum;
        }
        #endregion
    }
}