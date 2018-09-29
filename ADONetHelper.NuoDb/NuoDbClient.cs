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
using NuoDb.Data.Client;
#endregion

namespace ADONetHelper.NuoDb
{
    /// <summary>
    /// A specialized instance of <see cref="DbClient"/> that is used to query a NuoDb database system
    /// </summary>
    /// <seealso cref="DbClient"/>
    public class NuoDbClient : DbClient
    {
        #region Fields/Properties
        #endregion
        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="NuoDbClient"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public NuoDbClient(string connectionString) : base(connectionString, NuoDbProviderFactory.Instance)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="NuoDbClient"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="queryType">Type of the query.</param>
        public NuoDbClient(string connectionString, CommandType queryType) : base(connectionString, queryType, NuoDbProviderFactory.Instance)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="NuoDbClient"/> class.
        /// </summary>
        /// <param name="executor">An instance of <see cref="T:ADONetHelper.ISqlExecutor" /></param>
        public NuoDbClient(ISqlExecutor executor) : base(executor)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="NuoDbClient"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public NuoDbClient(NuoDbConnection connection) : base(connection)
        {
        }
        /// <summary>
        /// Insantiates a new instance of <see cref="NuoDbClient"/> using the passed in <paramref name="connectionString"/> and <paramref name="factory"/>
        /// </summary>
        /// <param name="connectionString">Connection string to use to query a database</param>
        /// <param name="factory">An instance of <see cref="IDbObjectFactory"/></param>
        public NuoDbClient(string connectionString, IDbObjectFactory factory) : base(connectionString, factory)
        {
        }
        #endregion
    }
}
