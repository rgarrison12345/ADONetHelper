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
using System.Reflection;
using System.Threading;
#if NETSTANDARD2_0 || NETSTANDARD2_1
using System.Runtime.Loader;
#endif
#if NETSTANDARD2_1
using System.Threading.Tasks;
#endif
#endregion

namespace ADONetHelper.Core
{
    /// <summary>
    /// A class that facilitates creating the ADO.NET class objects necessary to query a data store
    /// </summary>
    /// <remarks>
    /// <see cref="DbObjectFactory"/> is a class that is intended to be used at the lowest level of the ADO.NET workflow.  
    /// It creates the objects necessary to query a relational database using the RDBMS providers own driver to do this.
    /// For the .NET framework the providers dll can be within the Global Assembly Cache, and the providers dll can also be used as a dll
    /// contained within the application
    /// </remarks>
    /// <seealso cref="IDbObjectFactory"/>
    public class DbObjectFactory : IDbObjectFactory
    {
        #region Fields/Properties
        private readonly DbProviderFactory _dbProviderFactory;

        /// <summary>
        /// The character symbol to use when binding a variable in a given providers SQL query
        /// </summary>
        public string VariableBinder { get; set; } = "";

        /// <summary>
        /// Whether or not the passed in provider is capable of creating a data source enumerator
        /// </summary>
        public bool CanCreateDataSourceEnumerator
        {
            get
            {
                //Return this back to the caller
                return this._dbProviderFactory.CanCreateDataSourceEnumerator;
            }
        }
        /// <summary>
        /// Gets or sets the database parameter mapper.
        /// </summary>
        /// <value>
        /// The database parameter mapper.
        /// </value>
        public IDbParameterMapper DbParameterMapper { get; set; }
        #endregion
        #region Constructors
        /// <summary>
        /// Instantiates a new instance with the passed in <paramref name="factory"/>
        /// </summary>
        /// <param name="factory">An instance of the <see cref="DbProviderFactory"/> client class</param>
        public DbObjectFactory(DbProviderFactory factory)
        {
            _dbProviderFactory = factory;
        }
        /// <summary>
        /// Instantiates a new instance with the passed in <paramref name="providerInvariantName"/>
        /// </summary>
        /// <param name="providerInvariantName">The name of the data provider that the should be used to query a data store</param>
        public DbObjectFactory(string providerInvariantName)
        {
#if !NETSTANDARD2_0
            try
            {
                _dbProviderFactory = DbProviderFactories.GetFactory(providerInvariantName);
            }
            catch (Exception ex)
            {
                _dbProviderFactory = GetProviderFactory(providerInvariantName);
            }
#else
            _dbProviderFactory = GetProviderFactory(providerInvariantName);
#endif
        }
        /// <summary>
        /// Instantiates a new instance with the passed in <paramref name="connection"/>
        /// </summary>
        /// <param name="connection">An instance of <see cref="DbConnection"/> </param>
        public DbObjectFactory(DbConnection connection)
        {
#if !NETSTANDARD2_0
            _dbProviderFactory = DbProviderFactories.GetFactory(connection);
#elif NETSTANDARD2_0
            //Get the assembly from the dbconnection type
            _dbProviderFactory = GetProviderFactory(connection.GetType().Assembly);
#endif
        }
#if !NETSTANDARD2_0
        /// <summary>
        /// Instantiates a new instance with the passed in <paramref name="row"/>
        /// </summary>
        /// <param name="row">An instance of <see cref="DataRow"/> that has the necessary information to create an instance of <see cref="DbProviderFactory"/></param>
        public DbObjectFactory(DataRow row)
        {
            _dbProviderFactory = DbProviderFactories.GetFactory(row);
        }
#endif
        #endregion
        #region Utility Methods
        /// <summary>
        /// Provides a mechanism for enumerating all available instances of database servers within the local network
        /// </summary>
        /// <returns>Returns a new instance of <see cref="DbDataSourceEnumerator"/> created by the current <see cref="DbProviderFactory"/></returns>
        public DbDataSourceEnumerator GetDataSourceEnumerator()
        {
            //Return this back to the caller
            return _dbProviderFactory.CreateDataSourceEnumerator();
        }
        /// <summary>
        /// Gets a <see cref="DbDataAdapter"/> based on the provider the <see cref="DbObjectFactory"/> is utilizing
        /// </summary>
        /// <returns>Returns an instance of <see cref="DbDataAdapter"/></returns>
        public DbDataAdapter GetDbDataAdapter()
        {
            //Return this back to the caller
            return _dbProviderFactory.CreateDataAdapter();
        }
        /// <summary>
        /// Gets a <see cref="DbCommandBuilder"/> based on the provider the <see cref="DbObjectFactory"/> is utilizing
        /// </summary>
        /// <returns>Returns an instance of <see cref="DbCommandBuilder"/></returns>
        public DbCommandBuilder GetDbCommandBuilder()
        {
            //Return this back to the caller
            return _dbProviderFactory.CreateCommandBuilder();
        }
        /// <summary>
        /// Gets a <see cref="DbConnectionStringBuilder"/> based off the provider passed into class using the passed in <paramref name="connectionString"/>
        /// </summary>
        /// <param name="connectionString">The connection string that will be used to when building a connection string</param>
        /// <returns>Returns a <see cref="DbConnectionStringBuilder"/> based off of target .NET framework data provider</returns>
        public DbConnectionStringBuilder GetDbConnectionStringBuilder(string connectionString)
        {
            DbConnectionStringBuilder builder = GetDbConnectionStringBuilder();

            //Don't set if empty
            if (!string.IsNullOrEmpty(connectionString) || connectionString.Trim() != string.Empty)
            {
                builder.ConnectionString = connectionString;
            }

            //Return this back to the caller
            return builder;
        }
        /// <summary>
        /// Gets a <see cref="DbConnectionStringBuilder"/> based off the provider passed into class
        /// </summary>
        /// <returns>Returns a <see cref="DbConnectionStringBuilder"/> based off of target .NET framework data provider</returns>
        public DbConnectionStringBuilder GetDbConnectionStringBuilder()
        {
            //Return this back to the caller
            return _dbProviderFactory.CreateConnectionStringBuilder();
        }
        /// <summary>
        /// Gets an instance of a formatted <see cref="DbCommand"/> object based on the specified provider
        /// </summary>
        ///<param name="transact">An instance of <see cref="DbTransaction"/></param>
        /// <param name="commandTimeout">Gets or sets the wait time in seconds before terminating the attempt to execute a command and generating an error.</param>
        /// <param name="connection">An instance of <see cref="DbConnection"/></param>
        /// <param name="parameters">The list of <see cref="IEnumerable{DbParameter}"/> associated with the query parameter</param>
        /// <param name="query">The SQL command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns an instance of <see cref="DbCommand"/> object based off the provider passed into the class</returns>
        public DbCommand GetDbCommand(CommandType queryCommandType, string query, IEnumerable<DbParameter> parameters, DbConnection connection, int commandTimeout, DbTransaction transact = null)
        {
            //Get the DbCommand object
            DbCommand dCommand = GetDbCommand(connection, transact, commandTimeout);

            //Set query and command type
            dCommand.CommandType = queryCommandType;
            dCommand.CommandText = query;

            //Check for null, don't add in null
            if (parameters != null)
            {
                //Add in all the parameters
                foreach (DbParameter param in parameters)
                {
                    dCommand.Parameters.Add(param);
                }
            }

            //Return this back to the caller
            return dCommand;
        }
        /// <summary>
        /// Gets an instance of a formatted <see cref="DbCommand"/> object based on the specified provider
        /// </summary>
        /// <param name="commandTimeout">Gets or sets the wait time in seconds before terminating the attempt to execute a command and generating an error.</param>
        /// <param name="connection">Represents a connection to a database</param>
        /// <param name="transact">An instance of a <see cref="DbTransaction"/> object</param>
        /// <returns>Returns an instance of <see cref="DbCommand"/> object based off the provider passed into the class</returns>
        public DbCommand GetDbCommand(DbConnection connection, DbTransaction transact, int commandTimeout)
        {
            //Get the DbCommand object
            DbCommand dCommand = GetDbCommand(commandTimeout);

            //Set query and command type
            dCommand.Connection = connection;
            dCommand.Transaction = transact;

            //Return this back to the caller
            return dCommand;
        }
        /// <summary>
        /// Gets an instance of <see cref="DbCommand"/> object
        /// </summary>
        /// <param name="commandTimeout">Gets or sets the wait time in seconds before terminating the attempt to execute a command and generating an error.</param>
        /// <returns>Returns an instance of <see cref="DbCommand"/> object</returns>
        public DbCommand GetDbCommand(int commandTimeout)
        {
            DbCommand command = GetDbCommand();
            command.CommandTimeout = commandTimeout;

            //Return this back to the caller
            return command;
        }
        /// <summary>
        /// Gets an instance of <see cref="DbCommand"/> object
        /// </summary>
        /// <returns>Returns an instance of <see cref="DbCommand"/> object</returns>
        public DbCommand GetDbCommand()
        {
            DbCommand command = _dbProviderFactory.CreateCommand();

            //Dispose interfaces to clear up any resources used by this instance
            command.Disposed += DbCommand_Disposed;

            //Return this back to the caller
            return command;
        }
        /// <summary>
        /// Instantiates a new instance of the <see cref="DbConnection"/> object based on the specified provider
        /// </summary>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <returns>Returns a new instance of the <see cref="DbConnection"/> object based on the specified provider</returns>
        public DbConnection GetDbConnection(string connectionString)
        {
            //Get the DbConnection object
            DbConnection db = GetDbConnection();

            //Set the connection string
            db.ConnectionString = connectionString;

            //Return this back to the caller
            return db;
        }
        /// <summary>
        /// Instantiates a new instance of the <see cref="DbConnection"/> object based on the specified provider
        /// </summary>
        /// <returns>Returns a new instance of the <see cref="DbConnection"/> object based on the specified provider</returns>
        public DbConnection GetDbConnection()
        {
            //Return this back to the caller
            return _dbProviderFactory.CreateConnection();
        }
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
        public DbParameter GetFixedSizeDbParameter(string parameterName, object parameterValue, DbType dataType, byte? scale = null, byte? precision = null, ParameterDirection paramDirection = ParameterDirection.Input)
        {
            //Get the DbParameter object
            DbParameter parameter = GetDbParameter(parameterName, parameterValue, dataType, paramDirection);

            //Check for values
            if (precision.HasValue == true)
            {
                parameter.Precision = precision.Value;
            }
            if (scale.HasValue == true)
            {
                parameter.Scale = scale.Value;
            }

            //Return this back to the caller
            return parameter;
        }
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/> object based on the specified provider
        /// </summary>
        /// <exception cref="ArgumentNullException">Throws when the <paramref name="paramDirection"/> is <see cref="ParameterDirection.Output"/> and the <paramref name="size"/> is <c>null</c></exception>
        /// <param name="dataType">The <see cref="DbType"/> of the field in the database</param>
        /// <param name="size">maximum size, in bytes, of the data.  The default value is <c>null</c></param>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter as a <see cref="object"/></param>
        /// <param name="paramDirection">The direction of the parameter, defaults to <see cref="ParameterDirection.Input"/></param>
        /// <returns>Returns an instance of <see cref="DbParameter"/> object with information passed into procedure</returns>
        public DbParameter GetVariableSizeDbParameter(string parameterName, object parameterValue, DbType dataType, int? size = null, ParameterDirection paramDirection = ParameterDirection.Input)
        {
            //Get the DbParameter object
            DbParameter parameter = GetDbParameter(parameterName, parameterValue, dataType, paramDirection);

            //Check for value
            if (size.HasValue == true)
            {
                parameter.Size = size.Value;
            }

            //Check if this parameter is a database null value
            if (parameter.IsNullable == false && parameter.Size <= 0)
            {
                //Check the parameter direction
                if (parameter.Direction == ParameterDirection.Output)
                {
                    //Let implementors now that size must be set for output parameters
                    throw new ArgumentNullException("Parameter size must be set for variable sized data type output parameters");
                }
                else
                {
                    //Check if this is a string or byte array, we need to set the size of the parameter explicitly
                    if (parameter.Value is string)
                    {
                        parameter.Size = parameter.Value.ToString().Length;
                    }
                    else if (parameter.Value is byte[])
                    {
                        parameter.Size = ((byte[])parameter.Value).Length;
                    }
                }
            }

            //Return this back to the caller
            return parameter;
        }
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/> object based on the specified provider
        /// </summary>
        /// <param name="dataType">The <see cref="DbType"/> of the field in the database</param>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter as a <see cref="object"/></param>
        /// <param name="paramDirection">The direction of the parameter, defaults to <see cref="ParameterDirection.Input"/></param>
        /// <returns>Returns an instance of <see cref="DbParameter"/> object with information passed into procedure</returns>
        public DbParameter GetDbParameter(string parameterName, object parameterValue, DbType dataType, ParameterDirection paramDirection)
        {
            //Get the DbParameter object
            DbParameter parameter = GetDbParameter(parameterName, parameterValue);

            //Set parameter properties
            parameter.DbType = dataType;
            parameter.Direction = paramDirection;

            //Return this back to the caller
            return parameter;
        }
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/> object based on the specified provider
        /// </summary>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter</param>
        /// <returns>Returns an instance of <see cref="DbParameter"/> object with information passed into procedure</returns>
        public DbParameter GetDbParameter(string parameterName, object parameterValue)
        {
            //Get the DbParameter object
            DbParameter parameter = GetDbParameter();

            parameter.Value = parameterValue ?? DBNull.Value;

            //Check for null reference
            if (parameter.Value != DBNull.Value)
            {
                string parameterString = parameter.Value.ToString();

                //Check if this is a date time value
                if (parameterString == DateTime.MinValue.ToString() || parameterString == DateTime.MaxValue.ToString())
                {
                    //SQL Server cannot handle these values
                    parameter.Value = DBNull.Value;
                }
            }

            //Check for null or empty
            if (!string.IsNullOrEmpty(VariableBinder) || VariableBinder.Trim() != string.Empty)
            {
                parameter.ParameterName = VariableBinder + parameterName.Replace(VariableBinder, "");
            }
            else
            {
                parameter.ParameterName = parameterName;
            }

            //Check db parameter has been set
            if(DbParameterMapper != null)
            {
                //Now get the RDBMS mapped data type to the .net data type
                parameter.DbType = DbParameterMapper.GetDbType(parameter.Value);
            }

            //Check if this is nullable
            parameter.IsNullable = (parameter.Value == DBNull.Value);

            //Return this back to the caller
            return parameter;
        }
        /// <summary>
        /// Create an instance of <see cref="DbParameter"/> object based off of the provider passed into factory
        /// </summary>
        /// <returns>Returns an instantiated <see cref="DbParameter"/> object</returns>
        public DbParameter GetDbParameter()
        {
            //Return this back to the caller
            return _dbProviderFactory.CreateParameter();
        }
        /// <summary>
        /// Gets an instace of the <see cref="DbTransaction"/> object based on the <see cref="DbConnection"/> object passed in
        /// </summary>
        /// <param name="connection">An instance of <see cref="DbConnection"/></param>
        /// <returns>An instance of the <see cref="DbTransaction"/> object</returns>
        public DbTransaction GetDbTransaction(DbConnection connection)
        {
            //Return this back to the caller
            return connection.BeginTransaction();
        }
        /// <summary>
        /// Gets an instace of the <see cref="DbTransaction"/> object based on the <see cref="DbConnection"/> object passed in
        /// </summary>
        /// <param name="level">The transaction locking level for the passed in <paramref name="connection"/></param>
        /// <param name="connection">An instance of <see cref="DbConnection"/></param>
        /// <returns>An instance of the <see cref="DbTransaction"/> object</returns>
        public DbTransaction GetDbTransaction(DbConnection connection, IsolationLevel level)
        {
            //Return this back to the caller
            return connection.BeginTransaction(level);
        }
#if NETSTANDARD2_1
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async ValueTask<DbTransaction> GetDbTransactionAsync(DbConnection connection, CancellationToken token = default)
        {
            return await connection.BeginTransactionAsync(token);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="level"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async ValueTask<DbTransaction> GetDbTransactionAsync(DbConnection connection, IsolationLevel level, CancellationToken token = default)
        {
            return await connection.BeginTransactionAsync(level, token);
        }
#endif
        #endregion
        #region Helper Methods        
        /// <summary>
        /// Databases the command disposed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        public void DbCommand_Disposed(object sender, EventArgs e)
        {
            ((DbCommand)sender).Parameters.Clear();
        }
#if NETSTANDARD2_0 || NETSTANDARD2_1
        /// <summary>
        /// Gets an instance of <see cref="DbProviderFactory"/> based off a .NET drivers <paramref name="providerName"/>, such as System.Data.SqlClient.
        /// Looks for the <paramref name="providerName"/> within the current <see cref="AssemblyLoadContext"/>
        /// </summary>
        /// <returns>Returns an instance of <see cref="DbProviderFactory"/></returns>
        public static DbProviderFactory GetProviderFactory(string providerName)
        {
            //Get the assembly
            return GetProviderFactory(AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(providerName)));
        }
#else
        /// <summary>
        /// Gets an instance of <see cref="DbProviderFactory"/> based off a .NET drivers <paramref name="providerName"/>, such as System.Data.SqlClientt
        /// </summary>
        /// <returns>Returns an instance of <see cref="DbProviderFactory"/></returns>
        public static DbProviderFactory GetProviderFactory(string providerName)
        {
            //Get the assembly
            return GetProviderFactory(Assembly.Load(new AssemblyName(providerName)));
        }
#endif
        /// <summary>
        /// Gets an instance of <see cref="DbProviderFactory"/> based off a .NET driver <see cref="Assembly"/>
        /// Looks for the <see cref="DbProviderFactory"/> within the current <see cref="Assembly"/>
        /// </summary>
        /// <returns>Returns an instance of <see cref="DbProviderFactory"/></returns>
        public static DbProviderFactory GetProviderFactory(Assembly assembly)
        {
            Type providerFactory = null;

            //Get the type that inherits from DbProviderFactory
            foreach (Type t in assembly.GetTypes())
            {

                //Check if this is a dbproviderfactory
                if (t.GetTypeInfo().BaseType == typeof(DbProviderFactory))
                {
                    providerFactory = t;
                    break;
                }
            }

            //Get the field to get the factory instance
            FieldInfo field = providerFactory.GetField("Instance");

            //Get the provider factory
            return (DbProviderFactory)field.GetValue(null);
        }
        #endregion
    }
}