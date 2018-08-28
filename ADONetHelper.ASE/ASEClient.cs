#region Using Statements
using AdoNetCore.AseClient;
using System.Data;
#endregion

namespace ADONetHelper.ASE
{
    /// <summary>
    /// A specialized instance of <see cref="DbClient"/> that is used to query a SAP database system
    /// </summary>
    /// <seealso cref="DbClient"/>
    public sealed class ASEClient : DbClient
    {
        #region Constructors
        /// <summary>
        /// Intializes the <see cref="ASEClient"/> with an instance <see cref="ISqlExecutor"/>
        /// </summary>
        /// <param name="executor">An instance of <see cref="ISqlExecutor"/></param>
        public ASEClient(ISqlExecutor executor) : base(executor)
        {
        }
        /// <summary>
        /// Constructor to query a database using an existing <see cref="AseConnection"/> to initialize the <paramref name="connection"/>
        /// </summary>
        /// <param name="connection">An instance of <see cref="AseConnection"/> to use to connect to a server and database</param>
        public ASEClient(AseConnection connection) : base(connection)
        {
        
        }
        #endregion
    }
}
