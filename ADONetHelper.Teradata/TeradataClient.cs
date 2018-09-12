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
using Teradata.Client.Provider;
#endregion

namespace ADONetHelper.TeraData
{
    /// <summary>
    /// A specialized instance of <see cref="DbClient"/> to target Terdata system
    /// </summary>
    /// <seealso cref="ADONetHelper.DbClient" />
    public class TeradataClient : DbClient
    {
        #region Fields/Properties        
        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        private TdConnection Connection
        {
            get
            {
                //Return this back to the caller
                return (TdConnection)this.ExecuteSQL.Connection;
            }
        }
        /// <summary>
        /// Gets the query band object that defines the query bands at the connection level
        /// </summary>
        /// <value>
        /// The query band.
        /// </value>
        public TdQueryBand QueryBand
        {
            get
            {
                //Return this back to the caller
                return this.Connection.QueryBand;
            }
        }
        #endregion
        #region Constructors
        /// <summary>
        /// The overloaded constuctor that will initialize the <paramref name="connectionString"/>, And <paramref name="queryCommandType"/>
        /// </summary>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        public TeradataClient(string connectionString, CommandType queryCommandType) : base(connectionString, queryCommandType, TdFactory.Instance)
        {
        }
        /// <summary>
        /// The overloaded constuctor that will initialize the <paramref name="connectionString"/>
        /// </summary>
        /// <param name="connectionString">The connection string used to query a data store</param>
        public TeradataClient(string connectionString) : base(connectionString, TdFactory.Instance)
        {
        }
        /// <summary>
        /// Intializes the <see cref="TeradataClient"/> with a <see cref="ISqlExecutor"/>
        /// </summary>
        /// <param name="executor">An instance of <see cref="ISqlExecutor"/></param>
        public TeradataClient(ISqlExecutor executor) : base(executor)
        {
        }
        /// <summary>
        /// Constructor to query a database using an existing <see cref="TdConnection"/> to initialize the <paramref name="connection"/>
        /// </summary>
        /// <param name="connection">An instance of <see cref="TdConnection"/> to use to query a database </param>
        public TeradataClient(TdConnection connection) : base(connection)
        {
        }
        /// <summary>
        /// Insantiates a new instance of <see cref="TeradataClient"/> using the passed in <paramref name="connectionString"/> and <paramref name="factory"/>
        /// </summary>
        /// <param name="connectionString">Connection string to use to query a database</param>
        /// <param name="factory">An instance of <see cref="IDbObjectFactory"/></param>
        public TeradataClient(string connectionString, IDbObjectFactory factory) : base(connectionString, factory)
        {
        }
        #endregion
        #region Utility Methods        
        /// <summary>
        /// Changes the query band.
        /// </summary>
        /// <param name="band">An instance of <see cref="TdQueryBand"/></param>
        public void ChangeQueryBand(TdQueryBand band)
        {
            //Change the query band
            this.Connection.ChangeQueryBand(band);
        }
        #endregion
    }
}
