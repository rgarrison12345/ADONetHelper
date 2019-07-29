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

namespace ADONetHelper
{
    public partial interface IDbClient
    {
        #region Data Retrieval
#if !NETSTANDARD1_3
        /// <summary>
        /// Gets an instance of <see cref="DataSet"/>
        /// </summary>
        /// <param name="query">SQL query to use to build a <see cref="DataSet"/></param>
        /// <returns>Returns an instance of <see cref="DataSet"/> based on the <paramref name="query"/> passed into the routine</returns>
        DataSet GetDataSet(string query);
        /// <summary>
        /// Gets an instance of <see cref="DataSet"/>
        /// </summary>
        /// <param name="connection">An instance of a <see cref="DbConnection"/></param>
        /// <param name="query">SQL query to use to build a <see cref="DataSet"/></param>
        /// <returns>Returns an instance of <see cref="DataSet"/> based on the <paramref name="query"/> passed into the routine</returns>
        DataSet GetDataSet(string query, DbConnection connection);
        /// <summary>
        /// Utility method for returning a <see cref="DataTable"/> object created from the passed in query
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns a <see cref="DataTable"/> object</returns>
        DataTable GetDataTable(string query);
        /// <summary>
        /// Utility method for returning a <see cref="DataTable"/> object created from the passed in query
        /// </summary>
        /// <param name="connection">An instance of <see cref="DbConnection"/></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns a <see cref="DataTable"/> object</returns>
        DataTable GetDataTable(string query, DbConnection connection);
#endif
        /// <summary>
        /// Gets a single instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Gets an instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine.
        /// Or the default value of <typeparamref name="T"/> if there are no search results
        /// </returns>
        T GetDataObject<T>(string query) where T : class;
        /// <summary>
        /// Gets a list of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants create from query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns a <see cref="IEnumerable{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        IEnumerable<T> GetDataObjectEnumerable<T>(string query) where T : class;
        /// <summary>
        /// Gets a list of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <param name="connection">An instance of <see cref="DbConnection"/></param>
        /// <typeparam name="T">An instance of the type the caller wants create to from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns a <see cref="IEnumerable{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        IEnumerable<T> GetDataObjectEnumerable<T>(string query, DbConnection connection) where T : class;
        /// <summary>
        /// Utility method for returning a <see cref="DbDataReader"/> object created from the passed in query
        /// </summary>
        /// <param name="transact">An instance of <see cref="DbTransaction"/> to use with the passed in <paramref name="query"/></param>
        /// <param name="behavior">Provides a description of the results of the query and its effect on the database.  Defaults to <see cref="CommandBehavior.Default"/></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>An instance of <see cref="DbDataReader"/> object, the caller is responsible for handling closing the <see cref="DbDataReader"/>.  Once the data reader is closed, the database connection will be closed as well</returns>
        DbDataReader GetDbDataReader(string query, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null);
        /// <summary>
        /// Utility method for acting on a <see cref="DbDataReader"/>
        /// </summary>
        /// <param name="act">Action methods that takes in a <see cref="DbDataReader"/></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>A <see cref="DbDataReader"/> object, the caller is responsible for handling closing the <see cref="DbDataReader"/>.  Once the data reader is closed, the database connection will be closed as well</returns>
        void GetDbDataReader(string query, Action<DbDataReader> act);
        /// <summary>
        /// Utility method for returning a scalar value from the database
        /// </summary>
        /// <param name="transact">An instance of <see cref="DbTransaction"/></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the value of the first column in the first row as an object</returns>
        object GetScalarValue(string query, DbTransaction transact = null);
        #endregion
        #region Data Modification
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the amount of records affected by the passed in query</returns>
        int ExecuteNonQuery(string query);
        /// <summary>
        /// Utility method for executing a query or stored procedure in a SQL transaction
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query</returns>
        int ExecuteTransactedNonQuery(string query);
        /// <summary>
        /// Utility method for executing a query or stored procedure in a SQL transaction
        /// </summary>
        /// <param name="transact">An instance of a DbTransaction class</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="commitTransaction">Whether or not to commit the transaction that was passed in if successful</param>
        /// <returns>Returns the number of rows affected by this query</returns>
        int ExecuteTransactedNonQuery(string query, DbTransaction transact, bool commitTransaction);
        /// <summary>
        /// Utility method for executing batches of queries or stored procedures in a SQL transaction
        /// </summary>
        /// <param name="commands">The list of query database parameters that are associated with a query</param>
        /// <returns>Returns the number of rows affected by all queries passed in, assuming all are succesful</returns>
        List<int> ExecuteTransactedBatchedNonQuery(IEnumerable<SQLQuery> commands);
        /// <summary>
        /// Utility method for executing batches of queries or stored procedures in a SQL transaction
        /// </summary>
        /// <param name="commands">The list of query database parameters that are associated with a query</param>
        /// <param name="transact">An instance of a DbTransaction class</param>
        /// <returns>Returns the number of rows affected by all queries passed in, assuming all are succesful</returns>
        List<int> ExecuteTransactedBatchedNonQuery(IEnumerable<SQLQuery> commands, DbTransaction transact);
        #endregion
    }
}
