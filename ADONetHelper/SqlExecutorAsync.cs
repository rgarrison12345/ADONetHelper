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
#region Using Declarations
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
#endregion

namespace ADONetHelper
{
    public partial class SqlExecutor
    {
        #region Data Retrieval
        /// <summary>
        /// Gets a single instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Gets an instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine.
        /// Or the default value of <typeparamref name="T"/> if there are no search results
        /// </returns>
        public async Task<T> GetDataObjectAsync<T>(CommandType queryCommandType, string query) where T : class
        {
            //Return this back to the caller
            return await this.GetDataObjectAsync<T>(queryCommandType, query, this.Connection, default(CancellationToken)).ConfigureAwait(false);
        }
        /// <summary>
        /// Gets a single instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>Gets an instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine.
        /// Or the default value of <typeparamref name="T"/> if there are no search results
        /// </returns>
        public async Task<T> GetDataObjectAsync<T>(CommandType queryCommandType, string query, CancellationToken token) where T : class
        {
            //Return this back to the caller
            return await this.GetDataObjectAsync<T>(queryCommandType, query, this.Connection, token).ConfigureAwait(false);
        }
        /// <summary>
        /// Gets a single instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <returns>Gets an instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine.
        /// Or the default value of <typeparamref name="T"/> if there are no search results
        /// </returns>
        public async Task<T> GetDataObjectAsync<T>(CommandType queryCommandType, string query, string connectionString) where T : class
        {
            //Return this back to the caller
            return await this.GetDataObjectAsync<T>(queryCommandType, query, connectionString, default(CancellationToken)).ConfigureAwait(false);
        }
        /// <summary>
        /// Gets a single instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>Gets an instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine.
        /// Or the default value of <typeparamref name="T"/> if there are no search results
        /// </returns>
        public async Task<T> GetDataObjectAsync<T>(CommandType queryCommandType, string query, string connectionString, CancellationToken token) where T : class
        {
            //Wrap this in a using statement to automatically dispose of resources
            using (DbConnection connection = this.Factory.GetDbConnection(connectionString))
            {
                //Return this back to the caller
                return await this.GetDataObjectAsync<T>(queryCommandType, query, connection, token).ConfigureAwait(false);
            }
        }
        /// <summary>
        /// Gets a single instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="connection">An instance of a DbConnection object to use to query a datastore</param>
        /// <returns>Gets an instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine.
        /// Or the default value of <typeparamref name="T"/> if there are no search results
        /// </returns>
        public async Task<T> GetDataObjectAsync<T>(CommandType queryCommandType, string query, DbConnection connection) where T : class
        {
            //Return this back to the caller
            return await this.GetDataObjectAsync<T>(queryCommandType, query, connection, default(CancellationToken)).ConfigureAwait(false);
        }
        /// <summary>
        /// Gets an instance of the <typeparamref name="T"/> parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="connection">An instance of a DbConnection object to use to query a datastore</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>Gets an instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine.
        /// Or the default value of <typeparamref name="T"/> if there are no search results
        /// </returns>
        public async Task<T> GetDataObjectAsync<T>(CommandType queryCommandType, string query, DbConnection connection, CancellationToken token) where T : class
        {
            //Check if cancelled
            if (token.IsCancellationRequested == true)
            {
                token.ThrowIfCancellationRequested();
            }
           
            //Open the connection if necessary
            await Utilities.OpenDbConnectionAsync(connection, token).ConfigureAwait(false);

            //Wrap this to automatically handle disposing of resources
            using (DbDataReader reader = await this.GetDbDataReaderAsync(queryCommandType, query, connection, token, CommandBehavior.SingleRow).ConfigureAwait(false))
            {
                //Get the field name and value pairs out of this query
                List<IDictionary<string, object>> results = await this.GetDynamicResultsAsync(reader, token).ConfigureAwait(false);

                //Check if we need to return the default for the type
                if (results == null || results.Count == 0)
                {
                    //Return the default for the type back to the caller
                    return default(T);
                }
                else
                {
                    List<T> list = GetDynamicTypeList<T>(results);

                    //Return this back to the caller
                    return list[0];
                }
            }
        }
        /// <summary>
        /// Gets a list of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns a <see cref="List{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public async Task<List<T>> GetDataObjectListAsync<T>(CommandType queryCommandType, string query) where T : class
        {
            //Return this back to the caller
            return await this.GetDataObjectListAsync<T>(queryCommandType, query, this.Connection, default(CancellationToken)).ConfigureAwait(false);
        }
        /// <summary>
        /// Gets a list of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns a <see cref="List{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public async Task<List<T>> GetDataObjectListAsync<T>(CommandType queryCommandType, string query, CancellationToken token) where T : class
        {
            //Return this back to the caller
            return await this.GetDataObjectListAsync<T>(queryCommandType, query, this.Connection, token).ConfigureAwait(false);
        }
        /// <summary>
        /// Gets a list of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <returns>Returns a <see cref="List{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public async Task<List<T>> GetDataObjectListAsync<T>(CommandType queryCommandType, string query, string connectionString) where T : class
        {
            //Return this back to the caller
            return await this.GetDataObjectListAsync<T>(queryCommandType, query, connectionString, default(CancellationToken)).ConfigureAwait(false);
        }
        /// <summary>
        /// Gets a list of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>Returns a <see cref="List{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public async Task<List<T>> GetDataObjectListAsync<T>(CommandType queryCommandType, string query, string connectionString, CancellationToken token) where T : class
        {
            //Wrap this in a using statement to automatically dispose of resources
            using (DbConnection connection = this.Factory.GetDbConnection(connectionString))
            {
                //Return this back to the caller
                return await this.GetDataObjectListAsync<T>(queryCommandType, query, connection, token).ConfigureAwait(false);
            }
        }
        /// <summary>
        /// Gets a list of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="connection">An instance of a DbConnection object to use to query a datastore</param>
        /// <returns>Returns a <see cref="List{T}"/> based on the results of the passed in <paramref name="query"/>s</returns>
        public async Task<List<T>> GetDataObjectListAsync<T>(CommandType queryCommandType, string query, DbConnection connection) where T : class
        {
            //Return this back to the caller
            return await this.GetDataObjectListAsync<T>(queryCommandType, query, connection, default(CancellationToken)).ConfigureAwait(false);
        }
        /// <summary>
        /// Gets a list of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="connection">An instance of a DbConnection object to use to query a datastore</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>Returns a <see cref="List{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public async Task<List<T>> GetDataObjectListAsync<T>(CommandType queryCommandType, string query, DbConnection connection, CancellationToken token) where T : class
        {
            //Check if cancelled
            if (token.IsCancellationRequested == true)
            {
                token.ThrowIfCancellationRequested();
            }

            //Open the connection if necessary
            await Utilities.OpenDbConnectionAsync(connection, token).ConfigureAwait(false);

            //Wrap this to automatically handle disposing of resources
            using (DbDataReader reader = await this.GetDbDataReaderAsync(queryCommandType, query, connection, token, CommandBehavior.SingleResult).ConfigureAwait(false))
            {
                //Get the field name and value pairs out of this query
                List<IDictionary<string, object>> results = await this.GetDynamicResultsAsync(reader, token).ConfigureAwait(false);

                //Return this back to the caller
                return GetDynamicTypeList<T>(results);
            }
        }
        /// <summary>
        /// Utility method for returning a <see cref="Task{DbDataReader}"/> object
        /// </summary>
        /// <param name="behavior">Provides a description of the results of the query and its effect on the database.  Defaults to <see cref="CommandBehavior.CloseConnection"/></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <returns>Returns an instance of <see cref="DbDataReader"/>, the caller is responsible for handling closing the DataReader</returns>
        public async Task<DbDataReader> GetDbDataReaderAsync(CommandType queryCommandType, string connectionString, string query, CommandBehavior behavior = CommandBehavior.CloseConnection)
        {
            //Return this back to the caller
            return await this.GetDbDataReaderAsync(queryCommandType, connectionString, query, default(CancellationToken), behavior).ConfigureAwait(false);
        }
        /// <summary>
        /// Utility method for returning a <see cref="Task{DbDataReader}"/> object
        /// </summary>
        /// <param name="behavior">Provides a description of the results of the query and its effect on the database.  Defaults to <see cref="CommandBehavior.CloseConnection"/></param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <returns>Returns an instance of <see cref="DbDataReader"/>, the caller is responsible for handling closing the DataReader</returns>
        public async Task<DbDataReader> GetDbDataReaderAsync(CommandType queryCommandType, string connectionString, string query, CancellationToken token, CommandBehavior behavior = CommandBehavior.CloseConnection)
        {
            //Wrap this in a using statement to automatically dispose of resources
            using (DbConnection connection = this.Factory.GetDbConnection(connectionString))
            {
                //Return this back to the caller
                return await this.GetDbDataReaderAsync(queryCommandType, query, connection, token, behavior).ConfigureAwait(false);
            }
        }
        /// <summary>
        /// Utility method for returning a <see cref="Task{DbDataReader}"/> object
        /// </summary>
        /// <param name="transact">An instance of <see cref="DbTransaction"/></param>
        /// <param name="behavior">Provides a description of the results of the query and its effect on the database.  Defaults to <see cref="CommandBehavior.Default"/></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns an instance of <see cref="DbDataReader"/>, the caller is responsible for handling closing the DataReader</returns>
        public async Task<DbDataReader> GetDbDataReaderAsync(CommandType queryCommandType, string query, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null)
        {
            //Return this back to the caller
            return await this.GetDbDataReaderAsync(queryCommandType, query, default(CancellationToken), behavior, transact).ConfigureAwait(false);
        }
        /// <summary>
        /// Utility method for returning a <see cref="Task{DbDataReader}"/> object
        /// </summary>
        /// <param name="transact">An instance of <see cref="DbTransaction"/></param>
        /// <param name="behavior">Provides a description of the results of the query and its effect on the database.  Defaults to <see cref="CommandBehavior.Default"/></param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns an instance of <see cref="DbDataReader"/>, the caller is responsible for handling closing the DataReader</returns>
        public async Task<DbDataReader> GetDbDataReaderAsync(CommandType queryCommandType, string query, CancellationToken token, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null)
        {
            //Return this back to the caller
            return await this.GetDbDataReaderAsync(queryCommandType, query, this.Connection, token, behavior, transact).ConfigureAwait(false);
        }
        /// <summary>
        /// Utility method for returning a <see cref="Task{DbDataReader}"/> object
        /// </summary>
        /// <param name="transact">An instance of <see cref="DbTransaction"/></param>
        /// <param name="behavior">Provides a description of the results of the query and its effect on the database.  Defaults to <see cref="CommandBehavior.Default"/></param>
        /// <param name="connection">An instance of a <see cref="DbConnection"/> object to use to query a datastore</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns an instance of <see cref="DbDataReader"/>, the caller is responsible for handling closing the DataReader</returns>
        public async Task<DbDataReader> GetDbDataReaderAsync(CommandType queryCommandType, string query, DbConnection connection, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null)
        {
            //Return this back to the caller
            return await this.GetDbDataReaderAsync(queryCommandType, query, connection, default(CancellationToken), behavior, transact).ConfigureAwait(false);
        }
        /// <summary>
        /// Utility method for returning a <see cref="Task{DbDataReader}"/> object
        /// </summary>
        /// <param name="transact">An instance of <see cref="DbTransaction"/></param>
        /// <param name="behavior">Provides a description of the results of the query and its effect on the database.  Defaults to <see cref="CommandBehavior.Default"/></param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="connection">An instance of a <see cref="DbConnection"/> object to use to query a datastore</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns an instance of <see cref="DbDataReader"/> object, the caller is responsible for handling closing the DataReader</returns>
        public async Task<DbDataReader> GetDbDataReaderAsync(CommandType queryCommandType, string query, DbConnection connection, CancellationToken token, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null)
        {
            //Check if cancelled
            if (token.IsCancellationRequested == true)
            {
                token.ThrowIfCancellationRequested();
            }

            //Open the database connection if necessary
            await Utilities.OpenDbConnectionAsync(connection, token).ConfigureAwait(false);

            //Wrap this in a using statement to handle disposing of resources
            using (DbCommand command = this.Factory.GetDbCommand(queryCommandType, query, this.Parameters, connection, this.CommandTimeout, transact))
            {
                try
                {
                    //Get the data reader
                    return await command.ExecuteReaderAsync(behavior, token).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    //Set the output parameters
                    this._parameters = this.GetParameterList(command);
                }
            }
        }
        /// <summary>
        /// Utility method for returning a <see cref="Task{Object}"/> value from the database
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the value of the first column in the first row returned from the passed in query as an object</returns>
        public async Task<object> GetScalarValueAsync(CommandType queryCommandType, string query)
        {
            //Return this back to the caller
            return await this.GetScalarValueAsync(queryCommandType, query, this.Connection, default(CancellationToken)).ConfigureAwait(false);
        }
        /// <summary>
        /// Utility method for returning a <see cref="Task{Object}"/> value from the database
        /// </summary>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the value of the first column in the first row returned from the passed in query as an object</returns>
        public async Task<object> GetScalarValueAsync(CommandType queryCommandType, string query, CancellationToken token)
        {
            //Return this back to the caller
            return await this.GetScalarValueAsync(queryCommandType, query, this.Connection, token).ConfigureAwait(false);
        }
        /// <summary>
        /// Utility method for returning a <see cref="Task{Object}"/> value from the database
        /// </summary>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the value of the first column in the first row returned from the passed in query as an object</returns>
        public async Task<object> GetScalarValueAsync(CommandType queryCommandType, string query, string connectionString)
        {
            //Return this back to the caller
            return await GetScalarValueAsync(queryCommandType, query, connectionString, default(CancellationToken)).ConfigureAwait(false);
        }
        /// <summary>
        /// Utility method for returning a <see cref="Task{Object}"/> value from the database
        /// </summary>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the value of the first column in the first row returned from the passed in query as an object</returns>
        public async Task<object> GetScalarValueAsync(CommandType queryCommandType, string query, string connectionString, CancellationToken token)
        {
            //Wrap this in a using statement to handle disposing of resources
            using (DbConnection connection = this.Factory.GetDbConnection(connectionString))
            {
                //Return this back to the caller
                return await GetScalarValueAsync(queryCommandType, query, connection, token).ConfigureAwait(false);
            }
        }
        /// <summary>
        /// Utility method for returning a <see cref="Task{Object}"/> value from the database
        /// </summary>
        /// <param name="connection">An instance of a <see cref="DbConnection"/> object to use to query a datastore</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the value of the first column in the first row returned from the passed in query as an object</returns>
        public async Task<object> GetScalarValueAsync(CommandType queryCommandType, string query, DbConnection connection)
        {
            //Return this back to the caller
            return await this.GetScalarValueAsync(queryCommandType, query, connection, default(CancellationToken)).ConfigureAwait(false);
        }
        /// <summary>
        /// Utility method for returning a <see cref="Task{Object}"/> value from the database
        /// </summary>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="connection">An instance of a <see cref="DbConnection"/> object to use to query a datastore</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the value of the first column in the first row returned from the passed in query as an object</returns>
        public async Task<object> GetScalarValueAsync(CommandType queryCommandType, string query, DbConnection connection, CancellationToken token)
        {
            //Check if cancelled
            if(token.IsCancellationRequested == true)
            {
                token.ThrowIfCancellationRequested();
            }

            //Open the connection to the database
            await Utilities.OpenDbConnectionAsync(connection, token).ConfigureAwait(false);

            //Wrap this in a using statement to handle disposing of resources
            using (DbCommand command = this.Factory.GetDbCommand(queryCommandType, query, this.Parameters, connection, this.CommandTimeout))
            {
                try
                {
                    //Return this back to the caller
                    return await command.ExecuteScalarAsync(token).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    //Set the output parameters
                    this._parameters = this.GetParameterList(command);
                }
            }
        }
        #endregion
        #region Data Modification
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="Task{Int32}"/></returns>
        public async Task<int> ExecuteNonQueryAsync(CommandType queryCommandType, string query)
        {
            //Execute this query
            return await this.ExecuteNonQueryAsync(queryCommandType, query, default(CancellationToken)).ConfigureAwait(false);
        }
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="token">Propagates notification that operations should be canceled</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="Task{Int32}"/></returns>
        public async Task<int> ExecuteNonQueryAsync(CommandType queryCommandType, string query, CancellationToken token)
        {
            //Execute this query
            return await this.ExecuteNonQueryAsync(queryCommandType, query, this.Connection, token).ConfigureAwait(false);
        }
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="Task{Int32}"/></returns>
        public async Task<int> ExecuteNonQueryAsync(CommandType queryCommandType, string query, string connectionString)
        {
            //Execute this query
            return await this.ExecuteNonQueryAsync(queryCommandType, query, connectionString, default(CancellationToken)).ConfigureAwait(false);
        }
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="token">Propagates notification that operations should be canceled</param>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="Task{Int32}"/></returns>
        public async Task<int> ExecuteNonQueryAsync(CommandType queryCommandType, string query, string connectionString, CancellationToken token)
        {
            //Wrap this in a using statement to handle disposing of resources
            using (DbConnection connection = this.Factory.GetDbConnection(connectionString))
            {
                //Execute this query
                return await this.ExecuteNonQueryAsync(queryCommandType, query, connection, token).ConfigureAwait(false);
            }
        }
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="connection">An instance of a <see cref="DbConnection"/> object to use to query a datastore</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="Task{Int32}"/></returns>
        public async Task<int> ExecuteNonQueryAsync(CommandType queryCommandType, string query, DbConnection connection)
        {
            //Return this back to the caller
            return await this.ExecuteNonQueryAsync(queryCommandType, query, connection, default(CancellationToken)).ConfigureAwait(false);
        }
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="token">Propagates notification that operations should be canceled</param>
        /// <param name="connection">An instance of a <see cref="DbConnection"/> object to use to query a datastore</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="Task{Int32}"/></returns>
        public async Task<int> ExecuteNonQueryAsync(CommandType queryCommandType, string query, DbConnection connection, CancellationToken token)
        {
            //Check if cancelled
            if (token.IsCancellationRequested == true)
            {
                token.ThrowIfCancellationRequested();
            }

            //Open the database connection if necessary
            await Utilities.OpenDbConnectionAsync(connection, token).ConfigureAwait(false);

            //Wrap this in a using statement to automatically handle disposing of resources
            using (DbCommand command = this.Factory.GetDbCommand(queryCommandType, query, this.Parameters, connection, this.CommandTimeout))
            {
                try
                {
                    //Return this back to the caller
                    return await command.ExecuteNonQueryAsync(token).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    //Set the output parameters
                    this._parameters = this.GetParameterList(command);
                }
            }
        }
        /// <summary>
        /// Utility method for executing a query or stored procedure in a SQL transaction
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the number of rows affected by this query</returns>
        public async Task<int> ExecuteTransactedNonQueryAsync(CommandType queryCommandType, string query)
        {
            //Return this back to the caller
            return await this.ExecuteTransactedNonQueryAsync(queryCommandType, query, this.Connection).ConfigureAwait(false);
        }
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="Task{Int32}"/></returns>
        public async Task<int> ExecuteTransactedNonQueryAsync(CommandType queryCommandType, string query, string connectionString)
        {
            //Wrap this in a using statement to handle disposing of resources
            using (DbConnection connection = this.Factory.GetDbConnection(connectionString))
            {
                //Return this back to the caller
                return await this.ExecuteTransactedNonQueryAsync(queryCommandType, query, connection).ConfigureAwait(false);
            }
        }
        /// <summary>
        /// Utility method for executing a query or stored procedure in a SQL transaction
        /// </summary>
        /// <param name="commitTransaction">Whether or not to commit this transaction after it was completed successfully</param>
        /// <param name="transact">An instance of a <see cref="DbTransaction"/> class</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the number of rows affected by this query</returns>
        public async Task<int> ExecuteTransactedNonQueryAsync(CommandType queryCommandType, DbTransaction transact, string query, bool commitTransaction = true)
        {
            //Open the connection
            await Utilities.OpenDbConnectionAsync(this.Connection).ConfigureAwait(false);

            //Wrap this in a using statement to automatically dispose of resources
            using (DbCommand command = this.Factory.GetDbCommand(queryCommandType, query, this.Parameters, this.Connection, this.CommandTimeout))
            {
                try
                {
                    //Get the number of records affected by this query
                    int recordsAffected = await command.ExecuteNonQueryAsync().ConfigureAwait(false);

                    //Check if we need to commit this
                    if (commitTransaction == true)
                    {
                        //Now commit the transaction
                        await Task.Run(() => command.Transaction.Commit()).ConfigureAwait(false);
                    }

                    //Return this back to the caller
                    return recordsAffected;
                }
                catch (Exception ex)
                {
                    try
                    {
                        //Automatically roll back the error
                        await Task.Run(() => transact.Rollback()).ConfigureAwait(false);

                        //Need to throw this up the stack
                        throw;
                    }
                    catch (Exception innerEx)
                    {
                        throw;
                    }
                }
                finally
                {
                    //Set the output parameters
                    this._parameters = this.GetParameterList(command);
                }
            }
        }
        /// <summary>
        /// Utility method for executing a query or stored procedure in a SQL transaction
        /// </summary>
        /// <param name="connection">An instance of a <see cref="DbConnection"/> object to use to query a datastore</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the number of rows affected by this query</returns>
        public async Task<int> ExecuteTransactedNonQueryAsync(CommandType queryCommandType, string query, DbConnection connection)
        {
            //Open the connection
            await Utilities.OpenDbConnectionAsync(connection).ConfigureAwait(false);

            //Wrap this in a using statement to automatically handle disposing of resources
            using (DbTransaction transact = this.Factory.GetDbTransaction(this.Connection))
            {
                //Wrap this in a using statement to automatically handle disposing of resources
                using (DbCommand command = this.Factory.GetDbCommand(queryCommandType, query, this.Parameters, this.Connection, this.CommandTimeout))
                {
                    try
                    {
                        //Set the transaction
                        command.Transaction = transact;

                        //Return this back to the caller
                        int recordsAffected = await command.ExecuteNonQueryAsync().ConfigureAwait(false);

                        //Now commit the transaction
                        await Task.Run(() => command.Transaction.Commit()).ConfigureAwait(false);

                        //Return this back to the caller
                        return recordsAffected;
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            //Automatically roll back the error
                            await Task.Run(() => transact.Rollback()).ConfigureAwait(false);

                            //Need to throw this up the stack
                            throw;
                        }
                        catch (Exception innerEx)
                        {
                            throw;
                        }
                    }
                    finally
                    {
                        //Set the output parameters
                        this._parameters = this.GetParameterList(command);
                    }
                }
            }
        }
        #endregion
        #region Helper Methods
        /// <summary>
        /// Gets the query values coming out of the passed in <paramref name="reader"/> for each row retrieved
        /// </summary>
        /// <param name="token">Propagates notification that operations should be canceled</param>
        /// <param name="reader">An instance of <see cref="DbDataReader"/> that has the results from a SQL query</param>
        /// <returns>Returns a <see cref="List{T}"/> of <see cref="Dictionary{TKey, TValue}"/> from the results of a sql query</returns>
        private async Task<List<IDictionary<string, object>>> GetDynamicResultsAsync(DbDataReader reader, CancellationToken token)
        {
            List<IDictionary<string, object>> results = new List<IDictionary<string, object>>();

            //Keep reading records while there are records to read
            while (await reader.ReadAsync(token).ConfigureAwait(false) == true)
            {
                Dictionary<string, object> obj = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);

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
        #endregion
    }
}