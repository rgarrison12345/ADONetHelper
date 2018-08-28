#region Using Statements
using System.Reflection;
using System.Text;
#endregion

namespace ADONetHelper
{
    /// <summary>
    /// Utility class that builds out queries to be exectued against a database
    /// </summary>
    public static class QueryBuilder
    {
        /// <summary>
        /// Creates a parameterized insert statement
        /// </summary>
        /// <param name="variableBinder">The binding symbol for variables and parameters for a specific RDBMS, defaults to @</param>
        /// <returns>Returns an ANSI standard insert statement as a <see cref="string"/></returns>
        public static string CreateInsertStatement<T>(string variableBinder = "@")
        {
            StringBuilder sqlString = new StringBuilder();
          
            //Return this back to the caller
            return sqlString.ToString();
        }
        /// <summary>
        /// Creates a parameterized update statement
        /// </summary>
        /// <param name="variableBinder">The binding symbol for variables and parameters for a specific RDBMS, defaults to @</param>
        /// <returns>Returns an ANSI standard update statement as a <see cref="string"/></returns>
        public static string CreateUpdateStatement<T>(string variableBinder = "@")
        {
            StringBuilder sqlString = new StringBuilder();


            //Retur nthis back to the caller
            return sqlString.ToString();
        }
    }
}
