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
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
#if !NET20 && !NET35 && !NET40
using System.Threading.Tasks;
#endif
#endregion

namespace ADONetHelper.Core
{
    /// <summary>
    /// The contract class for a factory class that creates database objects in order to query a database
    /// </summary>
    public interface IDbObjectFactory
    {
        #region Fields/Properties
        /// <summary>
        /// The character symbol to use when binding a variable in a given providers SQL query
        /// </summary>
        string VariableBinder { get; set; }
#if !NETSTANDARD1_3
        /// <summary>
        /// Whether or not the passed in provider is capable of creating a data source enumerator
        /// </summary>
        bool CanCreateDataSourceEnumerator { get; }
#endif
        #endregion
        #region Utility Methods
#if !NETSTANDARD1_3
        /// <summary>
        /// Provides a mechanism for enumerating all available instances of database servers within the local network
        /// </summary>
        /// <returns>Returns a new instance of <see cref="DbDataSourceEnumerator"/> created by the current <see cref="DbProviderFactory"/></returns>
        DbDataSourceEnumerator GetDataSourceEnumerator();
        /// <summary>
        /// Gets a <see cref="DbCommandBuilder"/> based on the provider the <see cref="DbObjectFactory"/> is utilizing
        /// </summary>
        /// <returns>Returns an instance of <see cref="DbCommandBuilder"/></returns>
        DbCommandBuilder GetDbCommandBuilder();
        /// <summary>
        /// Gets a <see cref="DbDataAdapter"/> based on the provider the <see cref="DbObjectFactory"/> is utilizing
        /// </summary>
        /// <returns>Returns an instance of <see cref="DbDataAdapter"/></returns>
        DbDataAdapter GetDbDataAdapter();
#endif
        /// <summary>
        /// Gets a <see cref="DbConnectionStringBuilder"/> based off the provider passed into class
        /// </summary>
        /// <param name="connectionString">The connection string that will be used to when building a connection string</param>
        /// <returns>Returns a <see cref="DbConnectionStringBuilder"/> based off of target .NET framework data provider</returns>
        DbConnectionStringBuilder GetDbConnectionStringBuilder(string connectionString);
        /// <summary>
        /// Gets a <see cref="DbConnectionStringBuilder"/> based off the provider passed into class
        /// </summary>
        /// <returns>Returns a <see cref="DbConnectionStringBuilder"/> based off of target .NET framework data provider</returns>
        DbConnectionStringBuilder GetDbConnectionStringBuilder();
        /// <summary>
        /// Gets an instance of a <see cref="DbCommand"/> object
        /// </summary>
        /// <returns>Returns an instance of a <see cref="DbCommand"/> Object</returns>
        DbCommand GetDbCommand();
        /// <summary>
        /// Gets an instance of a <see cref="DbCommand"/> object
        /// </summary>
        /// <param name="commandTimeout">Gets or sets the wait time in seconds before terminating the attempt to execute a command and generating an error.</param>
        /// <returns>Returns an instance of <see cref="DbCommand"/> Object</returns>
        DbCommand GetDbCommand(int commandTimeout);
        /// <summary>
        /// Gets an instance of a <see cref="DbCommand"/> subclass based on the specified provider
        /// </summary>
        /// <param name="commandTimeout">Gets or sets the wait time in seconds before terminating the attempt to execute a command and generating an error.</param>
        /// <param name="connection">Represents a <see cref="DbConnection"/> to a database</param>
        /// <param name="transact">An instance of a <see cref="DbTransaction"/> class</param>
        /// <returns>Returns an instantiated formatted <see cref="DbCommand"/> object based off the provider passed into the class</returns>
        DbCommand GetDbCommand(DbConnection connection, DbTransaction transact, int commandTimeout);
        /// <summary>
        /// Instantiates a new isntance of the <see cref="DbCommand"/> subclass based on the provider passed into the class constructor
        /// </summary>
        /// <param name="commandTimeout">Gets or sets the wait time in seconds before terminating the attempt to execute a command and generating an error.</param>
        /// <param name="connection">Represents a connection to a database</param>
        /// <param name="transact">An instance of <see cref="DbTransaction"/></param>
        /// <param name="parameters">The <see cref="IEnumerable{DbParameter}"/> of parameters associated with the query parameter</param>
        /// <param name="query">The SQL command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns an instantiated formatted <see cref="DbCommand"/> object based off the provider passed into the class</returns>
        DbCommand GetDbCommand(CommandType queryCommandType, string query, IEnumerable<DbParameter> parameters, DbConnection connection, int commandTimeout, DbTransaction transact = null);
        /// <summary>
        /// Instantiates a new instance of a <see cref="DbTransaction"/> subclass based on the provider passed into class constructor
        /// </summary>
        /// <param name="connection">An instance of the <see cref="DbConnection"/> class</param>
        /// <returns>An instance of the DbTransaction class</returns>
        DbTransaction GetDbTransaction(DbConnection connection);
        /// <summary>
        /// Gets an instace of the <see cref="DbTransaction"/> object based on the <see cref="DbConnection"/> object passed in
        /// </summary>
        /// <param name="level">The transaction locking level for the passed in <paramref name="connection"/></param>
        /// <param name="connection">An instance of <see cref="DbConnection"/></param>
        /// <returns>An instance of the <see cref="DbTransaction"/> object</returns>
        DbTransaction GetDbTransaction(DbConnection connection, IsolationLevel level);
#if NETSTANDARD2_1
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        ValueTask<DbTransaction> GetDbTransactionAsync(DbConnection connection, CancellationToken token = default);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="level"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        ValueTask<DbTransaction> GetDbTransactionAsync(DbConnection connection, IsolationLevel level, CancellationToken token = default);
#endif
        /// <summary>
        /// Instantiates a new instance of a <see cref="DbConnection"/> subclass based on the specified provider
        /// </summary>
        /// <returns>Returns a new instance of the <see cref="DbConnection"/> subclass based on the specified provider</returns>
        DbConnection GetDbConnection();
        /// <summary>
        /// Instantiates a new instance of a <see cref="DbConnection"/> subclass based on the provider passed into the class constructor
        /// </summary>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <returns>Returns a new instance of the <see cref="DbConnection"/> subclass based on the specified provider</returns>
        DbConnection GetDbConnection(string connectionString);
        /// <summary>
        /// Create an instance of a <see cref="DbParameter"/> based off of the provider passed into factory
        /// </summary>
        /// <returns>Returns an instantiated <see cref="DbParameter"/> object</returns>
        DbParameter GetDbParameter();
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/> subclass based on the specified provider
        /// </summary>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter</param>
        /// <returns>Returns an instance of <see cref="DbParameter"/> type with information passed into procedure</returns>
        DbParameter GetDbParameter(string parameterName, object parameterValue);
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/> object based on the specified provider
        /// </summary>
        /// <param name="dataType">The <see cref="DbType"/> of the field in the database</param>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter as a <see cref="object"/></param>
        /// <param name="paramDirection">The direction of the parameter, defaults to <see cref="ParameterDirection.Input"/></param>
        /// <returns>Returns an instance of <see cref="DbParameter"/> object with information passed into procedure</returns>
        DbParameter GetDbParameter(string parameterName, object parameterValue, DbType dataType, ParameterDirection paramDirection);
#if !NET20 && !NET35 && !NET40 && !NET45
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/> object based on the specified provider
        /// </summary>
        /// <param name="dataType">The <see cref="DbType"/> of the field in the database</param>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter as a <see cref="object"/></param>
        /// <param name="scale">The number of decimal places to which the <see cref="DbParameter.Value"/> property is resolved.  The default value is <c>null</c></param>
        /// <param name="precision">The maximum number of digits used to represent the <see cref="DbParameter.Value"/> property.  The default value is <c>null</c></param>
        /// <param name="paramDirection">The direction of the parameter, defaults to <see cref="ParameterDirection.Input"/></param>
        /// <returns>Returns an instance of <see cref="DbParameter"/> object with information passed into procedure</returns>
        DbParameter GetFixedSizeDbParameter(string parameterName, object parameterValue, DbType dataType, byte? scale = null, byte? precision = null, ParameterDirection paramDirection = ParameterDirection.Input);
#endif
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/> object based on the specified provider
        /// </summary>
        /// <param name="dataType">The <see cref="DbType"/> of the field in the database</param>
        /// <param name="size">maximum size, in bytes, of the data.  Should not be set for numeric types.  The default value is <c>null</c></param>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter as a <see cref="object"/></param>
        /// <param name="paramDirection">The direction of the parameter, defaults to <see cref="ParameterDirection.Input"/></param>
        /// <returns>Returns an instance of <see cref="DbParameter"/> object with information passed into procedure</returns>
        DbParameter GetVariableSizeDbParameter(string parameterName, object parameterValue, DbType dataType, int? size = null, ParameterDirection paramDirection = ParameterDirection.Input);
        #endregion
    }
}