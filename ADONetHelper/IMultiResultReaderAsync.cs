#region Licenses
/*MIT License
Copyright(c) 2019
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
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endregion

namespace ADONetHelper
{
    /// <summary>
    /// Contract class for a reader that performs asynchronous read operations against a database
    /// </summary>
    public interface IMultiResultReaderAsync
    {
        #region Utility Methods
        /// <summary>
        /// Reads the object asynchronously.
        /// </summary>
        /// <param name="token"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IAsyncEnumerable<T> ReadObjectEnumerableAsync<T>(CancellationToken token = default);
        /// <summary>
        /// Reads the object list.
        /// </summary>
        /// <param name="token"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<List<T>> ReadObjectListAsync<T>(CancellationToken token = default);
        /// <summary>
        /// Reads the object.
        /// </summary>
        /// <param name="token"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> ReadObjectAsync<T>(CancellationToken token = default);
        /// <summary>
        /// Moves to next result.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<bool> MoveToNextResultAsync(CancellationToken token = default);
        #endregion
    }
}