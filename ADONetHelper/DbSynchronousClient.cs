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
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
#if !NETSTANDARD1_3
using System.Transactions;
#endif
#endregion

namespace ADONetHelper
{ 
    public partial class DbClient
    {
        #region Data Retrieval
#if !NETSTANDARD1_3
        /// <summary>
        /// Gets an instance of <see cref="DataSet"/>
        /// </summary>
        /// <param name="query">SQL query to use to build a <see cref="DataSet"/></param>
        /// <returns>Returns an instance of <see cref="DataSet"/> based on the <paramref name="query"/> passed into the routine</returns>
        public DataSet GetDataSet(string query)
        {
            try
            {
                //Return this back to the caller
                return this.ExecuteSQL.GetDataSet(this.QueryCommandType, query);
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
                        Thread.Sleep(this.RetryInterval);
                    }

                    //Call this again
                    return this.GetDataSet(query);
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
        /// Gets an instance of <see cref="DataSet"/>
        /// </summary>
        /// <param name="connection">An instance of <see cref="DbConnection"/></param>
        /// <param name="query">SQL query to use to build a <see cref="DataSet"/></param>
        /// <returns>Returns an instance of <see cref="DataSet"/> based on the <paramref name="query"/> passed into the routine</returns>
        public DataSet GetDataSet(string query, DbConnection connection)
        {
            try
            {
                //Return this back to the caller
                return this.ExecuteSQL.GetDataSet(this.QueryCommandType, query, connection);
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
                        Thread.Sleep(this.RetryInterval);
                    }

                    //Call this again
                    return this.GetDataSet(query, connection);
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
        /// Gets an instance of <see cref="DataTable"/>
        /// </summary>
        /// <param name="query">SQL query to use to build a result set</param>
        /// <returns>Returns an instance of <see cref="DataTable"/></returns>
        public DataTable GetDataTable(string query)
        {
            try
            {
                //Keep incrementing the attempts
                attemptsMade++;

                //Return this back to the caller
                return this.ExecuteSQL.GetDataTable(this.QueryCommandType, query);
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
                        Thread.Sleep(this.RetryInterval);
                    }

                    //Call this again
                    return this.GetDataTable(query);
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
        /// Gets an instance of <see cref="DataTable"/>
        /// </summary>
        /// <param name="connection">An instance of <see cref="DbConnection"/></param>
        /// <param name="query">SQL query to use to build a result set</param>
        /// <returns>Returns an instance of <see cref="DataTable"/></returns>
        public DataTable GetDataTable(string query, DbConnection connection)
        {
            try
            {
                //Keep incrementing the attempts
                attemptsMade++;

                //Return this back to the caller
                return this.ExecuteSQL.GetDataTable(this.QueryCommandType, query);
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
                        Thread.Sleep(this.RetryInterval);
                    }

                    //Call this again
                    return this.GetDataTable(query, connection);
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
        /// Utility method for returning a <see cref="DbDataReader"/> object created from the passed in query
        /// </summary>
        /// <param name="transact">An instance of <see cref="DbTransaction"/> to use with the passed in <paramref name="query"/></param>
        /// <param name="behavior">Provides a description of the results of the query and its effect on the database.  Defaults to <see cref="CommandBehavior.Default"/></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>An instance of <see cref="DbDataReader"/></returns>
        public DbDataReader GetDbDataReader(string query, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null)
        {
            try
            {
                //Keep incrementing
                attemptsMade++;

                //Return this back to the caller
                return this.ExecuteSQL.GetDbDataReader(this.QueryCommandType, query, behavior, transact);
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
                        Thread.Sleep(this.RetryInterval);
                    }

                    //Call this again
                    return this.GetDbDataReader(query, behavior, transact);
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
        /// Utility method for acting on a <see cref="DbDataReader"/>
        /// </summary>
        /// <param name="act">Action methods that takes in a <see cref="DbDataReader"/></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>A <see cref="DbDataReader"/> object, the caller is responsible for handling closing the <see cref="DbDataReader"/>.  Once the data reader is closed, the database connection will be closed as well</returns>
        public void GetDbDataReader(string query, Action<DbDataReader> act)
        {
            try
            {
                //Keep incrementing
                attemptsMade++;

                //Return this back to the caller
                this.ExecuteSQL.GetDbDataReader(this.QueryCommandType, query, act);
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
                        Thread.Sleep(this.RetryInterval);
                    }

                    //Call this again
                    this.GetDbDataReader(query, act);
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
        /// Utility method for returning a scalar value as an <see cref="object"/> from the database
        /// </summary>
        /// <param name="transact">An instance of <see cref="DbTransaction"/></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the value of the first column in the first row as an object</returns>
        public object GetScalarValue(string query, DbTransaction transact = null)
        {
            try
            {
                //Keep incrementing
                attemptsMade++;

                //Return this back to the caller
                return this.ExecuteSQL.GetScalarValue(this.QueryCommandType, query, transact);
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
                        Thread.Sleep(this.RetryInterval);
                    }

                    //Call this again
                    return this.GetScalarValue(query, transact);
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
        /// Gets a single instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Gets an instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine.
        /// Or the default value of <typeparamref name="T"/> if there are no search results
        /// </returns>
        public T GetDataObject<T>(string query) where T : class
        {
            try
            {
                //Increment attempts
                attemptsMade++;

                //Return this back to the caller
                return this.ExecuteSQL.GetDataObject<T>(this.QueryCommandType, query);
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
                        Thread.Sleep(this.RetryInterval);
                    }

                    //Call this again
                    return this.GetDataObject<T>(query);
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
        /// Gets a single instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine
        /// </summary>
        /// <param name="connection">An instance of <see cref="DbConnection"/></param>
        /// <typeparam name="T">An instance of the type caller wants create from query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Gets an instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine.
        /// Or the default value of <typeparamref name="T"/> if there are no search results
        /// </returns>
        public T GetDataObject<T>(string query, DbConnection connection) where T : class
        {
            try
            {
                //Return this back to the caller
                return this.ExecuteSQL.GetDataObject<T>(this.QueryCommandType, query, connection);
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
                        Thread.Sleep(this.RetryInterval);
                    }

                    //Return this back to the caller
                    return this.GetDataObject<T>(query, connection);
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
        /// <typeparam name="T">An instance of the type the caller wants create to from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns a <see cref="List{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public List<T> GetDataObjectList<T>(string query) where T : class
        {
            try
            {
                //Increment attempts
                attemptsMade++;

                //Return this back to the caller
                return this.ExecuteSQL.GetDataObjectList<T>(this.QueryCommandType, query);
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
                        Thread.Sleep(this.RetryInterval);
                    }

                    //Call this again
                    return this.GetDataObjectList<T>(query);
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
        /// <param name="connection">An instance of <see cref="DbConnection"/></param>
        /// <typeparam name="T">An instance of the type the caller wants create to from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns a <see cref="List{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public List<T> GetDataObjectList<T>(string query, DbConnection connection) where T : class
        {
            try
            {
                //Increment attempts
                attemptsMade++;

                //Return this back to the caller
                return this.ExecuteSQL.GetDataObjectList<T>(this.QueryCommandType, query, connection);
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
                        Thread.Sleep(this.RetryInterval);
                    }

                    //Return this back to the caller
                    return this.GetDataObjectList<T>(query, connection);
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
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the amount of records affected by the passed in query</returns>
        public int ExecuteNonQuery(string query)
        {
            try
            {
                //Keep incrementing
                attemptsMade++;

                //Return this back to the caller
                return this.ExecuteSQL.ExecuteNonQuery(this.QueryCommandType, query);
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
                        Thread.Sleep(this.RetryInterval);
                    }

                    //Call this again
                    return this.ExecuteNonQuery(query);
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
        /// Utility method for executing batches of queries or stored procedures in a SQL transaction
        /// </summary>
        /// <param name="commands">The list of query database parameters that are associated with a query</param>
        /// <returns>Returns the number of rows affected by all queries passed in, assuming all are succesful</returns>
        public List<int> ExecuteBatchedNonQuery(IEnumerable<SQLQuery> commands)
        {
            try
            {
                //Keep incrementing
                attemptsMade++;

                //Return this back to the caller
                return this.ExecuteSQL.ExecuteBatchedNonQuery(commands);
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
                        Thread.Sleep(this.RetryInterval);
                    }

                    //Call this again
                    return this.ExecuteBatchedNonQuery(commands);
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
        /// Utility method for executing a query or stored procedure in a SQL transaction
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query</returns>
        public int ExecuteTransactedNonQuery(string query)
        {
            try
            {
                //Keep incrementing
                attemptsMade++;

                //Return this back to the caller
                return this.ExecuteSQL.ExecuteTransactedNonQuery(this.QueryCommandType, query);
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
                        Thread.Sleep(this.RetryInterval);
                    }

                    //Call this again
                    return this.ExecuteTransactedNonQuery(query);
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
        /// Utility method for executing a query or stored procedure in a SQL transaction
        /// </summary>
        /// <param name="transact">An instance of a <see cref="DbTransaction"/> class</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="commitTransaction">Whether or not to commit the transaction that was passed in if successful</param>
        /// <returns>Returns the number of rows affected by this query</returns>
        public int ExecuteTransactedNonQuery(string query, DbTransaction transact, bool commitTransaction = true)
        {
            try
            {
                //Keep incrementing
                attemptsMade++;

                //Return this back to the caller
                return this.ExecuteSQL.ExecuteTransactedNonQuery(this.QueryCommandType, transact, query, commitTransaction);
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
                        Thread.Sleep(this.RetryInterval);
                    }

                    //Call this again
                    return this.ExecuteTransactedNonQuery(query, transact, commitTransaction);
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
        /// Utility method for executing batches of queries or stored procedures in a SQL transaction
        /// </summary>
        /// <param name="commands">The list of query database parameters that are associated with a query</param>
        /// <returns>Returns the number of rows affected by all queries passed in, assuming all are succesful</returns>
        public List<int> ExecuteTransactedBatchedNonQuery(IEnumerable<SQLQuery> commands)
        {
            try
            {
                //Keep incrementing
                attemptsMade++;

                //Return this back to the caller
                return this.ExecuteSQL.ExecuteTransactedBatchedNonQuery(commands);
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
                        Thread.Sleep(this.RetryInterval);
                    }

                    //Call this again
                    return this.ExecuteTransactedBatchedNonQuery(commands);
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
        /// Utility method for executing batches of queries or stored procedures in a SQL transaction
        /// </summary>
        /// <param name="commands">The list of query database parameters that are associated with a query</param>
        /// <param name="transact">An instance of a DbTransaction class</param>
        /// <returns>Returns the number of rows affected by all queries passed in, assuming all are succesful</returns>
        public List<int> ExecuteTransactedBatchedNonQuery(IEnumerable<SQLQuery> commands, DbTransaction transact)
        {
            try
            {
                //Keep incrementing
                attemptsMade++;

                //Return this back to the caller
                return this.ExecuteSQL.ExecuteTransactedBatchedNonQuery(commands, transact);
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
                        Thread.Sleep(this.RetryInterval);
                    }

                    //Call this again
                    return this.ExecuteTransactedBatchedNonQuery(commands, transact);
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
        #endregion
        #region Connection Methods
        /// <summary>
        /// Starts a database transaction using the underlying <see cref="DbConnection"/>
        /// </summary>
        /// <returns>Returns an instance of <see cref="DbTransaction"/></returns>
        public DbTransaction GetDbTransaction()
        {
            //Return this back to the caller
            return this.ExecuteSQL.Connection.BeginTransaction();
        }
        /// <summary>
        /// Starts a <see cref="DbTransaction"/> using the underlying <see cref="DbConnection"/> with the <paramref name="level"/>
        /// </summary>
        /// <param name="level">The <see cref="System.Data.IsolationLevel"/> to describe the locking behavior for the transaction</param>
        /// <returns>Returns an instance of <see cref="DbTransaction"/></returns>
        public DbTransaction GetDbTransaction(System.Data.IsolationLevel level)
        {
            //Return this back to the caller
            return this.ExecuteSQL.Connection.BeginTransaction(level);
        }
#if !NETSTANDARD1_3
        /// <summary>
        /// Returns schema information for the data source of this <see cref="DbConnection"/> using the specified string for the schema name and the specified string array for the restriction values.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="restrictionValues">The restriction values.</param>
        /// <returns>A <see cref="DataTable"/> that contains schema information</returns>
        /// <remarks>If the connection is associated with a transaction, executing GetSchema calls may cause some providers to throw an exception</remarks>
        public DataTable GetSchema(string collectionName, string[] restrictionValues)
        {
            //Return this back to the caller
            return this.ExecuteSQL.Connection.GetSchema(collectionName, restrictionValues);
        }
        /// <summary>
        /// Returns schema information for the data source of this <paramref name="collectionName"/> using the specified string for the schema name.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <returns>A <see cref="DataTable"/> that contains schema information</returns>
        /// <remarks>If the connection is associated with a transaction, executing GetSchema calls may cause some providers to throw an exception</remarks>
        public DataTable GetSchema(string collectionName)
        {
            //Return this back to the caller
            return this.ExecuteSQL.Connection.GetSchema(collectionName);
        }
        /// <summary>
        /// Returns schema information for the data source of this <see cref="DbConnection"/>
        /// </summary>
        /// <returns>A <see cref="DataTable"/> that contains schema information</returns>
        /// <remarks>If the connection is associated with a transaction, executing GetSchema calls may cause some providers to throw an exception</remarks>
        public DataTable GetSchema()
        {
            //Return this back to the caller
            return this.ExecuteSQL.Connection.GetSchema();
        }
        /// <summary>
        /// Enlists the passed in <paramref name="transact"/> in a distributed transaction
        /// </summary>
        /// <param name="transact">An instance of <see cref="Transaction"/> to use to enlist a distributed transaction</param>
        /// <remarks>
        /// Because it enlists a connection in a Transaction instance, EnlistTransaction takes advantage of functionality available in the System.Transactions namespace for managing distributed transactions. 
        /// Once a connection is explicitly enlisted in a transaction, it cannot be unenlisted or enlisted in another transaction until the first transaction finishes. 
        /// </remarks>
        public void EnlistTransaction(Transaction transact)
        {
            this.ExecuteSQL.Connection.EnlistTransaction(transact);
        }
#endif
        /// <summary>
        /// Changes the current <see cref="DbConnection"/> to target a different database
        /// </summary>
        /// <param name="databaseName">The name of a database as a <see cref="string"/></param>
        public void ChangeDatabase(string databaseName)
        {
            //Now change the database
            this.ExecuteSQL.Connection.ChangeDatabase(databaseName);
        }
        /// <summary>
        /// Disposes of the <see cref="DbConnection"/> being used by this instance, clears any <see cref="DbParameter"/>
        /// assocatied with the current <see cref="DbConnection"/>
        /// </summary>
        public void Close()
        {
            try
            {
                //Increment attempts
                attemptsMade++;

                //Clear params from this connection
                this.ClearParameters();

                //Dispose of the database connection
                this.ExecuteSQL.Connection.Dispose();
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
                        Thread.Sleep(this.RetryInterval);
                    }

                    //Call this again
                    this.ExecuteSQL.Connection.Dispose();
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
        /// Opens the connection to a database
        /// </summary>
        public void Open()
        {
            try
            {
                //Increment attempts
                attemptsMade++;

                //Open the database connection
                this.ExecuteSQL.Connection.Open();
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
                        Thread.Sleep(this.RetryInterval);
                    }

                    //Call this again
                    this.Open();
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
