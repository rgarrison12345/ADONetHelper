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
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
#endregion

namespace ADONetHelper.Core
{
    /// <summary>
    /// Contract class that defines how a class that can execute sql queries should return xml from the database
    /// </summary>
    public interface IXMLExecutor
    {
        /// <summary>
        /// Returns an instance of <see cref="XmlReader"/> based on the <paramref name="query"/>
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns an instance of <see cref="XmlReader"/> based on the <paramref name="query"/> passed into the routine</returns>
        XmlReader ExecuteXMLReader(string query);
        /// <summary>
        /// Returns an instance of <see cref="XmlReader"/> based on the <paramref name="query"/>
        /// </summary>
        /// <param name="token"></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns an instance of <see cref="XmlReader"/> based on the <paramref name="query"/> passed into the routine</returns>
        Task<XmlReader> ExecuteXMLReaderAsync(string query, CancellationToken token = default);
    }
}
