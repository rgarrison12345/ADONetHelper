#region Using Statements
using Snowflake.Data.Client;
using System.Data;
#endregion

namespace ADONetHelper.Snowflake
{
    /// <summary>
    /// A specialized instance of <see cref="DbClient"/> used to connect to a snowflake data store
    /// </summary>
    /// <seealso cref="ADONetHelper.DbClient" />
    public class SnowflakeClient : DbClient
    {
        #region Constructors
        /// <summary>
        /// The overloaded constuctor that will initialize the <paramref name="connectionString"/>, And <paramref name="queryCommandType"/>
        /// </summary>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        //public SnowflakeClient(string connectionString, CommandType queryCommandType) : base(connectionString, queryCommandType, NpgsqlFactory.Instance)
        //{

        //}
        ///// <summary>
        ///// The overloaded constuctor that will initialize the <paramref name="connectionString"/>
        ///// </summary>
        ///// <param name="connectionString">The connection string used to query a data store</param>
        //public SnowflakeClient(string connectionString) : base(connectionString, null)
        //{
        //}
        /// <summary>
        /// Intializes the <see cref="SnowflakeClient"/> with a <see cref="ISqlExecutor"/>
        /// </summary>
        /// <param name="executor">An instance of <see cref="ISqlExecutor"/></param>
        public SnowflakeClient(ISqlExecutor executor) : base(executor)
        {
        }
        /// <summary>
        /// Insantiates a new instance of <see cref="SnowflakeClient"/> using the passed in <paramref name="connectionString"/> and <paramref name="factory"/>
        /// </summary>
        /// <param name="connectionString">Connection string to use to query a database</param>
        /// <param name="factory">An instance of <see cref="IDbObjectFactory"/></param>
        public SnowflakeClient(string connectionString, IDbObjectFactory factory) : base(connectionString, factory)
        {
        }
        /// <summary>
        /// Constructor to query a database using an existing <see cref="SnowflakeDbConnection"/> to initialize the <paramref name="connection"/>
        /// </summary>
        /// <param name="connection">An instance of <see cref="SnowflakeDbConnection"/> to use to query a database </param>
        public SnowflakeClient(SnowflakeDbConnection connection) : base(connection)
        {
        }
        #endregion
    }
}
