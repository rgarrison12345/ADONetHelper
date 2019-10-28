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
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
#endregion

namespace ADONetHelper.Core
{
    /// <summary>
    /// /Contract class that defines syncrhonous operations against a database
    /// </summary>
    public interface ISqlExecutorSync
    {
        #region Data Retrieval
        /// <summary>
        /// Gets an instance of <see cref="DataSet"/>
        /// </summary>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="connection">An instance of <see cref="DbConnection"/></param>
        /// <param name="query">SQL query to use to build a <see cref="DataSet"/></param>
        /// <returns>Returns an instance of <see cref="DataSet"/> based on the <paramref name="query"/> passed into the routine</returns>
        DataSet GetDataSet(CommandType queryCommandType, string query, DbConnection connection);
        /// <summary>
        /// Gets an instance of <see cref="DataSet"/>
        /// </summary>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="query">SQL query to use to build a <see cref="DataSet"/></param>
        /// <returns>Returns an instance of <see cref="DataSet"/> based on the <paramref name="query"/> passed into the routine</returns>
        DataSet GetDataSet(CommandType queryCommandType, string query);
        /// <summary>
        /// Gets an instance of <see cref="DataTable"/>
        /// </summary>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="query">SQL query to use to build a result set</param>
        /// <returns>Returns an instance of <see cref="DataTable"/></returns>
        DataTable GetDataTable(CommandType queryCommandType, string query);
        /// <summary>
        /// Gets an instance of <see cref="DataTable"/>
        /// </summary>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="connection">An instance of <see cref="DbConnection"/></param>
        /// <param name="query">SQL query to use to build a result set</param>
        /// <returns>Returns an instance of <see cref="DataTable"/></returns>
        DataTable GetDataTable(CommandType queryCommandType, string query, DbConnection connection);
        /// <summary>
        /// Gets a single instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns an instance of the <typeparamref name="T"/> based on the fields in the passed in query.  Returns the default value for the type if a record is not found</returns>
        T GetDataObject<T>(CommandType queryCommandType, string query) where T : class;
        /// <summary>
        /// Gets a single instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <returns>Returns an instance of the <typeparamref name="T"/> based on the fields in the passed in query.  Returns the default value for the type if a record is not found</returns>
        T GetDataObject<T>(CommandType queryCommandType, string query, string connectionString) where T : class;
        /// <summary>
        /// Gets a single instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="connection">An instance of a DbConnection object to use to query a datastore</param>
        /// <returns>Returns an instance of the <typeparamref name="T"/> based on the fields in the passed in query.  Returns the default value for the type if a record is not found</returns>
        T GetDataObject<T>(CommandType queryCommandType, string query, DbConnection connection) where T : class;
        /// <summary>
        /// Gets a <see cref="List{T}"/> of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants created from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns a <see cref="List{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        List<T> GetDataObjectList<T>(CommandType queryCommandType, string query);
        /// <summary>
        /// Gets a <see cref="List{T}"/> of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants created from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <returns>Returns a <see cref="List{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        List<T> GetDataObjectList<T>(CommandType queryCommandType, string query, string connectionString);
        /// <summary>
        /// Gets a <see cref="List{T}"/> of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants created from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="connection">An instance of a <see cref="DbConnection"/> object to use to query a datastore</param>
        /// <returns>Returns a <see cref="List{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        List<T> GetDataObjectList<T>(CommandType queryCommandType, string query, DbConnection connection);
        /// <summary>
        /// Gets a <see cref="IEnumerable{T}"/> of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns a <see cref="IEnumerable{T}"/> based on the results of the passedin <paramref name="query"/></returns>
        IEnumerable<T> GetDataObjectEnumerable<T>(CommandType queryCommandType, string query);
        /// <summary>
        /// Gets a <see cref="IEnumerable{T}"/> of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <returns>Returns a <see cref="IEnumerable{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        IEnumerable<T> GetDataObjectEnumerable<T>(CommandType queryCommandType, string query, string connectionString);
        /// <summary>
        /// Gets a <see cref="IEnumerable{T}"/> of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="connection">An instance of a DbConnection object to use to query a datastore</param>
        /// <returns>Returns a <see cref="IEnumerable{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        IEnumerable<T> GetDataObjectEnumerable<T>(CommandType queryCommandType, string query, DbConnection connection);
        /// <summary>
        /// Utility method for returning a <see cref="DbDataReader"/>
        /// </summary>
        /// <param name="transact">An instance of <see cref="DbTransaction"/></param>
        /// <param name="behavior">Provides a description of the results of the query and its effect on the database.  Defaults to <see cref="CommandBehavior.Default"/></param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>An instance of <see cref="DbDataReader"/></returns>
        DbDataReader GetDbDataReader(CommandType queryCommandType, string query, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null);
        /// <summary>
        /// Utility method for returning a <see cref="DbDataReader"/>
        /// </summary>
        /// <param name="transact">An instance of <see cref="DbTransaction"/></param>
        /// <param name="behavior">Provides a description of the results of the query and its effect on the database.  Defaults to <see cref="CommandBehavior.Default"/></param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="connection">An instance of the DbConnection class</param>
        /// <returns>An instance of <see cref="DbDataReader"/></returns>
        DbDataReader GetDbDataReader(CommandType queryCommandType, string query, DbConnection connection, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null);
        /// <summary>
        /// Utility method for returning a DataReader object
        /// </summary>
        /// <param name="behavior">Provides a description of the results of the query and its effect on the database.  Defaults to <see cref="CommandBehavior.Default"/></param>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>An instance of <see cref="DbDataReader"/> object</returns>
        DbDataReader GetDbDataReader(CommandType queryCommandType, string connectionString, string query, CommandBehavior behavior = CommandBehavior.Default);
        /// <summary>
        /// Utility method for acting on a <see cref="DbDataReader"/>
        /// </summary>
        /// <param name="act">Action methods that takes in a <see cref="DbDataReader"/></param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>A <see cref="DbDataReader"/> object, the caller is responsible for handling closing the <see cref="DbDataReader"/>.  Once the data reader is closed, the database connection will be closed as well</returns>
        void GetDbDataReader(CommandType queryCommandType, string query, Action<DbDataReader> act);
        /// <summary>
        /// Utility method for returning a scalar value from the database
        /// </summary>
        /// <param name="transact">An instance of <see cref="DbTransaction"/></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the value of the first column in the first row returned from the passed in query as an object</returns>
        object GetScalarValue(CommandType queryCommandType, string query, DbTransaction transact = null);
        /// <summary>
        /// Utility method for returning a scalar value from the database
        /// </summary>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the value of the first column in the first row as an object</returns>
        object GetScalarValue(CommandType queryCommandType, string query, string connectionString);
        /// <summary>
        /// Utility method for returning a scalar value from the database
        /// </summary>
        /// <param name="transact">An instance of <see cref="DbTransaction"/></param>
        /// <param name="connection">An instanace of the DbConnection object used to query a data store</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the value of the first column in the first row as an object</returns>
        object GetScalarValue(CommandType queryCommandType, string query, DbConnection connection, DbTransaction transact = null);
        #endregion
        #region Data Modification
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query</returns>
        int ExecuteNonQuery(CommandType queryCommandType, string query);
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <returns>Returns the number of rows affected by this query</returns>
        int ExecuteNonQuery(CommandType queryCommandType, string query, string connectionString);
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="connection">An instance of the DbConnection object to use to query a data store</param>
        /// <returns>Returns the number of rows affected by this query</returns>
        int ExecuteNonQuery(CommandType queryCommandType, string query, DbConnection connection);
        /// <summary>
        /// Utility method for executing batches of queries or stored procedures in a SQL transaction
        /// </summary>
        /// <param name="commands">The list of query database parameters that are associated with a query</param>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <returns>Returns the number of rows affected by all queries passed in</returns>
        List<int> ExecuteBatchedNonQuery(string connectionString, IEnumerable<SQLQuery> commands);
        /// <summary>
        /// Utility method for executing batches of queries or stored procedures in a SQL transaction
        /// </summary>
        /// <param name="commands">The list of query database parameters that are associated with a query</param>
        /// <returns>Returns the number of rows affected by all queries passed in, assuming all are succesful</returns>
        List<int> ExecuteBatchedNonQuery(IEnumerable<SQLQuery> commands);
        /// <summary>
        /// Utility method for executing batches of queries or stored procedures in a SQL transaction
        /// </summary>
        /// <param name="commands">The list of query database parameters that are associated with a query</param>
        /// <param name="connection">An instance of a DbConnection object to use to query a datastore</param>
        /// <returns>Returns the number of rows affected by all queries passed in, assuming all are succesful</returns>
        List<int> ExecuteBatchedNonQuery(IEnumerable<SQLQuery> commands, DbConnection connection);
        /// <summary>
        /// Utility method for executing batches of queries or stored procedures in a SQL transaction
        /// </summary>
        /// <param name="commands">The list of query database parameters that are associated with a query</param>
        /// <returns>Returns the number of rows affected by all queries passed in, assuming all are succesful</returns>
        List<int> ExecuteTransactedBatchedNonQuery(IEnumerable<SQLQuery> commands);
        /// <summary>
        /// Utility method for executing batches of queries or stored procedures in a SQL transaction
        /// </summary>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <param name="commands">The list of query database parameters that are associated with a query</param>
        /// <returns>Returns the number of rows affected by all queries passed in, assuming all are succesful</returns>
        List<int> ExecuteTransactedBatchedNonQuery(string connectionString, IEnumerable<SQLQuery> commands);
        /// <summary>
        /// Utility method for executing batches of queries or stored procedures in a SQL transaction
        /// </summary>
        /// <param name="connection">An instance of a DbConnection object to use to query a datastore</param>
        /// <param name="commands">The list of query database parameters that are associated with a query</param>
        /// <returns>Returns the number of rows affected by all queries passed in, assuming all are succesful</returns>
        List<int> ExecuteTransactedBatchedNonQuery(IEnumerable<SQLQuery> commands, DbConnection connection);
        /// <summary>
        /// Utility method for executing batches of queries or stored procedures in a SQL transaction
        /// </summary>
        /// <param name="commands">The list of query database parameters that are associated with a query</param>
        /// <param name="transact">An instance of a DbTransaction class</param>
        /// <returns>Returns the number of rows affected by all queries passed in, assuming all are succesful</returns>
        List<int> ExecuteTransactedBatchedNonQuery(IEnumerable<SQLQuery> commands, DbTransaction transact);
        /// <summary>
        /// Utility method for executing batches of queries or stored procedures in a SQL transaction
        /// </summary>
        /// <param name="commands">The list of query database parameters that are associated with a query</param>
        /// <param name="connection">An instance of the DbConnection object to use to query a data store</param>
        /// <param name="transact">An instance of a DbTransaction class</param>
        /// <returns>Returns the number of rows affected by all queries passed in, assuming all are succesful</returns>
        List<int> ExecuteTransactedBatchedNonQuery(IEnumerable<SQLQuery> commands, DbConnection connection, DbTransaction transact);
        /// <summary>
        /// Utility method for executing a query or stored procedure in a SQL transaction
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the number of rows affected by this query</returns>
        int ExecuteTransactedNonQuery(CommandType queryCommandType, string query);
        /// <summary>
        /// Utility method for executing a query or stored procedure in a SQL transaction
        /// </summary>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <returns>Returns the number of rows affected by this query</returns>
        int ExecuteTransactedNonQuery(CommandType queryCommandType, string query, string connectionString);
        /// <summary>
        /// Utility method for executing a query or stored procedure in a SQL transaction
        /// </summary>
        /// <param name="connection">An instance of a DbConnection object to use to query a datastore</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the number of rows affected by this query</returns>
        int ExecuteTransactedNonQuery(CommandType queryCommandType, string query, DbConnection connection);
        /// <summary>
        /// Utility method for executing a query or stored procedure in a SQL transaction
        /// </summary>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="transact">An instance of a DbTransaction class</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="commitTransaction">Whether or not to commit the transaction that is passed in if succesful</param>
        /// <returns>Returns the number of rows affected by this query</returns>
        int ExecuteTransactedNonQuery(CommandType queryCommandType, DbTransaction transact, string query, bool commitTransaction);
        /// <summary>
        /// Utility method for executing a query or stored procedure in a SQL transaction
        /// </summary>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="transact">An instance of a DbTransaction class</param>
        /// <param name="connection">An instance of a DbConnection object to use to query a datastore</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="commitTransaction">Whether or not to commit the transaction that is passed in if succesful</param>
        /// <returns>Returns the number of rows affected by this query</returns>
        int ExecuteTransactedNonQuery(CommandType queryCommandType, DbConnection connection, DbTransaction transact, string query, bool commitTransaction);
        #endregion
    }
}