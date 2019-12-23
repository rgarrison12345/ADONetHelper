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
using ADONetHelper.Core;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
#endregion

namespace ADONetHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="IMultiResultReaderAsync"/>
    /// <seealso cref="IMultiResultReaderSync"/>
    /// <seealso cref="IDisposable"/>
    public class MultiResultReader : IMultiResultReaderAsync, IMultiResultReaderSync, IDisposable
#if NETSTANDARD2_1
        //, IAsyncDisposable
#endif
    {
        #region Variables
        private bool disposedValue = false; // To detect redundant calls
        #endregion
        #region Fields/Properties
        private readonly DbDataReader _reader = null;
        #endregion
        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiResultReader"/> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public MultiResultReader(DbDataReader reader)
        {
            _reader = reader;
        }
        #endregion
        #region Async Methods
        /// <summary>
        /// Reads the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<List<T>> ReadObjectListAsync<T>(CancellationToken token = default)
        {
            //Keep looping through each object in enumerator
            return Utilities.GetDynamicTypeList<T>(await Utilities.GetDynamicResultsListAsync(_reader, token).ConfigureAwait(false));
        }
        /// <summary>
        /// Reads the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async IAsyncEnumerable<T> ReadObjectEnumerableAsync<T>([EnumeratorCancellation] CancellationToken token = default)
        {
            //Keep looping through each object in enumerator
            await foreach (IDictionary<string, object> dict in Utilities.GetDynamicResultsEnumerableAsync(_reader, token))
            {
                //Keep yielding results
                yield return Utilities.GetSingleDynamicType<T>(dict);
            }
        }
        /// <summary>
        /// Reads the object asynchronous.
        /// </summary>
        /// <param name="token"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> ReadObjectAsync<T>(CancellationToken token = default)
        {
            return Utilities.GetSingleDynamicType<T>(await Utilities.GetDynamicResultAsync(_reader, token).ConfigureAwait(false));
        }
        /// <summary>
        /// Moves the next to result asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task<bool> MoveToNextResultAsync(CancellationToken token = default)
        {
            //Move to next result set
            return await _reader.NextResultAsync(token).ConfigureAwait(false);
        }
        #endregion
        #region Sync Methods        
        /// <summary>
        /// Reads the object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> ReadObjectEnumerable<T>()
        {
            //Keep looping through each object in enumerator
            foreach (IDictionary<string, object> dict in Utilities.GetDynamicResultsEnumerable(_reader))
            {
                //Keep yielding results
                yield return Utilities.GetSingleDynamicType<T>(dict);
            }
        }
        /// <summary>
        /// Reads the object list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> ReadObjectList<T>()
        {
            return Utilities.GetDynamicTypeList<T>(Utilities.GetDynamicResultsList(_reader));
        }
        /// <summary>
        /// Reads the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ReadObject<T>()
        {
            return Utilities.GetSingleDynamicType<T>(Utilities.GetDynamicResult(_reader));
        }
        /// <summary>
        /// Moves to next result.
        /// </summary>
        /// <returns></returns>
        public bool MoveToNextResult()
        {
            //Move to next result set
            return _reader.NextResult();
        }
        #endregion
        #region Helper Methods
        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {
            _reader.Close();
        }
#if NETSTANDARD2_1
        /// <summary>
        /// Closes the asynchronous.
        /// </summary>
        public async Task CloseAsync()
        {
            await _reader.CloseAsync().ConfigureAwait(false);
        }
#endif
        #endregion
        #region IDisposable Support            
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            //Check if we haven't disposed
            if (!disposedValue)
            {
                //Check if we are disposing of managed objects
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    Close();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~MultiResultReader()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
        #region Helper Methods
        #endregion
    }
}