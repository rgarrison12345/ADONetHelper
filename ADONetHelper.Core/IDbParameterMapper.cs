#region Using Statements
using System.Data;
#endregion

namespace ADONetHelper.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDbParameterMapper
    {
        /// <summary>
        /// Gets the type of the database.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        DbType GetDbType(object value);
    }
}