#region Licenses
/*MIT License
Copyright(c) 2018
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
#if !NET20 && !NET35 && !NET40
using System.Threading;
using System.Threading.Tasks;
#endif
using System.Reflection;
#endregion

namespace ADONetHelper
{
    /// <summary>
    /// Static utility class for assembly
    /// </summary>
    internal static class Utilites
    {
        /// <summary>
        /// Checks if the passed in type is a generic type that is nullable
        /// </summary>
        /// <param name="type">The .NET type to check for nullable</param>
        /// <returns>Returns true if the passed in type is nullable, false otherwise</returns>
        internal static bool IsNullableGenericType(Type type)
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
        /// Opens the passed in <see cref="DbConnection"/>
        /// </summary>
        /// <param name="connection">An instance of the <see cref="DbConnection"/> class</param>
        internal static void OpenDbConnection(DbConnection connection)
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
        /// <param name="connection">An instance of the <see cref="DbConnection"/> class</param>
        internal async static Task OpenDbConnectionAsync(DbConnection connection)
        {
            //Await the task
            await OpenDbConnectionAsync(connection, CancellationToken.None).ConfigureAwait(false);
        }
        /// <summary>
        /// Opens the passed in <see cref="DbConnection"/> if the <see cref="DbConnection.State"/> is <see cref="ConnectionState.Closed"/>
        /// </summary>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="connection">An instance of the <see cref="DbConnection"/> class</param>
        internal async static Task OpenDbConnectionAsync(DbConnection connection, CancellationToken token)
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
