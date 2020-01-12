#region Licenses
/*MIT License
Copyright(c) 2020
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
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
#endregion

namespace ADONetHelper
{
    /// <summary>
    /// Contract class that defines asynchronous operations to be performed against a data store
    /// </summary>
    public interface IAsynchronousClient
    {
        #region Data Retrieval
        /// <summary>
        /// Gets an instance of <see cref="DataTable"/> asynchronously
        /// </summary>
        /// <param name="query">SQL query to use to build a <see cref="DataTable"/></param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>Returns a <see cref="Task{TResult}"/> of datatable</returns>
        Task<DataTable> GetDataTableAsync(string query, CancellationToken token = default);
        /// <summary>
        /// Gets a single instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>Gets an instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine.
        /// Or the default value of <typeparamref name="T"/> if there are no search results
        /// </returns>
        Task<T> GetDataObjectAsync<T>(string query, CancellationToken token = default) where T : class;
        /// <summary>
        /// Gets a list of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>Returns a list of type parameter object based on the fields in the passed in query</returns>
        IAsyncEnumerable<T> GetDataObjectEnumerableAsync<T>(string query, CancellationToken token = default) where T : class;
        /// <summary>
        /// Gets a list of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>Returns a list of type parameter object based on the fields in the passed in query</returns>
        Task<List<T>> GetDataObjectListAsync<T>(string query, CancellationToken token = default) where T : class;
        /// <summary>
        /// Utility method for returning a <see cref="Task{DbDataReader}"/> object created from the passed in query
        /// </summary>
        /// <param name="transact">An instance of <see cref="DbTransaction"/></param>
        /// <param name="behavior">Provides a description of the results of the query and its effect on the database.  Defaults to <see cref="CommandBehavior.Default"/></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>A <see cref="Task{DbDataReader}"/> object, the caller is responsible for handling closing the <see cref="DbDataReader"/>.  Once the data reader is closed, the database connection will be closed as well</returns>
        Task<DbDataReader> GetDbDataReaderAsync(string query, CancellationToken token = default, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null);
        /// <summary>
        /// Utility method for returning a <see cref="Task{Object}"/> value from the database
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>Returns the value of the first column in the first row as <see cref="Task"/></returns>
        Task<object> GetScalarValueAsync(string query, CancellationToken token = default);
        #endregion
        #region Data Modification
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="Task{Int32}"/></returns>
        Task<int> ExecuteNonQueryAsync(string query, CancellationToken token = default);
#if !NET461 && !NETSTANDARD2_0
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure with a transaction
        /// </summary>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="Task{Int32}"/></returns>
        Task<int> ExecuteTransactedNonQueryAsync(string query, CancellationToken token = default);
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure with a transaction
        /// </summary>
        /// <param name="commitTransaction">Whether or not to commit this transaction after it was completed successfully</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="transact">An instance of a <see cref="DbTransaction"/> class</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="Task{Int32}"/></returns>
        Task<int> ExecuteTransactedNonQueryAsync(string query, DbTransaction transact, bool commitTransaction = false, CancellationToken token = default);
#endif
        #endregion
        #region Connection Methods
#if !NET461 && !NETSTANDARD2_0
        /// <summary>
        /// Changes the current <see cref="DbConnection"/> to target a different database
        /// </summary>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="databaseName">The name of a database as a <see cref="string"/></param>
        Task ChangeDatabaseAsync(string databaseName, CancellationToken token = default);
#endif
        /// <summary>
        /// Opens the connection to a database asynchronously
        /// </summary>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        Task OpenAsync(CancellationToken token = default);
        #endregion
    }
}