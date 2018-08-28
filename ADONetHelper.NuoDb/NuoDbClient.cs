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
        public NuoDbClient(string connectionString, CommandType queryType) : base(connectionString,queryType, NuoDbProviderFactory.Instance)
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
        #endregion
    }
}
