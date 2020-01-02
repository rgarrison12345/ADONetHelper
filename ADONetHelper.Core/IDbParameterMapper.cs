#region Licenses
/*MIT License
Copyright(c) 2020
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
        #region Utility Methods
        /// <summary>
        /// Gets the <see cref="DbType"/> associated with the passed in <paramref name="parameterValue"/>
        /// </summary>
        /// <param name="parameterValue">The .NET value that will be mapped to a providers native data type</param>
        /// <returns>Returns a <see cref="DbType"/> value that describes the RDBMS type of passed in <paramref name="parameterValue"/></returns>
        DbType GetDbType(object parameterValue);
        #endregion
    }
}