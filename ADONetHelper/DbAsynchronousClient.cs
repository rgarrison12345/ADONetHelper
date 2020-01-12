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
#region Using Declarations
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
#endregion

 namespace ADONetHelper
{
    public partial class DbClient : IAsynchronousClient
    {
        #region Data Retrieval
        /// <summary>
        /// Gets an instance of <see cref="DataTable"/> asynchronously
        /// </summary>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="query">SQL query to use to build a <see cref="DataTable"/></param>
        /// <returns>Returns a <see cref="Task{DataTable}"/> of datatable</returns>
        public virtual async Task<DataTable> GetDataTableAsync(string query, CancellationToken token = default)
        {
            DataTable dt = new DataTable();

            //Wrap this in a using statement to automatically dispose of resources
            using (DbDataReader reader = await GetDbDataReaderAsync(query, token).ConfigureAwait(false))
            {
                dt.Load(reader);
            }

            //Return this back to the caller
            return dt;
        }
        /// <summary>
        /// Gets a single instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from query passed into procedure</typeparam>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Gets an instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine.
        /// Or the default value of <typeparamref name="T"/> if there are no search results
        /// </returns>
        public virtual async Task<T> GetDataObjectAsync<T>(string query, CancellationToken token = default) where T : class
        {
            //Return this back to the caller
            return await ExecuteSQL.GetDataObjectAsync<T>(QueryCommandType, query, token).ConfigureAwait(false);
        }
        /// <summary>
        /// Gets a list of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>Returns a <see cref="List{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public virtual async Task<List<T>> GetDataObjectListAsync<T>(string query, CancellationToken token = default) where T : class
        {
            //Return this back to the caller
            return await ExecuteSQL.GetDataObjectListAsync<T>(QueryCommandType, query, token).ConfigureAwait(false);
        }
        /// <summary>
        /// Gets a list of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>Returns a <see cref="IAsyncEnumerable{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public virtual async IAsyncEnumerable<T> GetDataObjectEnumerableAsync<T>(string query, [EnumeratorCancellation] CancellationToken token = default) where T : class
        {
            //Return this back to the caller
            await foreach (T type in ExecuteSQL.GetDataObjectEnumerableAsync<T>(QueryCommandType, query, token).ConfigureAwait(false))
            {
                yield return type;
            }
        }
        /// <summary>
        /// Utility method for returning a <see cref="Task{DbDataReader}"/> object created from the passed in query
        /// </summary>
        /// <param name="transact">An instance of <see cref="DbTransaction"/></param>
        /// <param name="behavior">Provides a description of the results of the query and its effect on the database.  Defaults to <see cref="CommandBehavior.Default"/></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>A <see cref="Task{DbDataReader}"/> object, the caller is responsible for handling closing the <see cref="DbDataReader"/>.  Once the data reader is closed, the database connection will be closed as well</returns>
        public virtual async Task<DbDataReader> GetDbDataReaderAsync(string query, CancellationToken token = default, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null)
        {
            //Return this back to the caller
            return await ExecuteSQL.GetDbDataReaderAsync(QueryCommandType, query, token, behavior, transact).ConfigureAwait(false);
        }
        /// <summary>
        /// Utility method for returning a <see cref="Task{Object}"/> value from the database
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>Returns the value of the first column in the first row as <see cref="Task"/></returns>
        public virtual async Task<object> GetScalarValueAsync(string query, CancellationToken token = default)
        {
            //Return this back to the caller
            return await ExecuteSQL.GetScalarValueAsync(QueryCommandType, query, token).ConfigureAwait(false);
        }
        #endregion
        #region Data Modifications
#if NETSTANDARD2_1
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure with a transaction
        /// </summary>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="Task{Int32}"/></returns>
        public virtual async Task<int> ExecuteTransactedNonQueryAsync(string query, CancellationToken token = default)
        {
            //Return this back to the caller
            return await ExecuteSQL.ExecuteTransactedNonQueryAsync(QueryCommandType, query, token).ConfigureAwait(false);
        }
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure with a transaction
        /// </summary>
        /// <param name="commitTransaction">Whether or not to commit this transaction after it was completed successfully</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="transact">An instance of a <see cref="DbTransaction"/> class</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="Task{Int32}"/></returns>
        public virtual async Task<int> ExecuteTransactedNonQueryAsync(string query, DbTransaction transact, bool commitTransaction = true, CancellationToken token = default)
        {
            //Return this back to the caller
            return await ExecuteSQL.ExecuteTransactedNonQueryAsync(QueryCommandType, transact, query, commitTransaction, token).ConfigureAwait(false);
        }
#endif
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="Task{Int32}"/></returns>
        public virtual async Task<int> ExecuteNonQueryAsync(string query, CancellationToken token = default)
        {
            //Return this back to the caller
            return await ExecuteSQL.ExecuteNonQueryAsync(QueryCommandType, query, token).ConfigureAwait(false);
        }
        #endregion
        #region Connection Methods
#if !NET461 && !NETSTANDARD2_0
        /// <summary>
        /// Changes the current <see cref="DbConnection"/> to target a different database
        /// </summary>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="databaseName">The name of a database as a <see cref="string"/></param>
        public async Task ChangeDatabaseAsync(string databaseName, CancellationToken token = default)
        {
            //Now change the database
            await ExecuteSQL.Connection.ChangeDatabaseAsync(databaseName, token).ConfigureAwait(false);
        }
#endif
        /// <summary>
        /// Opens the connection to a database asynchronously
        /// </summary>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        public async Task OpenAsync(CancellationToken token = default)
        {
            //Check if state is opened, might need a new connection string
            if (ExecuteSQL.Connection.State == ConnectionState.Closed)
            {
                //Set the connection string
                ExecuteSQL.Connection.ConnectionString = ConnectionString;
            }

            //Open the database connection
            await ExecuteSQL.Connection.OpenAsync(token).ConfigureAwait(false);
        }
        #endregion
    }
}