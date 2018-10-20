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
#if !NETSTANDARD1_3
using System.Transactions;
#endif
#endregion

namespace ADONetHelper
{
    /// <summary>
    /// Contract class for all classes that implement the DbClient pattern
    /// </summary>
    /// <seealso cref="IDisposable"/>
    /// <seealso cref="IDbParameterUtility"/>
    /// <seealso cref="IConnectionStringUtility"/>
    public partial interface IDbClient : IDbParameterUtility, IConnectionStringUtility, IDisposable
    {
        #region Events        
        /// <summary>
        /// Sets the state change event handler.  This event occurs when the <see cref="DbConnection.State"/> changes
        /// </summary>
        /// <value>
        /// The state change handler delegate
        /// </value>
        event StateChangeEventHandler StateChange;
        #endregion
        #region Fields/Properties
#if !NETSTANDARD1_3
        /// <summary>
        /// Whether or not the passed in provider is capable of creating a data source enumerator
        /// </summary>
        bool CanCreateDataSourceEnumerator { get; }
        /// <summary>
        /// Whether or not the <see cref="ConnectionString"/> is readonly
        /// </summary>
        bool ConnectionStringReadonly { get; }
        /// <summary>
        /// Whether the <see cref="ConnectionString"/> has a fixed size
        /// </summary>
        bool ConnectionStringFixedSize { get; }
#endif
        /// <summary>
        /// Gets the current number of keys that are contained within the <see cref="ConnectionString"/> property
        /// </summary>
        int ConnectionStringKeyCount { get; }
        /// <summary>
        /// The current <see cref="ConnectionState"/> of the <see cref="DbConnection"/>
        /// </summary>
        ConnectionState State { get; }
        /// <summary>
        /// The maximum amount of attempts a SQL query should attempt to execute before failing
        /// </summary>
        int RetryAttempts { get; set; }
        /// <summary>
        /// The amount of time in milliseconds a retry attempt should wait before another attempt is made
        /// </summary>
        int RetryInterval { get; set; }
        /// <summary>
        /// ConnectionString as a <see cref="string"/> to use when creating a <see cref="DbConnection"/>
        /// </summary>
        string ConnectionString { get; set; }
        /// <summary>
        /// Gets or sets the wait time in seconds before terminating the attempt to execute a command and generating an error.
        /// </summary>
        int CommandTimeout { get; set; }
        /// <summary>
        /// Gets the time to wait in seconds while trying to establish a connection before terminating the attempt and generating an error.
        /// </summary>
        int ConnectionTimeout { get; }
        /// <summary>
        /// Gets the name of the current database after a connection is opened, or the database name specified in the connection string before the connection is opened.
        /// </summary>
        string Database { get; }
        /// <summary>
        /// Gets the name of the database server to which to connect.
        /// </summary>
        string DataSource { get; }
        /// <summary>
        /// Gets a string that represents the version of the server to which the object is connected
        /// </summary>
        string ServerVersion { get; }
        #endregion
        #region Utility Methods
        /// <summary>
        /// Starts a database transaction using the underlying <see cref="DbConnection"/>
        /// </summary>
        /// <returns>Returns an instance of <see cref="DbTransaction"/></returns>
        DbTransaction GetDbTransaction();
        /// <summary>
        /// Starts a <see cref="DbTransaction"/> using the underlying <see cref="DbConnection"/> with the <paramref name="level"/>
        /// </summary>
        /// <param name="level">The <see cref="System.Data.IsolationLevel"/> to describe the locking behavior for the transaction</param>
        /// <returns>Returns an instance of <see cref="DbTransaction"/></returns>
        DbTransaction GetDbTransaction(System.Data.IsolationLevel level);
        /// <summary>
        /// Closes any open connections being used by the helper 
        /// </summary>
        void Close();
        /// <summary>
        /// Opens the connection to a database if not already opened
        /// </summary>
        void Open();
#if !NETSTANDARD1_3
        /// <summary>
        /// Enlists the passed in <paramref name="transact"/> in a distributed transaction
        /// </summary>
        /// <param name="transact">An instance of <see cref="Transaction"/> to use to enlist a distributed transaction</param>
        /// <remarks>
        /// Because it enlists a connection in a Transaction instance, EnlistTransaction takes advantage of functionality available in the System.Transactions namespace for managing distributed transactions. 
        /// Once a connection is explicitly enlisted in a transaction, it cannot be unenlisted or enlisted in another transaction until the first transaction finishes. 
        /// </remarks>
        void EnlistTransaction(Transaction transact);
        /// <summary>
        /// Returns schema information for the data source of this <see cref="DbConnection"/> using the specified string for the schema name and the specified string array for the restriction values.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="restrictionValues">The restriction values.</param>
        /// <returns>A <see cref="DataTable"/> that contains schema information</returns>
        /// <remarks>If the connection is associated with a transaction, executing GetSchema calls may cause some providers to throw an exception</remarks>
        DataTable GetSchema(string collectionName, string[] restrictionValues);
        /// <summary>
        /// Returns schema information for the data source of this <paramref name="collectionName"/> using the specified string for the schema name.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <returns>A <see cref="DataTable"/> that contains schema information</returns>
        /// <remarks>If the connection is associated with a transaction, executing GetSchema calls may cause some providers to throw an exception</remarks>
        DataTable GetSchema(string collectionName);
        /// <summary>
        /// Returns schema information for the data source of this <see cref="DbConnection"/>
        /// </summary>
        /// <returns>A <see cref="DataTable"/> that contains schema information</returns>
        /// <remarks>If the connection is associated with a transaction, executing GetSchema calls may cause some providers to throw an exception</remarks>
        DataTable GetSchema();
        /// <summary>
        /// Provides a mechanism for enumerating all available instances of database servers within the local network
        /// </summary>
        /// <returns>Returns a new instance of <see cref="DbDataSourceEnumerator"/> created by the current <see cref="DbProviderFactory"/></returns>
        DbDataSourceEnumerator GetDataSourceEnumerator();
#endif
        #endregion
    }
}
