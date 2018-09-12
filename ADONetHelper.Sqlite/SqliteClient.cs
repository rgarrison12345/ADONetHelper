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
using System.Data;
using System.Data.SQLite;
#endregion

namespace ADONetHelper.Sqlite
{
    /// <summary>
    /// Helper class that works with a sqlite datastore
    /// </summary>
    /// <seealso cref="DbClient"/>
    public class SqliteClient : DbClient
    {
        #region Fields/Properties        
        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        protected SQLiteConnection Connection
        {
            get
            {
                //Return this back to the caller
                return (SQLiteConnection)this.ExecuteSQL.Connection;
            }
        }
        #endregion
        #region Constructors
        /// <summary>
        /// Instantiates a new instance of <see cref="SqliteClient"/> using the passed in <paramref name="executor"/>
        /// </summary>
        /// <param name="executor">An instance of <see cref="ISqlExecutor"/></param>
        public SqliteClient(ISqlExecutor executor) : base(executor)
        {
        }
        /// <summary>
        /// Instantiates a new instance of <see cref="SqliteClient"/> using the passed in <paramref name="connectionString"/> and <paramref name="queryCommandType"/>
        /// </summary>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        public SqliteClient(string connectionString, CommandType queryCommandType) : base(connectionString, queryCommandType, SQLiteFactory.Instance)
        {
        }
        /// <summary>
        /// Instantiates a new instance of <see cref="SqliteClient"/> using the passed in <paramref name="connectionString"/>
        /// </summary>
        /// <param name="connectionString">The connection string used to query a data store</param>
        public SqliteClient(string connectionString) : base(connectionString, SQLiteFactory.Instance)
        {
        }
        /// <summary>
        /// Instantiates a new instance of <see cref="SqliteClient"/> using the passed in <paramref name="connection"/>
        /// </summary>
        /// <param name="connection">An instance of <see cref="SQLiteConnection"/> to use to query a database</param>
        public SqliteClient(SQLiteConnection connection) : base(connection)
        {
        }
        /// <summary>
        /// Instantiates a new instance of <see cref="SqliteClient"/> using the passed in <paramref name="connectionString"/> and <paramref name="factory"/>
        /// </summary>
        /// <param name="connectionString">Connection string to use to query a database</param>
        /// <param name="factory">An instance of <see cref="IDbObjectFactory"/></param>
        public SqliteClient(string connectionString, IDbObjectFactory factory) : base(connectionString, factory)
        {
        }
        #endregion
        #region Utility Methods        
        /// <summary>
        /// Verifies all queries within the current command context can be compiled
        /// </summary>
        /// <param name="query">The query.</param>
        /// <exception cref="SQLiteException">Thrown if any error occurs during compilation of the query</exception>
        public void VerifyOnly(string query)
        {
            //Wrap this in a using statement to automatically dispose of resources
            using (SQLiteCommand command = new SQLiteCommand(query, this.Connection))
            {
                //Add in any parameters
                command.Parameters.AddRange(this.ExecuteSQL.Parameters.ToArray());
                command.VerifyOnly();

                //Clear the parameters
                command.Parameters.Clear();
            }
        }
        #endregion
    }
}
