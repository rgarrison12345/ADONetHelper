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
    /// Base class that implements the basic functionality of an ADO.NET driver
    /// </summary>
    /// <seealso cref="IDbProvider"/>
    /// <seealso cref="IConnectionStringUtility"/>
    /// <seealso cref="IDbParameterUtility"/>
    public abstract class DbProvider : IDbProvider, IConnectionStringUtility, IDbParameterUtility
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
                lock (ExecuteSQL.Connection)
                {
                    ExecuteSQL.Connection.StateChange += value;
                }
            }
            remove
            {
                //Get an exclusive lock first
                lock (ExecuteSQL.Connection)
                {
                    ExecuteSQL.Connection.StateChange -= value;
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
                return ExecuteSQL.Factory.CanCreateDataSourceEnumerator;
            }
        }
        /// <summary>
        /// Whether or not the <see cref="ConnectionString"/> is readonly
        /// </summary>
        public bool ConnectionStringReadonly
        {
            get
            {
                //Return this back to the caller
                return ExecuteSQL.ConnectionStringBuilder.IsReadOnly;
            }
        }
        /// <summary>
        /// Whether the <see cref="ConnectionString"/> has a fixed size
        /// </summary>
        public bool ConnectionStringFixedSize
        {
            get
            {
                //Return this back to the caller
                return ExecuteSQL.ConnectionStringBuilder.IsFixedSize;
            }
        }
#endif
        /// <summary>
        /// Gets the current number of keys that are contained within the <see cref="ConnectionString"/> property
        /// </summary>
        public int ConnectionStringKeyCount
        {
            get
            {
                //Return this back to the caller
                return ExecuteSQL.ConnectionStringBuilder.Count;
            }
        }
        /// <summary>
        /// Represents an instance of the <see cref="ISqlExecutor"/> class to facilitate querying a data store
        /// </summary>
        protected ISqlExecutor ExecuteSQL
        {
            get
            {
                //Set the command and connection timeout
                _executeSQL.CommandTimeout = CommandTimeout;

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
                return ExecuteSQL.Connection.ServerVersion;
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
                return ExecuteSQL.Connection.State;
            }
        }
        /// <summary>
        /// Gets the name of the current database after a connection is opened, or the database name specified in the connection string before the <see cref="DbConnection"/> is opened.
        /// </summary>
        public virtual string Database
        {
            get
            {
                //Return this back to the caller
                return ExecuteSQL.Connection.Database;
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
                return ExecuteSQL.Connection.DataSource;
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
                return ExecuteSQL.ConnectionStringBuilder.ConnectionString;
            }
            set
            {
                //Set the connection string
                ExecuteSQL.ConnectionStringBuilder.ConnectionString = value;
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
                return ExecuteSQL.Connection.ConnectionTimeout;
            }
        }
        #endregion
        #region Constructors
#if !NETSTANDARD1_3 && !NETSTANDARD2_0
        /// <summary>
        /// Instantiates a new instance of <see cref="DbProvider"/> with the passed in <paramref name="row"/> and <paramref name="connectionString"/>
        /// </summary>
        /// <param name="row">An instance of <see cref="DataRow"/> that contains the information to configure a <see cref="DbProviderFactory"/></param>
        /// <param name="connectionString">The connection string to be used to query a data store</param>
        protected DbProvider(DataRow row, string connectionString)
        {
            _executeSQL = new SqlExecutor(row);
            ConnectionString = connectionString;
        }
#endif
        /// <summary>
        /// Instantiates a new instance of <see cref="DbProvider"/> with an instance of <see cref="ISqlExecutor"/>
        /// </summary>
        /// <param name="executor">An instance of <see cref="ISqlExecutor"/></param>
        protected DbProvider(ISqlExecutor executor)
        {
            //Set fields
            _executeSQL = executor;
        }
        /// <summary>
        /// Instantiates a new instance of <see cref="DbProvider"/> with the passed in <paramref name="connectionString"/>, and <paramref name="queryCommandType"/>, and <paramref name="factory"/>
        /// </summary>
        /// <param name="factory">An instance of a <see cref="DbProviderFactory"/> client class</param>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        protected DbProvider(string connectionString, CommandType queryCommandType, DbProviderFactory factory)
        {
            //Set fields/properties
            _executeSQL = new SqlExecutor(factory);
            ConnectionString = connectionString;
            QueryCommandType = queryCommandType;
        }
        /// <summary>
        /// Instantiates a new instance of <see cref="DbProvider"/> with the passed in <paramref name="connectionString"/> and <paramref name="factory"/>
        /// </summary>
        /// <param name="factory">An instance of the <see cref="DbProviderFactory"/> client class</param>
        /// <param name="connectionString">The connection string used to query a data store</param>
        protected DbProvider(string connectionString, DbProviderFactory factory)
        {
            //Set fields/properties
            _executeSQL = new SqlExecutor(factory);
            ConnectionString = connectionString;
        }
        /// <summary>
        /// Instantiates a new instance of <see cref="DbProvider"/> with the passed in with the passed in <paramref name="connectionString"/>, <paramref name="providerName"/>, and <paramref name="queryCommandType"/>
        /// </summary>
        /// <param name="providerName">The name of the data provider that the should be used to query a data store</param>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        protected DbProvider(string connectionString, string providerName, CommandType queryCommandType) : this(connectionString, providerName)
        {
            //Set properties
           QueryCommandType = queryCommandType;
        }
        /// <summary>
        /// Instantiates a new instance of <see cref="DbProvider"/> with the passed in <paramref name="connectionString"/> and <paramref name="providerName"/>
        /// </summary>
        /// <param name="providerName">The name of the data provider that the should be used to query a data store</param>
        /// <param name="connectionString">The connection string used to query a data store</param>
        protected DbProvider(string connectionString, string providerName) : this(providerName)
        {
            //Set properties
            ConnectionString = connectionString;
        }
        /// <summary>
        /// Instantiates a new instance of <see cref="DbProvider"/> with the passed in <paramref name="providerName"/>
        /// </summary>
        /// <param name="providerName">The name of the data provider that the should be used to query a data store</param>
        protected DbProvider(string providerName)
        {
            //Set fields
            _executeSQL = new SqlExecutor(providerName);
        }
        /// <summary>
        /// Instantiates a new instance of <see cref="DbProvider"/> using an existing <see cref="DbConnection"/> to initialize the <paramref name="connection"/>
        /// </summary>
        /// <param name="connection">An instance of <see cref="DbConnection"/> to use to query a database </param>
        protected DbProvider(DbConnection connection)
        {
            //Set properties
            _executeSQL = new SqlExecutor(connection);
        }
        /// <summary>
        /// Instantiates a new instance of <see cref="DbProvider"/> using the passed in <paramref name="connectionString"/> and <paramref name="factory"/>
        /// </summary>
        /// <param name="connectionString">The connection string used to query a database</param>
        /// <param name="factory">An instance of <see cref="IDbObjectFactory"/> to create the objects needed to help query a database</param>
        protected DbProvider(string connectionString, IDbObjectFactory factory)
        {
            //Set properties
            _executeSQL = new SqlExecutor(factory);
            ConnectionString = connectionString;
        }
        #endregion
        #region ConnectionString methods
        /// <summary>
        /// Adds a property name and value to the current connection string
        /// </summary>
        /// <param name="name">The name of the connection string property</param>
        /// <param name="value">The value to use with the connection string property</param>
        public void AddConnectionStringProperty(string name, object value)
        {
            //Add this property name and value
            ExecuteSQL.ConnectionStringBuilder.Add(name, value);
        }
        /// <summary>
        /// Removes a connection string property from the connection string by name
        /// </summary>
        /// <param name="name">The name of the connection string property</param>
        public void RemoveConnectionStringProperty(string name)
        {
            //Remove this property
            ExecuteSQL.ConnectionStringBuilder.Remove(name);
        }
        /// <summary>
        /// Retrieves a connection string property value as an object
        /// </summary>
        /// <param name="name">The name of the connection string property</param>
        /// <returns>Returns a connection string property as an <see cref="object"/>, null if property is not present</returns>
        public object GetConnectionStringPropertyValue(string name)
        {
            //Remove this property
            if (ExecuteSQL.ConnectionStringBuilder.TryGetValue(name, out object value) == true)
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
           ExecuteSQL.ConnectionStringBuilder.Clear();
        }
        /// <summary>
        /// Checks if the current <see cref="ConnectionString"/> in use contains the passed in <paramref name="keyword"/>
        /// </summary>
        /// <param name="keyword">The keyword to verify is in the current <see cref="ConnectionString"/></param>
        /// <returns>Returns a <see cref="bool"/> indicating if the <see cref="ConnectionString"/> contains the passed in <paramref name="keyword"/></returns>
        public bool ConnectionStringAllowsKey(string keyword)
        {
            //Return this back to the caller
            return ExecuteSQL.ConnectionStringBuilder.ContainsKey(keyword);
        }
        /// <summary>
        /// Configures the connection string with the key value pairs passed into the routine
        /// This will clear the current connection string to start over
        /// </summary>
        /// <param name="properties">Key value pairs of connection string property names and values</param>
        public void ConfigureConnectionString(IDictionary<string, object> properties)
        {
            //Clear any existing properties
            ExecuteSQL.ConnectionStringBuilder.Clear();

            //Loop through properties and build this out
            foreach (KeyValuePair<string, object> kvp in properties)
            {
                //Add this into the builder
                ExecuteSQL.ConnectionStringBuilder.Add(kvp.Key, kvp.Value);
            }
        }
        #endregion
        #region Parameter Methods
        /// <summary>
        /// Retrieves the entire <see cref="List{T}"/> of <see cref="DbParameter"/> that are currently in use
        /// </summary>
        /// <returns>Returns a <see cref="List{T}"/> of <see cref="DbParameter"/></returns>
        public List<DbParameter> GetCurrentParameters()
        {
            //Return this back to the caller
            return ExecuteSQL.Parameters;
        }
        /// <summary>
        /// Removes a <see cref="DbParameter"/> from the parameters collection for the current <see cref="DbConnection"/> by using the parameter name
        /// </summary>
        /// <param name="parameterName">The name of the parameter to identify the parameter to remove from the collection</param>
        /// <returns>Returns true if item was successully removed, false otherwise if item was not found in the list</returns>
        public bool RemoveParameter(string parameterName)
        {
            //Return this back to the caller
            return ExecuteSQL.Parameters.Remove(ExecuteSQL.Parameters.Find(x => x.ParameterName == VariableBinder + parameterName));
        }
        /// <summary>
        /// Removes a <see cref="DbParameter"/> from the parameters collection for the current <see cref="DbConnection"/> by using the index of the parameter
        /// </summary>
        /// <param name="index">The index of the parameter in the parameters collection to identify the parameter to remove from the collection</param>
        /// <returns>Returns true if item was successully removed, false otherwise if item was not found in the list</returns>
        /// <exception cref="IndexOutOfRangeException">thrown when an attempt is made to access an element of an array or collection with an index that is outside the bounds of the array or less than zero.</exception>
        public bool RemoveParameter(int index)
        {
            //Return this back to the caller
            return ExecuteSQL.Parameters.Remove(ExecuteSQL.Parameters[index]);
        }
        /// <summary>
        /// Clears all parameters from the parameters collection
        /// </summary>
        public void ClearParameters()
        {
            ExecuteSQL.Parameters?.Clear();
            ExecuteSQL.Parameters = null;
        }
        /// <summary>
        /// Retrieves a <see cref="DbParameter"/> object by using the passed in parameter name
        /// </summary>
        /// <exception cref="ArgumentException">Throws when the passed in <paramref name="parameterName"/> is <c>null</c> or <see cref="string.Empty"/></exception>
        /// <exception cref="InvalidOperationException">Thrown when the passed in parameter name is not present in the parameters collection</exception>
        /// <param name="parameterName">The name of the parameter to use to find the parameter value</param>
        /// <returns>The specified <see cref="DbParameter"/> object from the parameters collection</returns>
        public DbParameter GetParameter(string parameterName)
        {
            //Check for null or empty
            if (string.IsNullOrEmpty(parameterName) || parameterName.Trim() == string.Empty)
            {
                throw new ArgumentException(nameof(parameterName) + " is null or empty");
            }

            //Loop through all parameters
            foreach (DbParameter param in this.ExecuteSQL.Parameters)
            {
                //Check if the name is the same
                if (string.Equals(param.ParameterName, string.Concat(VariableBinder, parameterName), StringComparison.OrdinalIgnoreCase) == true)
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
        /// <exception cref="IndexOutOfRangeException">thrown when an attempt is made to access an element of an array or collection with an index that is outside the bounds of the array or less than zero.</exception>
        public DbParameter GetParameter(int index)
        {
            //Return this back to the caller
            return ExecuteSQL.Parameters[index];
        }
        /// <summary>
        /// Replaces an existing parameter with the new <see cref="DbParameter"/> with an existing <see cref="DbParameter.ParameterName"/>
        /// </summary>
        /// <param name="parameterName">The index as a <c>string</c> to use when searching for the existing parameter</param>
        /// <param name="param">A new instance of <see cref="DbParameter"/></param>
        public void ReplaceParameter(string parameterName, DbParameter param)
        {
            //Get index of this parameter
            int index = ExecuteSQL.Parameters.FindIndex(i => string.Equals(i.ParameterName, string.Concat(VariableBinder, parameterName), StringComparison.OrdinalIgnoreCase) == true);

            //Do a replace of the parameter
            ExecuteSQL.Parameters[index] = param;
        }
        /// <summary>
        /// Replaces an existing parameter with the new <see cref="DbParameter"/> passed in at the <paramref name="index"/>
        /// </summary>
        /// <param name="index">The index as an <see cref="int"/> to use when searching for the existing parameter</param>
        /// <param name="param">A new instance of <see cref="DbParameter"/></param>
        /// <exception cref="IndexOutOfRangeException">thrown when an attempt is made to access an element of an array or collection with an index that is outside the bounds of the array or less than zero.</exception>
        public void ReplaceParameter(int index, DbParameter param)
        {
            //Do a replace of the parameter
            ExecuteSQL.Parameters[index] = param;
        }
        /// <summary>
        /// Sets the value of an existing <see cref="DbParameter"/> by using the <paramref name="parameterName"/> and passed in <paramref name="value"/>
        /// </summary>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="value">The value of the parameter as an <see cref="object"/></param>
        public void SetParamaterValue(string parameterName, object value)
        {
            //Get index of this parameter
            int index = ExecuteSQL.Parameters.FindIndex(i => string.Equals(i.ParameterName, string.Concat(VariableBinder, parameterName), StringComparison.OrdinalIgnoreCase) == true);

            //Change the value
            SetParamaterValue(index, value);
        }
        /// <summary>
        /// Sets the value of an existing <see cref="DbParameter"/> by using the <paramref name="index"/> and passed in <paramref name="value"/>
        /// </summary>
        /// <param name="index">The index of the parameter in the parameters collection to identify the parameter to retrieve from the collection</param>
        /// <param name="value">The value of the parameter as an <see cref="object"/></param>
        /// <exception cref="IndexOutOfRangeException">thrown when an attempt is made to access an element of an array or collection with an index that is outside the bounds of the array or less than zero.</exception>
        public void SetParamaterValue(int index, object value)
        {
            DbParameter param = GetParameter(index);

            //Now set the value
            param.Value = value;

            //Change the value
            ExecuteSQL.Parameters[index] = param;
        }
#if !NET20 && !NET35 && !NET40 && !NET45
        /// <summary>
        /// Gets an initialized instance of a <see cref="DbParameter"/> object based on the specified provider
        /// </summary>
        /// <exception cref="ArgumentException">Throws argument exception when there are duplicate parameter names</exception>
        /// <param name="dataType">The <see cref="DbType"/> of the field in the database</param>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter as a <see cref="object"/></param>
        /// <param name="scale">The number of decimal places to which the <see cref="DbParameter.Value"/> property is resolved.  The default value is <c>null</c></param>
        /// <param name="precision">The maximum number of digits used to represent the <see cref="DbParameter.Value"/> property.  The default value is <c>null</c></param>
        /// <param name="paramDirection">The direction of the parameter, defaults to <see cref="ParameterDirection.Input"/></param>
        /// <returns>Returns an instance of <see cref="DbParameter"/> object with information passed into procedure</returns>
        public DbParameter AddFixedSizeParameter(string parameterName, object parameterValue, DbType dataType, byte? scale = null, byte? precision = null, ParameterDirection paramDirection = ParameterDirection.Input)
        {
            //Check if this parameter exists before adding to collection
            if (Contains(parameterName) == true)
            {
                throw new ArgumentException($"Parameter with name {parameterName} already exists", nameof(parameterName));
            }
            else
            {
                //Add this parameter to the collection
                ExecuteSQL.Parameters.Add(ExecuteSQL.Factory.GetFixedSizeDbParameter(parameterName, parameterValue, dataType, scale, precision, paramDirection));

                //Return this back to the caller
                return GetParameter(parameterName);
            }
        }
#endif
        /// <summary>
        /// Adds a new <see cref="DbParameter"/> to the parameters collection for the current <see cref="DbConnection"/>
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the passed in parameter name is already present in the parameters collection</exception>
        /// <param name="type">The data type of the parameter being sent to the data store with the query</param>
        /// <param name="size">The maximum size, in bytes, of the data being sent to the datastore.  If parameter is a variable length don't set for input parameters</param>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter</param>
        /// <param name="paramDirection">The direction of the parameter, defaults to input.  The size must be set for output parameters</param>
        public DbParameter AddVariableSizeParameter(string parameterName, object parameterValue, DbType type, int? size = null, ParameterDirection paramDirection = ParameterDirection.Input)
        {
            //Check if this parameter exists before adding to collection
            if (Contains(parameterName) == true)
            {
                throw new ArgumentException($"Parameter with name {parameterName} already exists", nameof(parameterName));
            }
            else
            {
                //Add this parameter to the collection
                ExecuteSQL.Parameters.Add(ExecuteSQL.Factory.GetVariableSizeDbParameter(parameterName, parameterValue, type, size, paramDirection));

                //Return this back to the caller
                return GetParameter(parameterName);
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
            if (Contains(parameterName) == true)
            {
                throw new ArgumentException($"Parameter with name {parameterName} already exists", nameof(parameterName));
            }
            else
            {
                //Add this parameter to the collection
                ExecuteSQL.Parameters.Add(ExecuteSQL.Factory.GetDbParameter(parameterName, parameterValue));

                //Return this back to the caller
                return GetParameter(parameterName);
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
            if (Contains(param.ParameterName) == true)
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
                //Check if we need to add in the variable binder
                if (param.ParameterName.StartsWith(VariableBinder) == false)
                {
                    param.ParameterName = VariableBinder + param.ParameterName;
                }

                //Add this parameter
                ExecuteSQL.Parameters.Add(param);

                //Return this back to the caller
                return param;
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
                if (Contains(dbParam.ParameterName) == true)
                {
                    throw new ArgumentException($"Parameter with name {dbParam.ParameterName} already exists");
                }
            }

            //Add this range of parameters to the parameters list
            ExecuteSQL.Parameters.AddRange(dbParams);
        }
        /// <summary>
        /// Adds an <see cref="IDictionary{TKey, TValue}"/> object to add to the helpers underlying db parameter collection for the current <see cref="DbConnection"/>
        /// </summary>
        /// <exception cref="ArgumentException">Throws argument exception when there are duplicate parameter names</exception>
        /// <param name="dbParams">An <see cref="IDictionary{TKey, TValue}"/> of <see cref="KeyValuePair{TKey, TValue}"/> where the key is a parameter name and the value is the value of a parameter</param>
        public void AddParameterRange(IDictionary<string, object> dbParams)
        {
            //Check if any of the items in this IEnumerable already exists in the list by checking parameter name
            foreach (KeyValuePair<string, object> kvp in dbParams)
            {
                //Raise exception here if parameter by name already exists
                if (Contains(kvp.Key) == true)
                {
                    throw new ArgumentException($"Parameter with name {kvp.Key} already exists");
                }
                else
                {
                    //Add this parameter to the collection
                    ExecuteSQL.Parameters.Add(ExecuteSQL.Factory.GetDbParameter(kvp.Key, kvp.Value));
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
                if (string.Equals(param.ParameterName, string.Concat(VariableBinder, parameterName), StringComparison.OrdinalIgnoreCase) == true)
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
                param = ExecuteSQL.Parameters[index];

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
    }
}