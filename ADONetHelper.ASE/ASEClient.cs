#region Using Statements
using AdoNetCore.AseClient;
using System.Data;
#endregion

namespace ADONetHelper.ASE
{
    /// <summary>
    /// A specialized instance of <see cref="DbClient"/> that is used to query an ASE database system
    /// </summary>
    /// <seealso cref="DbClient"/>
    public class ASEClient : DbClient
    {
        #region Constructors
#if NETCOREAPP2_1
        /// <summary>
        /// The overloaded constuctor that will initialize the <paramref name="connectionString"/>, And <paramref name="queryCommandType"/>
        /// </summary>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        public ASEClient(string connectionString, CommandType queryCommandType) : base(connectionString, queryCommandType, AseClientFactory.Instance)
        {

        }
        /// <summary>
        /// The overloaded constuctor that will initialize the <paramref name="connectionString"/>
        /// </summary>
        /// <param name="connectionString">The connection string used to query a data store</param>
        public ASEClient(string connectionString) : base(connectionString, AseClientFactory.Instance)
        {
        }
#endif
        /// <summary>
        /// Intializes the <see cref="ASEClient"/> with a <see cref="ISqlExecutor"/>
        /// </summary>
        /// <param name="executor">An instance of <see cref="ISqlExecutor"/></param>
        public ASEClient(ISqlExecutor executor) : base(executor)
        {
        }
        /// <summary>
        /// Constructor to query a database using an existing <see cref="AseConnection"/> to initialize the <paramref name="connection"/>
        /// </summary>
        /// <param name="connection">An instance of <see cref="AseConnection"/> to use to query a database </param>
        public ASEClient(AseConnection connection) : base(connection)
        {
        
        }
        #endregion
    }
}
