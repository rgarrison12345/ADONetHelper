#region Licenses
/*MIT License
Copyright(c) 2018
Robert Garrison

Permission Is hereby granted, free Of charge, To any person obtaining a copy
of this software And associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, And/Or sell
copies Of the Software, And To permit persons To whom the Software Is
furnished To Do so, subject To the following conditions:

The above copyright notice And this permission notice shall be included In all
copies Or substantial portions Of the Software.

THE SOFTWARE Is PROVIDED "AS IS", WITHOUT WARRANTY Of ANY KIND, EXPRESS Or
IMPLIED, INCLUDING BUT Not LIMITED To THE WARRANTIES Of MERCHANTABILITY,
FITNESS For A PARTICULAR PURPOSE And NONINFRINGEMENT. In NO Event SHALL THE
AUTHORS Or COPYRIGHT HOLDERS BE LIABLE For ANY CLAIM, DAMAGES Or OTHER
LIABILITY, WHETHER In AN ACTION Of CONTRACT, TORT Or OTHERWISE, ARISING FROM,
OUT Of Or In CONNECTION With THE SOFTWARE Or THE USE Or OTHER DEALINGS In THE
SOFTWARE*/
#endregion
#region Using statements
using IBM.Data.DB2.Core;
using System.Data;
#endregion

namespace ADONetHelper.DB2Client
{
    /// <summary>
    /// A client class thaat queries DB2 database system
    /// </summary>
    /// <seealso cref="ADONetHelper.DbClient" />
    public sealed class DB2Client : DbClient
    {
        #region Fields/Properties        
        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        private DB2Connection Connection
        {
            get
            {
                return (DB2Connection)this.ExecuteSQL.Connection;
            }
        }
        /// <summary>
        /// Gets the type of the server.
        /// </summary>
        /// <value>
        /// The type of the server.
        /// </value>
        public string ServerType
        {
            get
            { 
                //Return this back to the caller
                return this.Connection.ServerType;
            }
        }
        /// <summary>
        /// Gets the server build version.
        /// </summary>
        /// <value>
        /// The server build version.
        /// </value>
        public int ServerBuildVersion
        {
            get
            {
                //Return this back to the caller
                return this.Connection.ServerBuildVersion;
            }
        }
        /// <summary>
        /// Gets the server functional level.
        /// </summary>
        /// <value>
        /// The server functional level.
        /// </value>
        public string ServerFunctionalLevel
        {
            get
            {
                //Return this back to the caller
                return this.Connection.ServerFunctionalLevel;
            }
        }
        /// <summary>
        /// Gets the server minor version.
        /// </summary>
        /// <value>
        /// The server minor version.
        /// </value>
        public int ServerMinorVersion
        {
            get
            {
                //Return this back to the caller
                return this.Connection.ServerMinorVersion;
            }
        }
        /// <summary>
        /// Gets the server revision version.
        /// </summary>
        /// <value>
        /// The server revision version.
        /// </value>
        public int ServerRevisionVersion
        {
            get
            {
                //Return this back to the caller
                return this.Connection.ServerRevisionVersion;
            }
        }
        /// <summary>
        /// Gets the client workstation version.
        /// </summary>
        /// <value>
        /// The client workstation version.
        /// </value>
        public string ClientWorkstationVersion
        { 
            get
            {
                //Return this back to the caller
                return this.Connection.ClientWorkStation;
            }
        }
        /// <summary>
        /// Gets or sets the client user.
        /// </summary>
        /// <value>
        /// The client user.
        /// </value>
        public string ClientUser
        {
            get
            {
                //Return this back to the caller
                return this.Connection.ClientUser;
            }
        }
        /// <summary>
        /// Gets the name of the client program.
        /// </summary>
        /// <value>
        /// The name of the client program.
        /// </value>
        public string ClientProgramName
        {
            get
            {
                //Return this back to the caller
                return this.Connection.ClientProgramName;
            }
        }
        /// <summary>
        /// Gets the client program identifier.
        /// </summary>
        /// <value>
        /// The client program identifier.
        /// </value>
        public string ClientProgramID
        {
            get
            {
                //Return this back to the caller
                return this.Connection.ClientProgramID;
            }
        }
        /// <summary>
        /// Gets the client correlation token.
        /// </summary>
        /// <value>
        /// The client correlation token.
        /// </value>
        public string ClientCorrelationToken
        {
            get
            {
                //Return this back to the caller
                return this.Connection.ClientCorrelationToken;
            }
        }
        /// <summary>
        /// Gets the client appplication information.
        /// </summary>
        /// <value>
        /// The client appplication information.
        /// </value>
        public string ClientAppplicationInformation
        {
            get
            {
                //Return this back to the caller
                return this.Connection.ClientApplicationInformation;
            }
        }
        /// <summary>
        /// Gets the client accounting information.
        /// </summary>
        /// <value>
        /// The client accounting information.
        /// </value>
        public string ClientAccountingInformation
        {
            get
            {
                //Return this back to the caller
                return this.Connection.ClientAccountingInformation;
            }
        }
        #endregion
        #region Constructors
        /// <summary>
        /// Intializes the <see cref="DB2Client"/> with a <see cref="ISqlExecutor"/>
        /// </summary>
        /// <param name="executor">An instance of <see cref="ISqlExecutor"/></param>
        public DB2Client(ISqlExecutor executor) : base(executor)
        {
        }
        /// <summary>
        /// The overloaded constuctor that will initialize the <paramref name="connectionString"/>, And <paramref name="queryCommandType"/>
        /// </summary>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        public DB2Client(string connectionString, CommandType queryCommandType) : base(connectionString, queryCommandType, DB2Factory.Instance)
        {
        }
        /// <summary>
        /// The overloaded constuctor that will initialize the <paramref name="connectionString"/>
        /// </summary>
        /// <param name="connectionString">The connection string used to query a data store</param>
        public DB2Client(string connectionString) : base(connectionString, DB2Factory.Instance)
        {
        }
        /// <summary>
        /// Constructor to query a database using an existing <see cref="DB2Connection"/> to initialize the <paramref name="connection"/>
        /// </summary>
        /// <param name="connection">An instance of <see cref="DB2Connection"/> to use to query a database</param>
        public DB2Client(DB2Connection connection) : base(connection)
        {
        }
        /// <summary>
        /// Insantiates a new instance of <see cref="DB2Client"/> using the passed in <paramref name="connectionString"/> and <paramref name="factory"/>
        /// </summary>
        /// <param name="connectionString">Connection string to use to query a database</param>
        /// <param name="factory">An instance of <see cref="IDbObjectFactory"/></param>
        public DB2Client(string connectionString, IDbObjectFactory factory) : base(connectionString, factory)
        {
        }
        #endregion
        #region Utility Methods
        #endregion
    }
}
