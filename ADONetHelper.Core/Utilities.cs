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
#if !NET20 && !NET35 && !NET40
using System.Threading;
using System.Threading.Tasks;
#endif
using System.Reflection;
#endregion

namespace ADONetHelper.Core
{
    /// <summary>
    /// Static utility class for assembly
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Gets the type of the single dynamic.
        /// </summary>
        /// <typeparam name="T">A type that will be generated from the results of a sql query</typeparam>
        /// <param name="results">The results.</param>
        /// <returns></returns>
        public static T GetSingleDynamicType<T>(IDictionary<string, object> results)
        {
            //Get an instance of the object passed in
            object returnType = Activator.CreateInstance(typeof(T));
            Type type = returnType.GetType();

            //Loop through all properties
            foreach (PropertyInfo p in type.GetProperties())
            {
                DbField field = null;
                bool ignoredField = false;

                //Check if we can write the property
                if (p.CanWrite == false)
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
#if !NET20 && !NET35 && !NET40
                    else if (p.PropertyType.GetTypeInfo().IsEnum)
                    {
                        if (value != null)
                        {
                            p.SetValue(returnType, Enum.Parse(p.PropertyType, value.ToString()), null);
                        }
                    }
#else
                    //Check if this is an enum
                    else if (p.PropertyType.IsEnum)
                    {
                        if(value != null)
                        {
                            p.SetValue(returnType, Enum.Parse(p.PropertyType, results[p.Name].ToString()), null);
                        }
                    }
#endif
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
        /// Checks if the passed in type is a generic type that is nullable
        /// </summary>
        /// <param name="type">The .NET type to check for nullable</param>
        /// <returns>Returns true if the passed in type is nullable, false otherwise</returns>
        public static bool IsNullableGenericType(Type type)
        {
#if NET20 || NET35 || NET40
            //Return this back to the caller
            return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
#else
            TypeInfo info = type.GetTypeInfo();

            //Return this back to the caller
            return (info.IsGenericType && info.GetGenericTypeDefinition() == typeof(Nullable<>));
#endif
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
#if !NET20 && !NET35 && !NET40
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
#endif
    }
}