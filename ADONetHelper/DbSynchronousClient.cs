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
using ADONetHelper.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Transactions;
#endregion

namespace ADONetHelper
{
    public partial class DbClient : ISynchronousClient
    {
        #region Data Retrieval
        /// <summary>
        /// Gets an instance of <see cref="DataSet"/>
        /// </summary>
        /// <param name="query">SQL query to use to build a <see cref="DataSet"/></param>
        /// <returns>Returns an instance of <see cref="DataSet"/> based on the <paramref name="query"/> passed into the routine</returns>
        public DataSet GetDataSet(string query)
        {
            //Return this back to the caller
            return GetDataSet(query, ExecuteSQL.Connection);
        }
        /// <summary>
        /// Gets an instance of <see cref="DataSet"/>
        /// </summary>
        /// <param name="connection">An instance of <see cref="DbConnection"/></param>
        /// <param name="query">SQL query to use to build a <see cref="DataSet"/></param>
        /// <returns>Returns an instance of <see cref="DataSet"/> based on the <paramref name="query"/> passed into the routine</returns>
        public DataSet GetDataSet(string query, DbConnection connection)
        {
            //Return this back to the caller
            return ExecuteSQL.GetDataSet(QueryCommandType, query, connection);
        }
        /// <summary>
        /// Gets an instance of <see cref="DataTable"/>
        /// </summary>
        /// <param name="query">SQL query to use to build a result set</param>
        /// <returns>Returns an instance of <see cref="DataTable"/></returns>
        public DataTable GetDataTable(string query)
        {
            //Return this back to the caller
            return GetDataTable(query, ExecuteSQL.Connection);
        }
        /// <summary>
        /// Gets an instance of <see cref="DataTable"/>
        /// </summary>
        /// <param name="connection">An instance of <see cref="DbConnection"/></param>
        /// <param name="query">SQL query to use to build a result set</param>
        /// <returns>Returns an instance of <see cref="DataTable"/></returns>
        public DataTable GetDataTable(string query, DbConnection connection)
        {
            //Return this back to the caller
            return ExecuteSQL.GetDataTable(QueryCommandType, query, connection);
        }
        /// <summary>
        /// Utility method for returning a <see cref="DbDataReader"/> object created from the passed in query
        /// </summary>
        /// <param name="transact">An instance of <see cref="DbTransaction"/> to use with the passed in <paramref name="query"/></param>
        /// <param name="behavior">Provides a description of the results of the query and its effect on the database.  Defaults to <see cref="CommandBehavior.Default"/></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>An instance of <see cref="DbDataReader"/></returns>
        public DbDataReader GetDbDataReader(string query, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null)
        {
            //Return this back to the caller
            return ExecuteSQL.GetDbDataReader(QueryCommandType, query, behavior, transact);
        }
        /// <summary>
        /// Utility method for acting on a <see cref="DbDataReader"/>
        /// </summary>
        /// <param name="act">Action methods that takes in a <see cref="DbDataReader"/></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>A <see cref="DbDataReader"/> object, the caller is responsible for handling closing the <see cref="DbDataReader"/>.  Once the data reader is closed, the database connection will be closed as well</returns>
        public void GetDbDataReader(string query, Action<DbDataReader> act)
        {

            //Return this back to the caller
            ExecuteSQL.GetDbDataReader(QueryCommandType, query, act);
        }
        /// <summary>
        /// Utility method for returning a scalar value as an <see cref="object"/> from the database
        /// </summary>
        /// <param name="transact">An instance of <see cref="DbTransaction"/></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the value of the first column in the first row as an object</returns>
        public object GetScalarValue(string query, DbTransaction transact = null)
        {
            //Return this back to the caller
            return ExecuteSQL.GetScalarValue(QueryCommandType, query, transact);
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
            //Return this back to the caller
            return GetDataObject<T>(query, ExecuteSQL.Connection);
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
            //Return this back to the caller
            return ExecuteSQL.GetDataObject<T>(QueryCommandType, query, connection);
        }
        /// <summary>
        /// Gets a list of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type the caller wants create to from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns a <see cref="IEnumerable{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public IEnumerable<T> GetDataObjectEnumerable<T>(string query) where T : class
        {
            //Return this back to the caller
            return GetDataObjectEnumerable<T>(query, ExecuteSQL.Connection);
        }
        /// <summary>
        /// Gets a list of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <param name="connection">An instance of <see cref="DbConnection"/></param>
        /// <typeparam name="T">An instance of the type the caller wants create to from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns a <see cref="IEnumerable{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public IEnumerable<T> GetDataObjectEnumerable<T>(string query, DbConnection connection) where T : class
        {
            //Return this back to the caller
            return ExecuteSQL.GetDataObjectEnumerable<T>(QueryCommandType, query, connection);
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
            //Return this back to the caller
            return ExecuteSQL.ExecuteNonQuery(QueryCommandType, query);
        }
        /// <summary>
        /// Utility method for executing batches of queries or stored procedures in a SQL transaction
        /// </summary>
        /// <param name="commands">The list of query database parameters that are associated with a query</param>
        /// <returns>Returns the number of rows affected by all queries passed in, assuming all are succesful</returns>
        public List<int> ExecuteBatchedNonQuery(IEnumerable<SQLQuery> commands)
        {
            //Return this back to the caller
            return ExecuteSQL.ExecuteBatchedNonQuery(commands);
        }
        /// <summary>
        /// Utility method for executing a query or stored procedure in a SQL transaction
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query</returns>
        public int ExecuteTransactedNonQuery(string query)
        {
            //Return this back to the caller
            return ExecuteSQL.ExecuteTransactedNonQuery(QueryCommandType, query);
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
            //Return this back to the caller
            return ExecuteSQL.ExecuteTransactedNonQuery(QueryCommandType, transact, query, commitTransaction);
        }
        /// <summary>
        /// Utility method for executing batches of queries or stored procedures in a SQL transaction
        /// </summary>
        /// <param name="commands">The list of query database parameters that are associated with a query</param>
        /// <returns>Returns the number of rows affected by all queries passed in, assuming all are succesful</returns>
        public List<int> ExecuteTransactedBatchedNonQuery(IEnumerable<SQLQuery> commands)
        {
            //Return this back to the caller
            return ExecuteSQL.ExecuteBatchedNonQuery(commands);
        }
        /// <summary>
        /// Utility method for executing batches of queries or stored procedures in a SQL transaction
        /// </summary>
        /// <param name="commands">The list of query database parameters that are associated with a query</param>
        /// <param name="transact">An instance of a DbTransaction class</param>
        /// <returns>Returns the number of rows affected by all queries passed in, assuming all are succesful</returns>
        public List<int> ExecuteTransactedBatchedNonQuery(IEnumerable<SQLQuery> commands, DbTransaction transact)
        {
            //Return this back to the caller
            return ExecuteSQL.ExecuteTransactedBatchedNonQuery(commands, transact);
        }
        #endregion
        #region Connection Methods
        /// <summary>
        /// Starts a <see cref="DbTransaction"/> using the underlying <see cref="DbConnection"/>
        /// </summary>
        /// <returns>Returns an instance of <see cref="DbTransaction"/></returns>
        public DbTransaction GetDbTransaction()
        {
            //Return this back to the caller
            return ExecuteSQL.Connection.BeginTransaction();
        }
        /// <summary>
        /// Starts a <see cref="DbTransaction"/> using the underlying <see cref="DbConnection"/> with the <paramref name="level"/>
        /// </summary>
        /// <param name="level">The <see cref="System.Data.IsolationLevel"/> to describe the locking behavior for the transaction</param>
        /// <returns>Returns an instance of <see cref="DbTransaction"/></returns>
        public DbTransaction GetDbTransaction(System.Data.IsolationLevel level)
        {
            //Return this back to the caller
            return ExecuteSQL.Connection.BeginTransaction(level);
        }
        /// <summary>
        /// Changes the current <see cref="DbConnection"/> to target a different database
        /// </summary>
        /// <param name="databaseName">The name of a database as a <see cref="string"/></param>
        public void ChangeDatabase(string databaseName)
        {
            //Now change the database
            ExecuteSQL.Connection.ChangeDatabase(databaseName);
        }
        /// <summary>
        /// Disposes of the <see cref="DbConnection"/> being used by this instance, clears any <see cref="DbParameter"/>
        /// assocatied with the current <see cref="DbConnection"/>
        /// </summary>
        public void Close()
        {
            //Clear params from this connection
            ClearParameters();

            //Dispose of the database connection
            ExecuteSQL.Connection.Dispose();
        }
        /// <summary>
        /// Opens the connection to a database
        /// </summary>
        public void Open()
        {
            //Check if we need to set the connection string
            if(ExecuteSQL.Connection.State == ConnectionState.Closed)
            {
                ExecuteSQL.Connection.ConnectionString = ConnectionString;
            }

            //Call this again
            ExecuteSQL.Connection.Open();
        }
        #endregion
        #region Other Methods
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
            ExecuteSQL.Connection.EnlistTransaction(transact);
        }
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
            return ExecuteSQL.Connection.GetSchema(collectionName, restrictionValues);
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
            return ExecuteSQL.Connection.GetSchema(collectionName);
        }
        /// <summary>
        /// Returns schema information for the data source of this <see cref="DbConnection"/>
        /// </summary>
        /// <returns>A <see cref="DataTable"/> that contains schema information</returns>
        /// <remarks>If the connection is associated with a transaction, executing GetSchema calls may cause some providers to throw an exception</remarks>
        public DataTable GetSchema()
        {
            //Return this back to the caller
            return ExecuteSQL.Connection.GetSchema();
        }
        /// <summary>
        /// Provides a mechanism for enumerating all available instances of database servers within the local network
        /// </summary>
        /// <returns>Returns a new instance of <see cref="DbDataSourceEnumerator"/> created by the current <see cref="DbProviderFactory"/></returns>
        public DbDataSourceEnumerator GetDataSourceEnumerator()
        {
            //Return this back to the caller
            return ExecuteSQL.Factory.GetDataSourceEnumerator();
        }
        #endregion
        #region IDisposable Support
        /// <summary>
        /// Dispose of any unmanged resorces if disposing passed in is true 
        /// </summary>
        /// <param name="disposing">Whether or not we need to explicitly close unmanaged resources</param>
        protected virtual void Dispose(bool disposing)
        {
            //Check if we have disposed before
            if (!disposedValue)
            {
                //Check if we are disposing
                if (disposing)
                {
                    //Close connection to the database
                    Close();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                disposedValue = true;
            }
        }
        /// <summary>
        /// Dispose of any unmanged resources
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}