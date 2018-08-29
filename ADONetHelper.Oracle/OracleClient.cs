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
using Oracle.ManagedDataAccess.Client;
using System.IO;
using System.Data;
using System.Security;
using System.Xml;
#endregion

namespace ADONetHelper.Oracle
{
    /// <summary>
    /// A specialized instance of <see cref="DbClient"/> that is used to query an Oracle database system
    /// </summary>
    /// <seealso cref="DbClient"/>
    public class OracleClient : DbClient
    {
        #region Fields/Properties
        /// <summary>
        /// This property specifies the action name for the connection.
        /// </summary>
        /// <value>
        /// The string to be used as the action name.
        /// </value>
        /// <remarks>
        /// The default value is null. Using the ActionName property allows the application to set the action name in the application context for a given OracleConnection object.
        /// The ActionName property is reset to null when the Close or Dispose method is called on the <see cref="OracleConnection"/> object.
        /// </remarks>
        public string ActionName
        {
            set
            {
                //Set the value
                this.Connection.ActionName = value;
            }
        }
        /// <summary>
        /// This property specifies the name of the database domain that this connection is connected to.
        /// </summary>
        /// <value>
        /// The database domain that this connection is connected to
        /// </value>
        public string DatabaseDomainName
        {
            get
            {
                //Return this back to the caller
                return this.Connection.DatabaseDomainName;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int StatementCacheSize
        {
            get
            {
                //Return this back to the caller
                return this.Connection.StatementCacheSize;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ServiceName
        {
            get
            {
                //Return this back to the caller
                return this.Connection.ServiceName;
            }
            set
            {
                this.Connection.ServiceName = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool SwitchedConnection
        {
            get
            {
                //Return this back to the caller
                return this.Connection.SwitchedConnection;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string HostName
        {
            get
            {
                //Return this back to the caller
                return this.Connection.HostName;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ModuleName
        {
            set
            {
                this.Connection.ModuleName = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string InstanceName
        {
            get
            {
                //Return this back to the caller
                return this.Connection.InstanceName;
            }
        }
        /// <summary>
        /// Gets the name of the PDB.
        /// </summary>
        /// <value>
        /// The name of the PDB.
        /// </value>
        public string PDBName
        {
            get
            {
                //Return this back to the caller
                return this.Connection.PDBName;
            }
            set
            {
                this.Connection.PDBName = value;
            }
        }
        /// <summary>
        /// An instance of <see cref="OracleConnection"/> to use to connect to an Oracle database
        /// </summary>
        /// <returns></returns>
        private OracleConnection Connection
        {
            get
            {
                //Return this back to the caller
                return (OracleConnection)this.ExecuteSQL.Connection;
            }
        }
        #endregion
        #region Constructors
        /// <summary>
        /// Intializes the <see cref="OracleClient"/> with a <see cref="ISqlExecutor"/>
        /// </summary>
        /// <param name="executor">An instance of <see cref="ISqlExecutor"/></param>
        public OracleClient(ISqlExecutor executor) : base(executor)
        {

        }
        /// <summary>
        /// The overloaded constuctor that will initialize the <paramref name="connectionString"/>, And <paramref name="queryCommandType"/>
        /// </summary>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        public OracleClient(string connectionString, CommandType queryCommandType) : base(connectionString, queryCommandType, OracleClientFactory.Instance)
        {

        }
        /// <summary>
        /// The overloaded constuctor that will initialize the <paramref name="connectionString"/>
        /// </summary>
        /// <param name="connectionString">The connection string used to query a data store</param>
        public OracleClient(string connectionString) : base(connectionString, OracleClientFactory.Instance)
        {

        }
        /// <summary>
        /// Constructor to query a database using an existing <see cref="OracleConnection"/> to initialize the <paramref name="connection"/>
        /// </summary>
        /// <param name="connection">An instance of <see cref="OracleConnection"/> to use to query a database</param>
        public OracleClient(OracleConnection connection) : base(connection)
        {

        }
        #endregion
        #region Utility Methods
        /// <summary>
        /// Returns an instance of <see cref="XmlReader"/> based on the <paramref name="query"/>
        /// </summary>
        /// <param name="query">The query command text Or name of stored procedure to execute against the data store</param>
        /// <returns>Returns an instance of <see cref="XmlReader"/> based on the <paramref name="query"/> passed into the routine</returns>
        public XmlReader ExecuteXmlReader(string query)
        {
            //Wrap this in a using statement to automatically dispose of resources
            using (OracleCommand command = (OracleCommand)this.ExecuteSQL.Factory.GetDbCommand(this.QueryCommandType, query, this.ExecuteSQL.Parameters, this.Connection, this.CommandTimeout))
            {
                XmlReader reader = command.ExecuteXmlReader();

                //Clear any parameters
                command.Parameters.Clear();

                //Return this back to the caller
                return reader;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public Stream ExecuteStream(string query, OracleXmlCommandType commandType)
        {
            //Wrap this in a using statement to automatically dispose of resources
            using (OracleCommand command = (OracleCommand)this.ExecuteSQL.Factory.GetDbCommand(this.QueryCommandType, query, this.ExecuteSQL.Parameters, this.Connection, this.CommandTimeout))
            {
                command.XmlCommandType = commandType;

                //Get the stream
                Stream stream = command.ExecuteStream();

                //Clear the parameters
                command.Parameters.Clear();

                //Return this back to the caller
                return stream;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="outputStream"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public void ExecuteToStream(string query, Stream outputStream, OracleXmlCommandType commandType)
        {
            //Wrap this in a using statement to automatically dispose of resources
            using (OracleCommand command = (OracleCommand)this.ExecuteSQL.Factory.GetDbCommand(this.QueryCommandType, query, this.ExecuteSQL.Parameters, this.Connection, this.CommandTimeout))
            {
                command.XmlCommandType = commandType;

                //Set the output stream
                command.ExecuteToStream(outputStream);

                //Clear any parameters
                command.Parameters.Clear();
            }
        }
        #endregion
        #region Connection Methods
        /// <summary>
        /// Opens the current <see cref="OracleConnection"/> with a new password as a <see cref="string"/>, particularly useful if password has an expiration datetime
        /// </summary>
        /// <param name="password">The password to use when opening the connection</param>
        public void OpenWithNewPassowrd(string password)
        {
            //Try and reopen the connection with a new password
            this.Connection.OpenWithNewPassword(password);
        }
        /// <summary>
        /// Opens the current <see cref="OracleConnection"/> with a new password as a <see cref="SecureString"/>, particularly useful if password has an expiration datetime
        /// </summary>
        /// <param name="password">The password to use when opening the connection</param>
        public void OpenWithNewPassowrd(SecureString password)
        {
            //Try and reopen the connection with a new password
            this.Connection.OpenWithNewPassword(password);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public OracleGlobalization GetSessionInfo()
        {
            //Return this back to the caller
            return this.Connection.GetSessionInfo();
        }
        /// <summary>
        /// Purges the statement cache.
        /// </summary>
        public void PurgeStatementCache()
        {
            this.Connection.PurgeStatementCache();
        }
        /// <summary>
        /// Sets the session information.
        /// </summary>
        /// <param name="globe">The globe.</param>
        public void SetSessionInfo(OracleGlobalization globe)
        {
            //Return this back to the caller
            this.Connection.SetSessionInfo(globe);
        }
        #endregion
    }
}
