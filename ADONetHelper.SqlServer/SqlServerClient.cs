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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
#if !NET20 && !NET35 && !NET40
using System.Threading.Tasks;
#endif
using System.Xml;
#endregion

namespace ADONetHelper.SqlServer
{
    /// <summary>
    /// A specialized instance of <see cref="DbClient"/> that is used to query a SQL Server database system
    /// </summary>
    /// <seealso cref="DbClient"/>
    /// <seealso cref="IXMLExecutor"/>
    public class SqlServerClient : DbClient, IXMLExecutor
    {
        #region Fields/Properties
        /// <summary>
        /// An instance of <see cref="SqlConnection"/>
        /// </summary>
        /// <returns>Returns an instance of <see cref="SqlConnection"/></returns>
        private SqlConnection Connection
        {
            get
            {
                //Return this back to the caller
                return (SqlConnection)this.ExecuteSQL.Connection;
            }
        }
        /// <summary>
        /// Enables statistics gathering for the current connection when set to <c>true</c>
        /// </summary>
        /// <returns>Returns <c>true</c> if statistics are enabled, <c>false</c> otherwise</returns>
        public bool StatisticsEnabled
        {
            get
            {
                //Return this back to the caller
                return this.Connection.StatisticsEnabled;
            }
            set
            {
                this.Connection.StatisticsEnabled = value;
            }
        }
        /// <summary>
        /// The size in bytes of network packets used to communicate with an instance of sql server
        /// </summary>
        /// <returns></returns>
        public int PacketSize
        {
            get
            {
                //Return this back to the caller
                return this.Connection.PacketSize;
            }
        }
        /// <summary>
        /// Gets a string that identifies the database client.
        /// </summary>
        /// <returns>Gets a string that identifies the database client.</returns>
        public string WorkstationID
        {
            get
            {
                //Return this back to the caller
                return this.Connection.WorkstationId;
            }
        }
#if NET46 || NET461
        /// <summary>
        /// Gets or sets the access token for the connection
        /// </summary>
        /// <returns>The access token as a <see cref="string"/></returns>
        public string AccessToken
        {
            get
            {
                //Return this back to the caller
                return this.Connection.AccessToken;
            }
            set
            {
                this.Connection.AccessToken = value;
            }
        }
#endif
#if !NET20 && !NET35 && !NET40
        /// <summary>
        /// The connection ID of the most recent connection attempt, regardless of whether the attempt succeeded or failed.
        /// </summary>
        /// <returns>The connection ID of the most recent connection attempt, regardless of whether the attempt succeeded or failed as a <c>string</c></returns>
        public Guid ClientConnectionID
        {
            get
            {
                //Return this back to the caller
                return this.Connection.ClientConnectionId;
            }
        }
#endif
        #endregion
        #region Constructors
        /// <summary>
        /// Intializes the <see cref="SqlServerClient"/> with a <see cref="ISqlExecutor"/>
        /// </summary>
        /// <param name="executor">An instance of <see cref="ISqlExecutor"/></param>
        public SqlServerClient(ISqlExecutor executor) : base(executor)
        {
        }
        /// <summary>
        /// The overloaded constuctor that will initialize the <paramref name="connectionString"/>, And <paramref name="queryCommandType"/>
        /// </summary>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        public SqlServerClient(string connectionString, CommandType queryCommandType) : base(connectionString, queryCommandType, SqlClientFactory.Instance)
        {
        }
        /// <summary>
        /// The overloaded constuctor that will initialize the <paramref name="connectionString"/>
        /// </summary>
        /// <param name="connectionString">The connection string used to query a data store</param>
        public SqlServerClient(string connectionString) : base(connectionString, SqlClientFactory.Instance)
        {
        }
        /// <summary>
        /// Constructor to query a database using an existing <see cref="SqlConnection"/> to initialize the <paramref name="connection"/>
        /// </summary>
        /// <param name="connection">An instance of <see cref="SqlConnection"/> to use to connect to a server and database</param>
        public SqlServerClient(SqlConnection connection) : base(connection)
        {
        }
        #endregion
        #region Utility Methods        
        /// <summary>
        /// Empties the connection pool associated with this instance of <see cref="SqlServerClient"/> <see cref="SqlConnection"/>
        /// </summary>
        /// <remarks>
        /// ClearPool clears the connection pool that is associated with the current <see cref="SqlConnection"/>. If additional connections associated with connection are in use at the time of the call, they are marked appropriately and are discarded (instead of being returned to the pool) when Close is called on them.</remarks>
        public void ClearPool()
        {
            //Clear the current pool
            SqlConnection.ClearPool(this.Connection);
        }
        /// <summary>
        /// Returns an instance of <see cref="XmlReader"/> based on the <paramref name="query"/>
        /// </summary>
        /// <param name="query">The query command text Or name of stored procedure to execute against the data store</param>
        /// <returns>Returns an instance of <see cref="XmlReader"/> based on the <paramref name="query"/> passed into the routine</returns>
        public XmlReader ExecuteXMLReader(string query)
        {
            //Wrap this in a using statement to automatically dispose of resources
            using (SqlCommand command = (SqlCommand)this.ExecuteSQL.Factory.GetDbCommand(this.QueryCommandType, query, this.ExecuteSQL.Parameters, this.Connection, this.CommandTimeout))
            {
                try
                {
                    //Return this back to the caller
                    return command.ExecuteXmlReader();
                }
                finally
                {
                    command.Parameters.Clear();
                }
            }
        }
#if !NET20 && !NET35 && !NET40
        /// <summary>
        /// Returns an instance of <see cref="XmlReader"/> based on the <paramref name="query"/>
        /// </summary>
        /// <param name="query">The query command text Or name of stored procedure to execute against the data store</param>
        /// <returns>Returns an instance of <see cref="XmlReader"/> based on the <paramref name="query"/> passed into the routine</returns>
        public async Task<XmlReader> ExecuteXMLReaderAsync(string query)
        {
            //Wrap this in a using statement to automatically dispose of resources
            using (SqlCommand command = (SqlCommand)this.ExecuteSQL.Factory.GetDbCommand(this.QueryCommandType, query, this.ExecuteSQL.Parameters, this.Connection, this.CommandTimeout))
            {
                try
                {
                    //Return this back to the caller
                    return await command.ExecuteXmlReaderAsync();
                }
                finally
                {
                    //Clear parameters
                    command.Parameters.Clear();
                }
            }
        }
#endif
        /// <summary>
        /// All statistics are set to zero if <see cref="SqlConnection.StatisticsEnabled"/> is <c>true</c>
        /// </summary>
        public void ResetStatistics()
        {
            this.Connection.ResetStatistics();
        }
        /// <summary>
        /// Gets an instance of <see cref="SqlBulkCopy"/> based off of the existing <see cref="SqlConnection"/> being used by the instance
        /// </summary>
        /// <returns>Returns an instance of <see cref="SqlBulkCopy"/> for the client to configure</returns>
        public SqlBulkCopy GetSQLBulkCopy()
        {
            //Return this back to the caller
            return new SqlBulkCopy(this.Connection);
        }
        /// <summary>
        /// Gets an instance of <see cref="SqlBulkCopy"/> using the passed in <paramref name="connectionString"/>
        /// </summary>
        /// <param name="connectionString">The connection string to connect to the database as a <see cref="string"/></param>
        /// <returns>Returns an instance of <see cref="SqlBulkCopy"/></returns>
        public SqlBulkCopy GetSqlBulkCopy(string connectionString)
        {
            //Return this back to the caller
            return new SqlBulkCopy(connectionString);
        }
        /// <summary>
        /// Gets an instance of <see cref="SqlBulkCopy"/> using the passed in <paramref name="connectionString"/> and <paramref name="options"/>
        /// </summary>
        /// <param name="connectionString">The connection string to connect to the database as a <see cref="string"/></param>
        /// <param name="options">The <see cref="SqlBulkCopyOptions"/> to configure the <see cref="SqlBulkCopy"/></param>
        /// <returns>Returns an instance of <see cref="SqlBulkCopy"/></returns>
        public SqlBulkCopy GetSqlBulkCopy(string connectionString, SqlBulkCopyOptions options)
        {
            //Return this back to the caller
            return new SqlBulkCopy(connectionString, options);
        }
        /// <summary>
        /// Gets an instance of <see cref="SqlBulkCopy"/> based off of the existing <see cref="SqlConnection"/> being used by the instance
        /// </summary>
        /// <param name="options">The <see cref="SqlBulkCopyOptions"/> to configure the <see cref="SqlBulkCopy"/></param>
        /// <param name="transaction">An instance of <see cref="SqlTransaction"/></param>
        /// <returns>Returns an instance of <see cref="SqlBulkCopy"/></returns>
        public SqlBulkCopy GetSQLBulkCopy(SqlBulkCopyOptions options, SqlTransaction transaction)
        {
            //Return this back to the caller
            return new SqlBulkCopy(this.Connection, options, transaction);
        }
        /// <summary>
        /// Returns a name value pair collection of statistics at the point in time the method is called
        /// </summary>
        /// <returns>Returns a reference of <see cref="IDictionary{TKey, TValue}"/> of <see cref="DictionaryEntry"/></returns>
        /// <remarks>When this method is called, the values retrieved are those at the current point in time. 
        /// If you continue using the connection, the values are incorrect. You need to re-execute the method to obtain the most current values.
        /// </remarks>
        public IDictionary<string, object> GetConnectionStatisticts()
        {
            //Return this back to the caller
            return (IDictionary<string, object>)this.Connection.RetrieveStatistics();
        }
        #endregion
    }
}