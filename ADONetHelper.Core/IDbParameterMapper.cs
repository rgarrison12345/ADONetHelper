#region Using Statements
using System.Data;
#endregion

namespace ADONetHelper.Core
{
    /// <summary>
    /// Contract class the defines the behavior of a DbParameter mapper class
    /// </summary>
    public interface IDbParameterMapper
    {
        /// <summary>
        /// Gets the <see cref="DbType"/> associated with the passed in <paramref name="parameterValue"/>
        /// </summary>
        /// <param name="parameterValue">The .NET value that will be mapped to a providers native data type</param>
        /// <returns>Returns a <see cref="DbType"/> value that describes the RDBMS type of passed in <paramref name="parameterValue"/></returns>
        DbType GetDbType(object parameterValue);
    }
}