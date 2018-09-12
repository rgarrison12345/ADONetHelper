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
using System.Data.Odbc;
#endregion

namespace ADONetHelper.Odbc
{
    /// <summary>
    /// A specialized instance of <see cref="DbClient"/> that is used to query a relational database using ODBC technology
    /// </summary>
    /// <seealso cref="DbClient"/>
    public class OdbcClient : DbClient
    {
        #region Constructors
        /// <summary>
        /// The overloaded constuctor that will initialize the <paramref name="connectionString"/>, And <paramref name="queryCommandType"/>
        /// </summary>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        public OdbcClient(string connectionString, CommandType queryCommandType) : base(connectionString, queryCommandType, OdbcFactory.Instance)
        {
        }
        /// <summary>
        /// The overloaded constuctor that will initialize the <paramref name="connectionString"/>
        /// </summary>
        /// <param name="connectionString">The connection string used to query a data store</param>
        public OdbcClient(string connectionString) : base(connectionString, OdbcFactory.Instance)
        {
        }
        /// <summary>
        /// Intializes the <see cref="OdbcClient"/> with a <see cref="ISqlExecutor"/>
        /// </summary>
        /// <param name="executor">An instance of <see cref="ISqlExecutor"/></param>
        public OdbcClient(ISqlExecutor executor) : base(executor)
        {
        }
        /// <summary>
        /// Constructor to query a database using an existing <see cref="OdbcConnection"/> to initialize the <paramref name="connection"/>
        /// </summary>
        /// <param name="connection">An instance of <see cref="OdbcConnection"/> to use to query a database </param>
        public OdbcClient(OdbcConnection connection) : base(connection)
        {
        }
        /// <summary>
        /// Insantiates a new instance of <see cref="OdbcClient"/> using the passed in <paramref name="connectionString"/> and <paramref name="factory"/>
        /// </summary>
        /// <param name="connectionString">Connection string to use to query a database</param>
        /// <param name="factory">An instance of <see cref="IDbObjectFactory"/></param>
        public OdbcClient(string connectionString, IDbObjectFactory factory) : base(connectionString, factory)
        {
        }
        #endregion
    }
}
