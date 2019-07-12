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
#endregion

namespace ADONetHelper
{
    /// <summary>
    /// Utility class that provides methods for both retrieving and modifying data in a data store
    /// </summary>
    /// <seealso cref="ISqlExecutor"/>
    /// <remarks>
    /// SqlExecutor is a class that encompasses a <see cref="DbCommand"/> but the user of
    /// the class must manage the <see cref="DbConnection"/> the class will use
    /// </remarks>
    public partial class SqlExecutor : ISqlExecutor
    {
        #region Fields/Properties
        private readonly IDbObjectFactory _factory;
        private List<DbParameter> _parameters;
        private DbConnectionStringBuilder _connectionstringBuilder;
        private DbConnection _connection;

        /// <summary>
        /// An instance of the database object factory to create database object instances
        /// </summary>
        public IDbObjectFactory Factory
        {
            get
            {
                //Set this property
                _factory.VariableBinder = this.VariableBinder;

                //Return this back to the caller
                return _factory;
            }
        }
        /// <summary>
        /// The list of query database parameters that are associated with a query
        /// </summary>
        public List<DbParameter> Parameters
        {
            get
            {
                //Check for null reference
                _parameters = _parameters ?? new List<DbParameter>();

                //Return this back to the caller
                return _parameters;
            }
            set
            {
                _parameters = value;
            }
        }
        /// <summary>
        /// The character symbol to use when binding a variable in a given providers SQL query, such as @ symbol
        /// </summary>
        public string VariableBinder { get; set; } = "@";
        /// <summary>
        /// <see cref="DbConnectionStringBuilder"/> to use to build a connection string based off of the data provider DB helper is using
        /// </summary>
        public DbConnectionStringBuilder ConnectionStringBuilder
        {
            get
            {
                //Get the connection string builder
                _connectionstringBuilder = _connectionstringBuilder ?? this.Factory.GetDbConnectionStringBuilder();

                //Return this back to the caller
                return _connectionstringBuilder;
            }
        }
        /// <summary>
        /// Represents an instance of <see cref="DbConnection"/>
        /// </summary>
        public DbConnection Connection
        {
            get
            {
                //Get the DbConnection
                _connection = _connection ?? this.Factory.GetDbConnection();

                //Check if this was set
                if (string.IsNullOrEmpty(_connection.ConnectionString) || _connection.ConnectionString.Trim() == string.Empty)
                {
                    _connection.ConnectionString = this.ConnectionStringBuilder.ConnectionString;
                }

                //Return this back to the caller
                return _connection;
            }
        }
        /// <summary>
        /// Gets or sets the wait time in seconds before terminating the attempt to execute a command and generating an error.
        /// </summary>
        public int CommandTimeout { get; set; } = 30;
        #endregion
        #region Constructors
        /// <summary>
        /// Constructor to query a database using an existing <see cref="DbConnection"/> to initialize the <paramref name="connection"/>
        /// </summary>
        /// <param name="connection">An instance of <see cref="DbConnection"/> to use to query a database </param>
        public SqlExecutor(DbConnection connection)
        {
            //Set properties
            _connection = connection;
            _factory = new DbObjectFactory(connection);

            //Get the connection string from the connection
            this.ConnectionStringBuilder.ConnectionString = _connection.ConnectionString;
        }
#if NET20 || NET35 || NET40 || NET451 || NETSTANDARD2_1
        /// <summary>
        /// Instantiates a new instance of <see cref="SqlExecutor"/> with the passed in <paramref name="row"/>
        /// </summary>
        /// <param name="row">An instance of <see cref="DataRow"/> that contains the information to configure a <see cref="DbProviderFactory"/></param>
        public SqlExecutor(DataRow row)
        {
            _factory = new DbObjectFactory(row);
        }
#endif
        /// <summary>
        /// The overloaded constuctor that will initialize the <paramref name="providerName"/>
        /// </summary>
        /// <example>
        /// An example of provider name would be System.Data.SqlClient.  The drivers assembly needs to be installed in the Global Assembly Cache in order to be referenced.
        /// </example>
        /// <param name="providerName">The name of the data provider that the should be used to query a data store</param>
        public SqlExecutor(string providerName)
        {
            //Set properties
            _factory = new DbObjectFactory(providerName);
        }
        /// <summary>
        /// Initializes a new instance with an instance of <see cref="IDbObjectFactory"/>
        /// </summary>
        /// <param name="factory">An instance of <see cref="IDbObjectFactory"/></param>
        public SqlExecutor(IDbObjectFactory factory)
        {
            _factory = factory;
        }
        /// <summary>
        /// Instantiates a new instance of <see cref="SqlExecutor"/> with the <paramref name="factory"/>
        /// </summary>
        /// <param name="factory">An instance of <see cref="DbProviderFactory"/></param>
        public SqlExecutor(DbProviderFactory factory)
        {
            _factory = new DbObjectFactory(factory);
        }
        #endregion
        #region Helper Methods
        /// <summary>
        /// Gets all of the existing output parameter values from the passed in command object
        /// </summary>
        /// <param name="command">An instance of an existing <see cref="DbCommand"/> object to retrieve the output parameter values from</param>
        private List<DbParameter> GetParameterList(DbCommand command)
        {
            List<DbParameter> returnList = new List<DbParameter>();

            //Loop through all parameters
            foreach (DbParameter param in command.Parameters)
            {
                returnList.Add(param);
            }

            //We need to clear all parameters from the command, so that we can reuse parameters on the same connection
            command.Parameters.Clear();

            //Return this back to the caller
            return returnList;
        }
        #endregion
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// Releases the unmanaged resources used by the Component and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            //Check if we have already disposed objects
            if (!disposedValue)
            {
                //Check if we are disposing objects
                if (disposing)
                {
                    //Close the connection
                    this.Connection.Close();
                }

                //Set that we have disposed of objects
                disposedValue = true;
            }
        }
        /// <summary>
        /// Destructor for the class
        /// </summary>
        ~SqlExecutor()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }
        /// <summary>
        /// Releases the unmanaged resources used by the Component
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);

            //Now supress the finalizer
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}