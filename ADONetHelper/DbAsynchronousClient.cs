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
    public partial class DbClient
    {
        #region Data Retrieval
        /// <summary>
        /// Gets a single instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Gets an instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine.
        /// Or the default value of <typeparamref name="T"/> if there are no search results
        /// </returns>
        public async Task<T> GetDataObjectAsync<T>(string query) where T : class
        {
            //Return this back to the caller
            return await this.GetDataObjectAsync<T>(query, CancellationToken.None).ConfigureAwait(false);
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
        public async Task<T> GetDataObjectAsync<T>(string query, CancellationToken token) where T : class
        {
            try
            {
                //Increment attempts
                attemptsMade++;

                //Return this back to the caller
                return await this.ExecuteSQL.GetDataObjectAsync<T>(this.QueryCommandType, query, token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                //Check if we should retry the attempt
                if (this.RetryAttempts > 0 && attemptsMade <= this.RetryAttempts)
                {
                    //Check if we need to wait before next try
                    if (this.RetryInterval > 0)
                    {
                        //Wait for the next attempt
                        await Task.Delay(this.RetryInterval, token).ConfigureAwait(false);
                    }

                    //Call this again
                    return await this.GetDataObjectAsync<T>(query, token).ConfigureAwait(false);
                }
                else
                {
                    //Throw this back to the caller
                    throw;
                }
            }
            finally
            {
                attemptsMade = 0;
            }
        }
        /// <summary>
        /// Gets a list of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants to create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns a <see cref="List{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public async Task<List<T>> GetDataObjectListAsync<T>(string query) where T : class
        {
            //Return this back to the caller
            return await this.GetDataObjectListAsync<T>(query, CancellationToken.None).ConfigureAwait(false);
        }
        /// <summary>
        /// Gets a list of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>Returns a <see cref="List{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public async Task<List<T>> GetDataObjectListAsync<T>(string query, CancellationToken token) where T : class
        {
            try
            {
                //Increment attempts
                attemptsMade++;

                //Return this back to the caller
                return await this.ExecuteSQL.GetDataObjectListAsync<T>(this.QueryCommandType, query, token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                //Check if we should retry the attempt
                if (this.RetryAttempts > 0 && attemptsMade <= this.RetryAttempts)
                {
                    //Check if we need to wait before next try
                    if (this.RetryInterval > 0)
                    {
                        //Wait for the next attempt
                        await Task.Delay(this.RetryInterval, token).ConfigureAwait(false);
                    }

                    //Call this again
                    return await this.GetDataObjectListAsync<T>(query, token).ConfigureAwait(false);
                }
                else
                {
                    //Throw this back to the caller
                    throw;
                }
            }
            finally
            {
                attemptsMade = 0;
            }
        }
#if !NETSTANDARD1_3
        /// <summary>
        /// Gets an instance of <see cref="DataTable"/> asynchronously
        /// </summary>
        /// <param name="query">SQL query to use to build a <see cref="DataTable"/></param>
        /// <returns>Returns a <see cref="Task{DataTable}"/> of datatable</returns>
        public async Task<DataTable> GetDataTableAsync(string query)
        {
            //Return this back to the caller
            return await this.GetDataTableAsync(query, CancellationToken.None).ConfigureAwait(false);
        }
        /// <summary>
        /// Gets an instance of <see cref="DataTable"/> asynchronously
        /// </summary>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="query">SQL query to use to build a <see cref="DataTable"/></param>
        /// <returns>Returns a <see cref="Task{DataTable}"/> of datatable</returns>
        public async Task<DataTable> GetDataTableAsync(string query, CancellationToken token)
        {
            try
            {
                DataTable dt = new DataTable();

                //Increment the number of attempts made
                attemptsMade++;

                //Wrap this in a using statement to automatically dispose of resources
                using (DbDataReader reader = await this.GetDbDataReaderAsync(query, token).ConfigureAwait(false))
                {
                    dt.Load(reader);
                }

                //Return this back to the caller
                return dt;
            }
            catch (Exception ex)
            {
                //Check if we should retry the attempt
                if (this.RetryAttempts > 0 && attemptsMade <= this.RetryAttempts)
                {
                    //Check if we need to wait before next try
                    if (this.RetryInterval > 0)
                    {
                        //Wait for the next attempt
                        await Task.Delay(this.RetryInterval, token).ConfigureAwait(false);
                    }

                    //Call this again
                    return await this.GetDataTableAsync(query, token).ConfigureAwait(false);
                }
                else
                {
                    //Throw this back to the caller
                    throw;
                }
            }
            finally
            {
                attemptsMade = 0;
            }
        }
#endif
        /// <summary>
        /// Utility method for returning a <see cref="Task{DbDataReader}"/> object created from the passed in query
        /// </summary>
        /// <param name="transact">An instance of <see cref="DbTransaction"/> to use with the passed in <paramref name="query"/></param>
        /// <param name="behavior">Provides a description of the results of the query and its effect on the database.  Defaults to <see cref="CommandBehavior.Default"/></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>A <see cref="Task{DbDataReader}"/> object, the caller is responsible for handling closing the <see cref="DbDataReader"/>.  Once the data reader is closed, the database connection will be closed as well</returns>
        public async Task<DbDataReader> GetDbDataReaderAsync(string query, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null)
        {
            //Return this back to the caller
            return await this.GetDbDataReaderAsync(query, CancellationToken.None, behavior, transact).ConfigureAwait(false);
        }
        /// <summary>
        /// Utility method for returning a <see cref="Task{DbDataReader}"/> object created from the passed in query
        /// </summary>
        /// <param name="transact">An instance of <see cref="DbTransaction"/></param>
        /// <param name="behavior">Provides a description of the results of the query and its effect on the database.  Defaults to <see cref="CommandBehavior.Default"/></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>A <see cref="Task{DbDataReader}"/> object, the caller is responsible for handling closing the <see cref="DbDataReader"/>.  Once the data reader is closed, the database connection will be closed as well</returns>
        public async Task<DbDataReader> GetDbDataReaderAsync(string query, CancellationToken token, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null)
        {
            try
            {
                //Increment attempts
                attemptsMade++;

                //Return this back to the caller
                return await this.ExecuteSQL.GetDbDataReaderAsync(this.QueryCommandType, query, token, behavior, transact).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                //Check if we should retry the attempt
                if (this.RetryAttempts > 0 && attemptsMade <= this.RetryAttempts)
                {
                    //Check if we need to wait before next try
                    if (this.RetryInterval > 0)
                    {
                        //Wait for the next attempt
                        await Task.Delay(this.RetryInterval, token).ConfigureAwait(false);
                    }

                    //Call this again
                    return await this.GetDbDataReaderAsync(query, token, behavior).ConfigureAwait(false);
                }
                else
                {
                    //Throw this back to the caller
                    throw;
                }
            }
            finally
            {
                attemptsMade = 0;
            }
        }
        /// <summary>
        /// Utility method for returning a <see cref="Task{Object}"/> value from the database
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the value of the first column in the first row as <see cref="Task"/></returns>
        public async Task<object> GetScalarValueAsync(string query)
        {
            //Return this back to the caller
            return await this.GetScalarValueAsync(query, CancellationToken.None).ConfigureAwait(false);
        }
        /// <summary>
        /// Utility method for returning a <see cref="Task{Object}"/> value from the database
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>Returns the value of the first column in the first row as <see cref="Task"/></returns>
        public async Task<object> GetScalarValueAsync(string query, CancellationToken token)
        {
            try
            {
                //Increment attempts
                attemptsMade++;

                //Return this back to the caller
                return await this.ExecuteSQL.GetScalarValueAsync(this.QueryCommandType, query, token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                //Check if we should retry the attempt
                if (this.RetryAttempts > 0 && attemptsMade <= this.RetryAttempts)
                {
                    //Check if we need to wait before next try
                    if (this.RetryInterval > 0)
                    {
                        //Wait for the next attempt
                        await Task.Delay(this.RetryInterval, token).ConfigureAwait(false);
                    }

                    //Call this again
                    return await this.GetScalarValueAsync(query, token).ConfigureAwait(false);
                }
                else
                {
                    //Throw this back to the caller
                    throw;
                }
            }
            finally
            {
                attemptsMade = 0;
            }
        }
        #endregion
        #region Data Modifications
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure with a transaction
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="Task{Int32}"/></returns>
        public async Task<int> ExecuteTransactedNonQueryAsync(string query)
        {
            try
            {
                attemptsMade++;

                //Return this back to the caller
                return await this.ExecuteSQL.ExecuteTransactedNonQueryAsync(this.QueryCommandType, query).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                //Check if we should retry the attempt
                if (this.RetryAttempts > 0 && attemptsMade <= this.RetryAttempts)
                {
                    //Check if we need to wait before next try
                    if (this.RetryInterval > 0)
                    {
                        //Wait for the next attempt
                        await Task.Delay(this.RetryInterval).ConfigureAwait(false);
                    }

                    //Call this again
                    return await this.ExecuteTransactedNonQueryAsync(query).ConfigureAwait(false);
                }
                else
                {
                    //Throw this back to the caller
                    throw;
                }
            }
            finally
            {
                //Reset if necessary
                if (attemptsMade >= this.RetryAttempts)
                {
                    attemptsMade = 0;
                }
            }
        }
        /// <summary>
        /// Utility method for executing a query or stored procedure in a SQL transaction
        /// </summary>
        /// <param name="transact">An instance of a <see cref="DbTransaction"/> class</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query</returns>
        public async Task<int> ExecuteTransactedNonQueryAsync(string query, DbTransaction transact)
        {
            try
            {
                //Keep incrementing
                attemptsMade++;

                //Return this back to the caller
                return await this.ExecuteSQL.ExecuteTransactedNonQueryAsync(this.QueryCommandType, transact, query).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                //Check if we should retry the attempt
                if (this.RetryAttempts > 0 && attemptsMade <= this.RetryAttempts)
                {
                    //Check if we need to wait before next try
                    if (this.RetryInterval > 0)
                    {
                        //Wait for the next attempt
                        await Task.Delay(this.RetryInterval).ConfigureAwait(false);
                    }

                    //Call this again
                    return await this.ExecuteTransactedNonQueryAsync(query, transact).ConfigureAwait(false);
                }
                else
                {
                    attemptsMade = 0;

                    //Throw this back to the caller
                    throw;
                }
            }
            finally
            {
                //Reset if necessary
                if (attemptsMade >= this.RetryAttempts)
                {
                    attemptsMade = 0;
                }
            }
        }
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="Task{Int32}"/></returns>
        public async Task<int> ExecuteNonQueryAsync(string query)
        {
            //Return this back to the caller
            return await this.ExecuteNonQueryAsync(query, CancellationToken.None).ConfigureAwait(false);
        }
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="Task{Int32}"/></returns>
        public async Task<int> ExecuteNonQueryAsync(string query, CancellationToken token)
        {
            try
            {
                attemptsMade++;

                //Return this back to the caller
                return await this.ExecuteSQL.ExecuteNonQueryAsync(this.QueryCommandType, query, token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                //Check if we should retry the attempt
                if (this.RetryAttempts > 0 && attemptsMade <= this.RetryAttempts)
                {
                    //Check if we need to wait before next try
                    if (this.RetryInterval > 0)
                    {
                        //Wait for the next attempt
                        await Task.Delay(this.RetryInterval, token).ConfigureAwait(false);
                    }

                    //Call this again
                    return await this.ExecuteNonQueryAsync(query, token).ConfigureAwait(false);
                }
                else
                {
                    //Throw this back to the caller
                    throw;
                }
            }
            finally
            {
                //Reset if necessary
                if (attemptsMade >= this.RetryAttempts)
                {
                    attemptsMade = 0;
                }
            }
        }
        #endregion
        #region Connection Methods
        /// <summary>
        /// Opens the connection to a database asynchronously
        /// </summary>
        public async Task OpenAsync()
        {
            //Open the database connection
            await this.OpenAsync(CancellationToken.None).ConfigureAwait(false);
        }
        /// <summary>
        /// Opens the connection to a database asynchronously
        /// </summary>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        public async Task OpenAsync(CancellationToken token)
        {
            try
            {
                //Increment attempts
                attemptsMade++;

                //Open the database connection
                await this.ExecuteSQL.Connection.OpenAsync(token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                //Check if we should retry the attempt
                if (this.RetryAttempts > 0 && attemptsMade <= this.RetryAttempts)
                {
                    //Check if we need to wait before next try
                    if (this.RetryInterval > 0)
                    {
                        //Wait for next attempt
                        await Task.Delay(this.RetryInterval, token).ConfigureAwait(false);
                    }

                    //Call this again and open the database connection
                    await this.OpenAsync(token).ConfigureAwait(false);
                }
                else
                {
                    //Throw this back to the caller
                    throw;
                }
            }
            finally
            {
                attemptsMade = 0;
            }
        }
        #endregion
    }
}
