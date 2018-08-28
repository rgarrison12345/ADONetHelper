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
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
#endregion

namespace ADONetHelper.MySql
{
    /// <summary>
    /// A specialized instance of <see cref="DbClient"/> that is used to query a MySql database system
    /// </summary>
    /// <seealso cref="DbClient"/>
    public class MySqlClient : DbClient
    {
        #region Fields/Properties
        /// <summary>
        /// Returns the id of the server thread the <see cref="MySqlConnection"/> is executing on
        /// </summary>
        /// <returns>Returns the id of the server thread the <see cref="MySqlConnection"/> is executing on as an <see cref="int"/></returns>
        public int ServerThread
        {
            get
            {
                //Return this back to the caller
                return this.Connection.ServerThread;
            }
        }
        /// <summary>
        /// An instance of <see cref="MySqlConnection"/>
        /// </summary>
        /// <returns>Returns an instance of <see cref="MySqlConnection"/></returns>
        protected MySqlConnection Connection
        {
            get
            {
                //Return this back to the caller
                return (MySqlConnection)this.ExecuteSQL.Connection;
            }
        }
        #endregion
        #region Constructors
        /// <summary>
        /// The overloaded constuctor that will initialize the <paramref name="connectionString"/>, And <paramref name="queryCommandType"/>
        /// </summary>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        public MySqlClient(string connectionString, CommandType queryCommandType) : base(connectionString, queryCommandType, MySqlClientFactory.Instance)
        {
        }
        /// <summary>
        /// The overloaded constuctor that will initialize the <paramref name="connectionString"/>
        /// </summary>
        /// <param name="connectionString">The connection string used to query a data store</param>
        public MySqlClient(string connectionString) : base(connectionString, MySqlClientFactory.Instance)
        {
        }
        /// <summary>
        /// Intializes the <see cref="MySqlClient"/> with a <see cref="ISqlExecutor"/>
        /// </summary>
        /// <param name="executor">An instance of <see cref="ISqlExecutor"/></param>
        public MySqlClient(ISqlExecutor executor) : base(executor)
        {
        }
        /// <summary>
        /// Constructor to query a database using an existing <see cref="MySqlConnection"/> to initialize the <paramref name="connection"/>
        /// </summary>
        /// <param name="connection">An instance of <see cref="MySqlConnection"/> to use to query a database </param>
        public MySqlClient(MySqlConnection connection) : base(connection)
        {
        }
        #endregion
        #region Utility Methods
        #region Synchronous
        /// <summary>
        /// Gets an instance of <see cref="MySqlBulkLoader"/> using the current <see cref="MySqlConnection"/>
        /// </summary>
        /// <returns>Returns an instance of <see cref="MySqlConnection"/></returns>
        public MySqlBulkLoader GetBulkLoader()
        {
            //Return this back to the caller
            return new MySqlBulkLoader(this.Connection);
        }
        /// <summary>
        /// Determines whether the the current <see cref="MySqlConnection"/> to mysql server is valid
        /// </summary>
        /// <returns>Returns <c>true</c> if the connection to mysql server is valid</returns>
        public bool Ping()
        {
            //See if we can ping
            return this.Connection.Ping();
        }
        #endregion
        #region Asynchronous        
        /// <summary>
        /// Determines whether the the current <see cref="MySqlConnection"/> to mysql server is valid
        /// </summary>
        /// <returns>Returns <c>true</c> if the connection to mysql server is valid</returns>
        public async Task<bool> PingAsync()
        {
            //Return this back to the caller
            return await this.Connection.PingAsync();
        }
        /// <summary>
        /// Determines whether the the current <see cref="MySqlConnection"/> to mysql server is valid
        /// </summary>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns>Returns <c>true</c> if the connection to mysql server is valid</returns>
        public async Task<bool> PingAsync(CancellationToken token)
        {
            //Return this back to the caller
            return await this.Connection.PingAsync(token);
        }
        /// <summary>
        /// Begins the transaction asynchronously.
        /// </summary>
        /// <returns></returns>
        public async Task<MySqlTransaction> BeginTransactionAsync()
        {
            //Await this task
            return await this.Connection.BeginTransactionAsync();
        }
        /// <summary>
        /// Gets the transaction asynchronously.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <returns></returns>
        public async Task<MySqlTransaction> BeginTransactionAsync(IsolationLevel level)
        {
            //Await this task
            return await this.Connection.BeginTransactionAsync(level);
        }
        /// <summary>
        /// Begins the transaction asynchronously.
        /// </summary>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns></returns>
        public async Task<MySqlTransaction> BeginTransactionAsync(CancellationToken token)
        { 
            //Await this task
            return await this.Connection.BeginTransactionAsync(token);
        }
        /// <summary>
        /// Begins the transaction asynchronously.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns></returns>
        public async Task<MySqlTransaction> BeginTransactionAsync(IsolationLevel level, CancellationToken token)
        {
            //Await this task
            return await this.Connection.BeginTransactionAsync(level, token);
        }
        /// <summary>
        /// Changes the database in the current <see cref="MySqlConnection"/> context to a new database by <paramref name="name"/> asynchronously
        /// </summary>
        /// <param name="name">The name of the database the current connection will be changing to as a <see cref="string"/></param>
        /// <returns></returns>
        public async Task ChangeDatabaseAsync(string name)
        {
            //Await this task
            await this.Connection.ChangeDatabaseAsync(name);
        }
        /// <summary>
        /// Changes the database in the current <see cref="MySqlConnection"/> context to a new database by <paramref name="name"/> asynchronously
        /// </summary>
        /// <param name="name">The name of the database the current connection will be changing to as a <see cref="string"/></param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <returns></returns>
        public async Task ChangeDatabaseAsync(string name, CancellationToken token)
        {
            //Await this task
            await this.Connection.ChangeDatabaseAsync(name, token);
        }
        #endregion
        #endregion
    }
}
