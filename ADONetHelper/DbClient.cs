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
#endregion

namespace ADONetHelper
{
    /// <summary>
    /// Represents the base class for all <see cref="DbClient"/> classes
    /// </summary>
    /// <seealso cref="IDbClient"/>
    /// <remarks>
    /// DbClient is a utility class that encompasses both a <see cref="DbConnection"/> and a <see cref="DbCommand"/>
    /// to help query a database with minimal coding to focus on the users SQL
    /// </remarks>
    public partial class DbClient : IDbClient
    {
        #region Events        
        /// <summary>
        /// Sets the state change event handler.  This event occurs when the <see cref="DbConnection.State"/> changes
        /// </summary>
        /// <value>
        /// The state change handler delegate
        /// </value>
        public event StateChangeEventHandler StateChange
        {
            add
            {
                //Get an exclusive lock first
                lock (this.ExecuteSQL.Connection)
                {
                    this.ExecuteSQL.Connection.StateChange += value;
                }
            }
            remove
            {
                //Get an exclusive lock first
                lock (this.ExecuteSQL.Connection)
                {
                    this.ExecuteSQL.Connection.StateChange -= value;
                }
            }
        }
        #endregion
        #region Fields/Properties        
        /// <summary>
        /// The execute SQL
        /// </summary>
        private readonly ISqlExecutor _executeSQL;

#if !NETSTANDARD1_3
        /// <summary>
        /// Whether or not the passed in provider is capable of creating a data source enumerator
        /// </summary>
        public bool CanCreateDataSourceEnumerator
        {
            get
            {
                //Return this back to the caller
                return this.ExecuteSQL.Factory.CanCreateDataSourceEnumerator;
            }
        }
        /// <summary>
        /// Whether or not the <see cref="ConnectionString"/> is readonly
        /// </summary>
        public bool ConnectionStringReadonly
        {
            get
            {
                return this.ExecuteSQL.ConnectionStringBuilder.IsReadOnly;
            }
        }
        /// <summary>
        /// Whether the <see cref="ConnectionString"/> has a fixed size
        /// </summary>
        public bool ConnectionStringFixedSize
        {
            get
            {
                return this.ExecuteSQL.ConnectionStringBuilder.IsFixedSize;
            }
        }
#endif
        /// <summary>
        /// Represents an instance of the <see cref="ISqlExecutor"/> class to facilitate querying a data store
        /// </summary>
        protected ISqlExecutor ExecuteSQL
        {
            get
            {
                //Set the command and connection timeout
                _executeSQL.CommandTimeout = this.CommandTimeout;

                //Return this back to the caller
                return _executeSQL;
            }
        }
        /// <summary>
        /// Gets a string that represents the version of the server to which the object is connected
        /// </summary>
        public string ServerVersion
        {
            get
            {
                //Return this back to the caller
                return this.ExecuteSQL.Connection.ServerVersion;
            }
        }
        /// <summary>
        /// The current <see cref="ConnectionState"/> of the <see cref="DbConnection"/>
        /// </summary>
        public ConnectionState State
        {
            get
            {
                //Return this back to the caller
                return this.ExecuteSQL.Connection.State;
            }
        }
        /// <summary>
        /// Gets the name of the current database after a connection is opened, or the database name specified in the connection string before the connection is opened.
        /// </summary>
        public virtual string Database
        {
            get
            {
                //Return this back to the caller
                return this.ExecuteSQL.Connection.Database;
            }
        }
        /// <summary>
        /// Gets the name of the database server to which to connect.
        /// </summary>
        public string DataSource
        {
            get
            {
                //Return this back to the caller
                return this.ExecuteSQL.Connection.DataSource;
            }
        }
        /// <summary>
        /// ConnectionString as a <see cref="string"/> to use when creating a <see cref="DbConnection"/>
        /// </summary>
        public string ConnectionString
        {
            get
            {
                //Return this back to the caller
                return this.ExecuteSQL.ConnectionStringBuilder.ConnectionString;
            }
            set
            {
                //Set the connection string
                this.ExecuteSQL.ConnectionStringBuilder.ConnectionString = value;
            }
        }
        /// <summary>
        /// The character symbol to use when binding a variable in a given providers SQL query
        /// </summary>
        public string VariableBinder
        {
            get
            {
                //Return this back to the caller
                return _executeSQL.VariableBinder;
            }
            set
            {
                //Set for this layer as well
                _executeSQL.VariableBinder = value;
            }
        }
        /// <summary>
        /// Represents how a command should be interpreted by the data provider.  Default value is <see cref="CommandType.Text"/>
        /// </summary>
        public CommandType QueryCommandType { get; set; } = CommandType.Text;
        /// <summary>
        /// The maximum amount of attempts a SQL query should attempt to execute before failing.  Only uses values greater than zero
        /// </summary>
        public int RetryAttempts { get; set; } = 0;
        /// <summary>
        /// The amount of time in seconds a retry attempt should wait before another attempt is made.  Only uses values greater than zero
        /// </summary>
        public int RetryInterval { get; set; } = 0;
        /// <summary>
        /// Gets or sets the wait time in seconds before terminating the attempt to execute a command and generating an error.  Default value is 30
        /// </summary>
        public int CommandTimeout { get; set; } = 30;
        /// <summary>
        /// Gets the time to wait in seconds while trying to establish a connection before terminating the attempt and generating an error.
        /// </summary>
        public int ConnectionTimeout
        {
            get
            {
                //Return this back to the caller
                return this.ExecuteSQL.Connection.ConnectionTimeout;
            }
        }
        #endregion
        #region Variables
        /// <summary>
        /// Variable To detect redundant calls of dispose
        /// </summary>
        protected bool disposedValue = false;
        private int attemptsMade = 0;
        #endregion
        #region Constructors
        /// <summary>
        /// Instantiates a new instance of <see cref="DbClient"/> with an instance of <see cref="ISqlExecutor"/>
        /// </summary>
        /// <param name="executor">An instance of <see cref="ISqlExecutor"/></param>
        public DbClient(ISqlExecutor executor)
        {
            //Set fields
            _executeSQL = executor;
        }
        /// <summary>
        /// Instantiates a new instance of <see cref="DbClient"/> with the passed in <paramref name="connectionString"/>, and <paramref name="queryCommandType"/>, and <paramref name="factory"/>
        /// </summary>
        /// <param name="factory">An instance of a <see cref="DbProviderFactory"/> client class</param>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        public DbClient(string connectionString, CommandType queryCommandType, DbProviderFactory factory) : this(connectionString, string.Empty)
        {
            //Set fields/properties
            _executeSQL = new SqlExecutor(factory);
            this.QueryCommandType = queryCommandType;
        }
        /// <summary>
        /// Instantiates a new instance of <see cref="DbClient"/> with the passed in <paramref name="connectionString"/> and <paramref name="factory"/>
        /// </summary>
        /// <param name="factory">An instance of the <see cref="DbProviderFactory"/> client class</param>
        /// <param name="connectionString">The connection string used to query a data store</param>
        public DbClient(string connectionString, DbProviderFactory factory) : this(connectionString, string.Empty)
        {
            //Set fields/properties
            _executeSQL = new SqlExecutor(factory);
            this.ConnectionString = connectionString;
        }
        /// <summary>
        /// Instantiates a new instance of <see cref="DbClient"/> with the passed in with the passed in <paramref name="connectionString"/>, <paramref name="providerName"/>, and <paramref name="queryCommandType"/>
        /// </summary>
        /// <param name="providerName">The name of the data provider that the should be used to query a data store</param>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        public DbClient(string connectionString, string providerName, CommandType queryCommandType) : this(connectionString, providerName)
        {
            //Set properties
            this.QueryCommandType = queryCommandType;
        }
        /// <summary>
        /// Instantiates a new instance of <see cref="DbClient"/> with the passed in <paramref name="connectionString"/> and <paramref name="providerName"/>
        /// </summary>
        /// <param name="providerName">The name of the data provider that the should be used to query a data store</param>
        /// <param name="connectionString">The connection string used to query a data store</param>
        public DbClient(string connectionString, string providerName) : this(providerName)
        {
            //Set properties
            this.ConnectionString = connectionString;
        }                                                           
        /// <summary>
        /// Instantiates a new instance of <see cref="DbClient"/> with the passed in <paramref name="providerName"/>
        /// </summary>
        /// <param name="providerName">The name of the data provider that the should be used to query a data store</param>
        public DbClient(string providerName)
        {
            //Set fields
            _executeSQL = new SqlExecutor(providerName);
        }
        /// <summary>
        /// Instantiates a new instance of <see cref="DbClient"/> using an existing <see cref="DbConnection"/> to initialize the <paramref name="connection"/>
        /// </summary>
        /// <param name="connection">An instance of <see cref="DbConnection"/> to use to query a database </param>
        public DbClient(DbConnection connection)
        {
            //Set properties
            _executeSQL = new SqlExecutor(connection);
        }
        /// <summary>
        /// Instantiates a new instance of <see cref="DbClient"/> using the passed in <paramref name="connectionString"/> and <paramref name="factory"/>
        /// </summary>
        /// <param name="connectionString">The connection string used to query a database</param>
        /// <param name="factory">An instance of <see cref="IDbObjectFactory"/> to create the objects needed to help query a database</param>
        public DbClient(string connectionString, IDbObjectFactory factory) : this(connectionString, string.Empty)
        {
            //Set properties
            _executeSQL = new SqlExecutor(factory);
        }
        #endregion
        #region Destructor
        /// <summary>
        /// Finalizer for the class to release unmanaged resources
        /// </summary>
        ~DbClient()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }
        #endregion
        #region Utility Methods
        #region ConnectionString methods
        /// <summary>
        /// Adds a property name and value to the current connection string
        /// </summary>
        /// <param name="name">The name of the connection string property</param>
        /// <param name="value">The value to use with the connection string property</param>
        public void AddConnectionStringProperty(string name, object value)
        {
            //Add this property name and value
            this.ExecuteSQL.ConnectionStringBuilder.Add(name, value);
        }
        /// <summary>
        /// Removes a connection string property from the connection string by name
        /// </summary>
        /// <param name="name">The name of the connection string property</param>
        public void RemoveConnectionStringProperty(string name)
        {
            //Remove this property
            this.ExecuteSQL.ConnectionStringBuilder.Remove(name);
        }
        /// <summary>
        /// Retrieves a connection string property value as an object
        /// </summary>
        /// <param name="name">The name of the connection string property</param>
        /// <returns>Returns a connection string property as an <see cref="object"/>, null if property is not present</returns>
        public object GetConnectionStringPropertyValue(string name)
        {
            //Remove this property
            if (this.ExecuteSQL.ConnectionStringBuilder.TryGetValue(name, out object value) == true)
            {
                return value;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Clears the content of the connection string
        /// </summary>
        public void ClearConnectionString()
        {
            this.ExecuteSQL.ConnectionStringBuilder.Clear();
        }
        /// <summary>
        /// Checks if the current <see cref="ConnectionString"/> in use contains the passed in <paramref name="keyword"/>
        /// </summary>
        /// <param name="keyword">The keyword to verify is in the current <see cref="ConnectionString"/></param>
        /// <returns>Returns a <see cref="bool"/> indicating if the <see cref="ConnectionString"/> contains the passed in <paramref name="keyword"/></returns>
        public bool ConnectionStringContainsKey(string keyword)
        {
            //Return this back to the caller
            return this.ExecuteSQL.ConnectionStringBuilder.ContainsKey(keyword);
        }
        /// <summary>
        /// Configures the connection string with the key value pairs passed into the routine
        /// This will clear the current connection string to start over
        /// </summary>
        /// <param name="properties">Key value pairs of connection string property names and values</param>
        public void ConfigureConnectionString(IDictionary<string, object> properties)
        {
            //Clear any existing properties
            this.ExecuteSQL.ConnectionStringBuilder.Clear();

            //Loop through properties and build this out
            foreach (KeyValuePair<string, object> kvp in properties)
            {
                //Add this into the builder
                this.ExecuteSQL.ConnectionStringBuilder.Add(kvp.Key, kvp.Value);
            }
        }
        #endregion
        #region Parameter Methods
        /// <summary>
        /// Removes a <see cref="DbParameter"/> from the parameters collection for the current <see cref="DbConnection"/> by using the parameter name
        /// </summary>
        /// <param name="parameterName">The name of the parameter to identify the parameter to remove from the collection</param>
        /// <returns>Returns true if item was successully removed, false otherwise if item was not found in the list</returns>
        public bool RemoveParameter(string parameterName)
        {
            //Return this back to the caller
            return this.ExecuteSQL.Parameters.Remove(this.ExecuteSQL.Parameters.Find(x => x.ParameterName == this.VariableBinder + parameterName));
        }
        /// <summary>
        /// Removes a <see cref="DbParameter"/> from the parameters collection for the current <see cref="DbConnection"/> by using the index of the parameter
        /// </summary>
        /// <param name="index">The index of the parameter in the parameters collection to identify the parameter to remove from the collection</param>
        /// <returns>Returns true if item was successully removed, false otherwise if item was not found in the list</returns>
        public bool RemoveParameter(int index)
        {
            //Return this back to the caller
            return this.ExecuteSQL.Parameters.Remove(this.ExecuteSQL.Parameters[index]);
        }
        /// <summary>
        /// Clears all parameters from the parameters collection
        /// </summary>
        public void ClearParameters()
        {
            //Clear any parameters
            if (this.ExecuteSQL.Parameters != null)
            {
                this.ExecuteSQL.Parameters.Clear();
                this.ExecuteSQL.Parameters = null;
            }
        }
        /// <summary>
        /// Retrieves a <see cref="DbParameter"/> object by using the passed in parameter name
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the passed in parameter name is not present in the parameters collection</exception>
        /// <param name="parameterName">The name of the parameter to use to find the parameter value</param>
        /// <returns>The specified <see cref="DbParameter"/> object from the parameters collection</returns>
        public DbParameter GetParameter(string parameterName)
        {
            //Check for null or empty
            if (string.IsNullOrEmpty(parameterName) || parameterName.Trim() == string.Empty)
            {
                throw new ArgumentException(nameof(parameterName) + "is null or empty");
            }

            //Loop through all parameters
            foreach (DbParameter param in this.ExecuteSQL.Parameters)
            {
                //Check if the name is the same
                if (param.ParameterName.ToUpper() == this.VariableBinder + parameterName.ToUpper())
                {
                    //Return this back to the caller
                    return param;
                }
            }

            //Return this back to the caller
            return null;
        }
        /// <summary>
        /// Retrieves a <see cref="DbParameter"/> from the parameters collection by using the index of the parameter
        /// </summary>
        /// <param name="index">The index of the parameter in the parameters collection to identify the parameter to retrieve from the collection</param>
        /// <returns>Returns the DbParameter object located at this index</returns>
        public DbParameter GetParameter(int index)
        {
            //Return this back to the caller
            return this.ExecuteSQL.Parameters[index];
        }
        /// <summary>
        /// Replaces an existing parameter with the new <see cref="DbParameter"/> with an existing <see cref="DbParameter.ParameterName"/>
        /// </summary>
        /// <param name="parameterName">The index as a <c>string</c> to use when searching for the existing parameter</param>
        /// <param name="param">A new instance of <see cref="DbParameter"/></param>
        public void ReplaceParameter(string parameterName, DbParameter param)
        {
            //Get index of this parameter
            int index = this.ExecuteSQL.Parameters.FindIndex(i => i.ParameterName.ToUpper() == (this.VariableBinder + parameterName.ToUpper()));

            //Do a replace of the parameter
            this.ExecuteSQL.Parameters[index] = param;
        }
        /// <summary>
        /// Replaces an existing parameter with the new <see cref="DbParameter"/> passed in at the <paramref name="index"/>
        /// </summary>
        /// <param name="index">The index as an <see cref="int"/> to use when searching for the existing parameter</param>
        /// <param name="param">A new instance of <see cref="DbParameter"/></param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public void ReplaceParameter(int index, DbParameter param)
        {
            //Do a replace of the parameter
            this.ExecuteSQL.Parameters[index] = param;
        }
        /// <summary>
        /// Sets the value of an existing <see cref="DbParameter"/> by using the <paramref name="parameterName"/> and passed in <paramref name="value"/>
        /// </summary>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="value">The value of the parameter as an <see cref="object"/></param>
        public void SetParamaterValue(string parameterName, object value)
        {
            //Get index of this parameter
            int index = this.ExecuteSQL.Parameters.FindIndex(i => i.ParameterName.ToUpper() == (this.VariableBinder + parameterName.ToUpper()));

            //Change the value
            this.SetParamaterValue(index, value);
        }
        /// <summary>
        /// Sets the value of an existing <see cref="DbParameter"/> by using the <paramref name="index"/> and passed in <paramref name="value"/>
        /// </summary>
        /// <param name="index">The index of the parameter in the parameters collection to identify the parameter to retrieve from the collection</param>
        /// <param name="value">The value of the parameter as an <see cref="object"/></param>
        public void SetParamaterValue(int index, object value)
        {
            DbParameter param = this.GetParameter(index);

            //Now set the value
            param.Value = value;

            //Change the value
            this.ExecuteSQL.Parameters[index] = param;
        }
        /// <summary>
        /// Adds a new <see cref="DbParameter"/> to the parameters collection for the current <see cref="DbConnection"/>
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the passed in parameter name is already present in the parameters collection</exception>
        /// <param name="type">The data type of the parameter being sent to the data store with the query</param>
        /// <param name="size">The maximum size, in bytes, of the data being sent to the datastore.  If parameter is a variable length don't set for input parameters</param>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter</param>
        /// <param name="paramDirection">The direction of the parameter, defaults to input.  The size must be set for output parameters</param>
        public DbParameter AddParameter(string parameterName, object parameterValue, DbType type, int? size = null, ParameterDirection paramDirection = ParameterDirection.Input)
        {
            //Check if this parameter exists before adding to collection
            if (this.Contains(parameterName) == true)
            {
                throw new ArgumentException($"Parameter with name {parameterName} already exists", nameof(parameterName));
            }
            else
            {
                //Add this parameter to the collection
                this.ExecuteSQL.Parameters.Add(this.ExecuteSQL.Factory.GetDbParameter(parameterName, parameterValue, type, size, paramDirection));

                //Return this back to the caller
                return this.GetParameter(parameterName);
            }
        }
        /// <summary>
        /// Adds a new <see cref="DbParameter"/> to the parameters collection for the current <see cref="DbConnection"/>
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the passed in parameter name is already present in the parameters collection</exception>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter</param>
        public DbParameter AddParameter(string parameterName, object parameterValue)
        {
            //Check if this parameter exists before adding to collection
            if (this.Contains(parameterName) == true)
            {
                throw new ArgumentException($"Parameter with name {parameterName} already exists", nameof(parameterName));
            }
            else
            {
                //Add this parameter to the collection
                this.ExecuteSQL.Parameters.Add(this.ExecuteSQL.Factory.GetDbParameter(parameterName, parameterValue));

                //Return this back to the caller
                return this.GetParameter(parameterName);
            }
        }
        /// <summary>
        /// Adds the passed in parameter to the parameters collection for the current <see cref="DbConnection"/>
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the passed in parameter name is already present in the parameters collection</exception>
        /// <param name="param">An instance of <see cref="DbParameter"/> object, that is created the by the caller</param>
        public DbParameter AddParameter(DbParameter param)
        {
            //Check if this parameter exists before adding to collection
            if (this.Contains(param.ParameterName) == true)
            {
                throw new ArgumentException($"Parameter with name {param.ParameterName} already exists", nameof(param));
            }
            else
            {
                //Check if the value passed in was null
                if (param.Value == null)
                {
                    param.Value = DBNull.Value;
                }
                //Check if we need to add in the @ sign
                if (param.ParameterName.StartsWith(this.VariableBinder) == false)
                {
                    param.ParameterName = this.VariableBinder + param.ParameterName;
                }

                //Add this parameter
                this.ExecuteSQL.Parameters.Add(param);

                //Return this back to the caller
                return this.GetParameter(param.ParameterName);
            }
        }
        /// <summary>
        /// Adds an <see cref="IEnumerable{DbParameter}"/> objects to the helpers underlying db parameter collection for the current <see cref="DbConnection"/>
        /// </summary>
        /// <exception cref="ArgumentException">Throws argument exception when there are duplicate parameter names</exception>
        /// <param name="dbParams">An <see cref="IEnumerable{DbParameter}"/> objects to add to the helpers underlying db parameter collection</param>
        public void AddParameterRange(IEnumerable<DbParameter> dbParams)
        {
            //Check if any of the items in this IEnumerable already exists in the list by checking parameter name
            foreach (DbParameter dbParam in dbParams)
            {
                //Raise exception here if parameter by name already exists
                if (this.Contains(dbParam.ParameterName) == true)
                {
                    throw new ArgumentException($"Parameter with name {dbParam.ParameterName} already exists");
                }
            }

            //Add this range of parameters to the parameters list
            this.ExecuteSQL.Parameters.AddRange(dbParams);
        }
        /// <summary>
        /// Adds an <see cref="IDictionary{TKey, TValue}"/> object to add to the helpers underlying db parameter collection for the current <see cref="DbConnection"/>
        /// </summary>
        /// <param name="dbParams">An <see cref="IDictionary{TKey, TValue}"/> of <see cref="KeyValuePair{TKey, TValue}"/> where the key is a parameter name and the value is the value of a parameter</param>
        public void AddParameterRange(IDictionary<string, object> dbParams)
        {
            //Check if any of the items in this IEnumerable already exists in the list by checking parameter name
            foreach (KeyValuePair<string, object> kvp in dbParams)
            {
                //Raise exception here if parameter by name already exists
                if (this.Contains(kvp.Key) == true)
                {
                    throw new ArgumentException($"Parameter with name {kvp.Key} already exists");
                }
                else
                {
                    //Add this parameter to the collection
                    this.ExecuteSQL.Parameters.Add(this.ExecuteSQL.Factory.GetDbParameter(kvp.Key, kvp.Value));
                }
            }
        }
        /// <summary>
        /// Checks for a paremeter in the parameters list with the passed in name
        /// </summary>
        /// <param name="parameterName">The name of the parameter to use when searching the Parameters list</param>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="parameterName"/> passed into routine is null or empty</exception>
        /// <returns>True if this parameter exists in the parameters collection, false otherwise</returns>
        public bool Contains(string parameterName)
        {
            bool parameterExists = false;

            //Check if this even has a name, caller be using a provider that doesn't support named parameters
            if (string.IsNullOrEmpty(parameterName) || parameterName.Trim() == string.Empty)
            {
                throw new ArgumentException(nameof(parameterName) + "cannot be null or empty");
            }

            //Return this back to the caller
            foreach (DbParameter param in this.ExecuteSQL.Parameters)
            {
                //Check for parameter name including the variable binder
                if (param.ParameterName.ToUpper() == this.VariableBinder + parameterName.ToUpper())
                {
                    parameterExists = true;
                    break;
                }
            }

            //Return this back to the caller
            return parameterExists;
        }
        /// <summary>
        /// Checks for a paremeter in the parameters list with the passed in index
        /// </summary>
        /// <param name="index">The index of the parameter in the parameters collection to identify the parameter to remove from the collection</param>
        /// <returns>Returns true if item was found in the paramerters collection, false otherwise if item was not found in the collection</returns>
        public bool Contains(int index)
        {
            try
            {
                DbParameter param;

                //Get the parameter at this index
                param = this.ExecuteSQL.Parameters[index];

                //We found this item, tell the caller
                return (param == null);
            }
            catch (Exception ex)
            {
                //We couldn't find this item, tell the caller
                return false;
            }
        }
        #endregion
        #region Other Methods
#if !NETSTANDARD1_3
        /// <summary>
        /// Provides a mechanism for enumerating all available instances of database servers within the local network
        /// </summary>
        /// <returns>Returns a new instance of <see cref="DbDataSourceEnumerator"/> created by the current <see cref="DbProviderFactory"/></returns>
        public DbDataSourceEnumerator GetDataSourceEnumerator()
        {
            //Return this back to the caller
            return this.ExecuteSQL.Factory.GetDataSourceEnumerator();
        }
#endif
        #endregion
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
                    this.Close();
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
