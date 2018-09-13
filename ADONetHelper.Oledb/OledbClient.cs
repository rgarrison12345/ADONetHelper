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
#region Using Statements
using System.Data;
using System.Data.OleDb;
#endregion

namespace ADONetHelper.Oledb
{
    /// <summary>
    /// A specialized instance of <see cref="DbClient"/> that is used to query an Oledb data source
    /// </summary>
    /// <seealso cref="DbClient"/>
    public class OledbClient : DbClient
    {
        #region Events
        #endregion
        #region Fields/Properties
        /// <summary>
        /// An instance of <see cref="OleDbConnection"/> to use to connect to an OledDb data source
        /// </summary>
        /// <returns></returns>
        protected OleDbConnection Connection
        {
            get
            {
                //Return this back to the caller
                return (OleDbConnection)this.ExecuteSQL.Connection;
            }
        }
        /// <summary>
        /// Gets the name of the OLE DB provider specified in the "Provider= " clause of the connection string.
        /// </summary>
        public string Provider
        {
            get
            {
                return this.Connection.Provider;
            }
        }
        #endregion
        #region Constructors
        /// <summary>
        /// The overloaded constuctor that will initialize the <paramref name="connectionString"/>, And <paramref name="queryCommandType"/>
        /// </summary>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        public OledbClient(string connectionString, CommandType queryCommandType) : base(connectionString, queryCommandType, OleDbFactory.Instance)
        {
        }
        /// <summary>
        /// The overloaded constuctor that will initialize the <paramref name="connectionString"/>
        /// </summary>
        /// <param name="connectionString">The connection string used to query a data store</param>
        public OledbClient(string connectionString) : base(connectionString, OleDbFactory.Instance)
        {
        }
        /// <summary>
        /// Intializes the <see cref="OledbClient"/> with a <see cref="ISqlExecutor"/>
        /// </summary>
        /// <param name="executor">An instance of <see cref="ISqlExecutor"/></param>
        public OledbClient(ISqlExecutor executor) : base(executor)
        {
        }
        /// <summary>
        /// Constructor to query a database using an existing <see cref="OleDbConnection"/> to initialize the <paramref name="connection"/>
        /// </summary>
        /// <param name="connection">An instance of <see cref="OleDbConnection"/> to use to query a database </param>
        public OledbClient(OleDbConnection connection) : base(connection)
        {
        }
        /// <summary>
        /// Insantiates a new instance of <see cref="OledbClient"/> using the passed in <paramref name="connectionString"/> and <paramref name="factory"/>
        /// </summary>
        /// <param name="connectionString">Connection string to use to query a database</param>
        /// <param name="factory">An instance of <see cref="IDbObjectFactory"/></param>
        public OledbClient(string connectionString, IDbObjectFactory factory) : base(connectionString, factory)
        {
        }
        #endregion
        #region Methods        
        /// <summary>
        /// Updates the <see cref="System.Data.ConnectionState"/> property of the current <see cref="OleDbConnection"/> object.
        /// </summary>
        /// <remarks>
        /// Some OLE DB providers can check the current state of the connection. 
        /// For example, if the database server has recycled since you opened your <see cref="OleDbConnection"/>, 
        /// the State property will continue to return Open. If you are working with an OLE DB Provider that 
        /// supports polling for this information on a live connection, calling the <see cref="OleDbConnection.ResetState"/> 
        /// method and then checking the State property will tell you that the connection is no longer open. 
        /// The ResetState method relies on functionality in the OLE DB Provider to verify the current state 
        /// of the connection. To determine if your OLE DB Provider supports this functionality, 
        /// check the provider's documentation for information on DBPROP_CONNECTIONSTATUS.
        /// </remarks>
        public void ResetState()
        { 
            this.Connection.ResetState();
        }
        #endregion
    }
}
