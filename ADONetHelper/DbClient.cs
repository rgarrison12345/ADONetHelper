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
using ADONetHelper.Core;
using System.Data;
using System.Data.Common;
#endregion

namespace ADONetHelper
{
    /// <summary>
    /// Represents the base class for all <see cref="DbClient"/> classes
    /// </summary>
    /// <seealso cref="DbProvider"/>
    /// <seealso cref="ISynchronousClient"/>
#if !NET20 && !NET35 && !NET40
    /// <seealso cref="IAsynchronousClient"/>
#endif
    /// <remarks>
    /// DbClient is a utility class that encompasses both a <see cref="DbConnection"/> and a <see cref="DbCommand"/>
    /// to help query a database with minimal coding to focus on the users SQL
    /// </remarks>
    public partial class DbClient : DbProvider
    {
        #region Variables
        /// <summary>
        /// Variable To detect redundant calls of dispose
        /// </summary>
        protected bool disposedValue = false;
        #endregion
        #region Constructors
#if !NETSTANDARD1_3 && !NETSTANDARD2_0
        /// <summary>
        /// Instantiates a new instance of <see cref="DbClient"/> with the passed in <paramref name="row"/> and <paramref name="connectionString"/>
        /// </summary>
        /// <param name="row">An instance of <see cref="DataRow"/> that contains the information to configure a <see cref="DbProviderFactory"/></param>
        /// <param name="connectionString">The connection string to be used to query a data store</param>
        public DbClient(DataRow row, string connectionString) : base(row, connectionString)
        {
        }
#endif
        /// <summary>
        /// Instantiates a new instance of <see cref="DbClient"/> with an instance of <see cref="ISqlExecutor"/>
        /// </summary>
        /// <param name="executor">An instance of <see cref="ISqlExecutor"/></param>
        public DbClient(ISqlExecutor executor) : base(executor)
        {
        }
        /// <summary>
        /// Instantiates a new instance of <see cref="DbClient"/> with the passed in <paramref name="connectionString"/>, and <paramref name="queryCommandType"/>, and <paramref name="factory"/>
        /// </summary>
        /// <param name="factory">An instance of a <see cref="DbProviderFactory"/> client class</param>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        public DbClient(string connectionString, CommandType queryCommandType, DbProviderFactory factory) : base(connectionString, queryCommandType, factory)
        {
        }
        /// <summary>
        /// Instantiates a new instance of <see cref="DbClient"/> with the passed in <paramref name="connectionString"/> and <paramref name="factory"/>
        /// </summary>
        /// <param name="factory">An instance of the <see cref="DbProviderFactory"/> client class</param>
        /// <param name="connectionString">The connection string used to query a data store</param>
        public DbClient(string connectionString, DbProviderFactory factory) : base(connectionString, factory)
        {
        }
        /// <summary>
        /// Instantiates a new instance of <see cref="DbClient"/> with the passed in with the passed in <paramref name="connectionString"/>, <paramref name="providerName"/>, and <paramref name="queryCommandType"/>
        /// </summary>
        /// <param name="providerName">The name of the data provider that the should be used to query a data store</param>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        public DbClient(string connectionString, string providerName, CommandType queryCommandType) : base(connectionString, providerName, queryCommandType)
        {
        }
        /// <summary>
        /// Instantiates a new instance of <see cref="DbClient"/> with the passed in <paramref name="connectionString"/> and <paramref name="providerName"/>
        /// </summary>
        /// <param name="providerName">The name of the data provider that the should be used to query a data store</param>
        /// <param name="connectionString">The connection string used to query a data store</param>
        public DbClient(string connectionString, string providerName) : base(connectionString, providerName)
        {
        }
        /// <summary>
        /// Instantiates a new instance of <see cref="DbClient"/> with the passed in <paramref name="providerName"/>
        /// </summary>
        /// <param name="providerName">The name of the data provider that the should be used to query a data store</param>
        public DbClient(string providerName) : base(providerName)
        {
        }
        /// <summary>
        /// Instantiates a new instance of <see cref="DbClient"/> using an existing <see cref="DbConnection"/> to initialize the <paramref name="connection"/>
        /// </summary>
        /// <param name="connection">An instance of <see cref="DbConnection"/> to use to query a database </param>
        public DbClient(DbConnection connection) : base(connection)
        {
        }
        /// <summary>
        /// Instantiates a new instance of <see cref="DbClient"/> using the passed in <paramref name="connectionString"/> and <paramref name="factory"/>
        /// </summary>
        /// <param name="connectionString">The connection string used to query a database</param>
        /// <param name="factory">An instance of <see cref="IDbObjectFactory"/> to create the objects needed to help query a database</param>
        public DbClient(string connectionString, IDbObjectFactory factory) : base(connectionString, factory)
        {
        }
        #endregion
    }
}