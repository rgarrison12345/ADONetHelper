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
using Polly;
using Polly.Registry;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
#endregion

namespace ADONetHelper.Polly
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DbFaultTolerantClient
    {
        #region Fields/Properties
        private PolicyRegistry _asyncPolicyRegistry;

        /// <summary>
        /// Gets the asynchronous registry.
        /// </summary>
        /// <value>
        /// The asynchronous registry.
        /// </value>
        private PolicyRegistry AsyncRegistry
        {
            get
            {
                //Check for null, might need to instantiate a new instance
                if(_asyncPolicyRegistry == null)
                {
                    _asyncPolicyRegistry = new PolicyRegistry();
                }

                return _asyncPolicyRegistry;
            }
        }
        #endregion
        #region Data Retrieval             
        /// <summary>
        /// Executes the get data object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="policyName">Name of the policy.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task<T> ExecuteGetDataObjectAsync<T>(string query, string policyName, CancellationToken token = default) where T : class
        {
            //Return this back to the caller
            return await ExecuteGetDataObjectAsync<T>(query, GetAsyncPolicy<IAsyncPolicy>(policyName), token).ConfigureAwait(false);
        }
        /// <summary>
        /// Executes the get data object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="policy">The policy.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">policy</exception>
        public async Task<T> ExecuteGetDataObjectAsync<T>(string query, IAsyncPolicy policy, CancellationToken token = default) where T : class
        {
            //Check for null 
            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }

            //Return this back to the caller
            return await policy.ExecuteAsync(async () => await ExecuteSQL.GetDataObjectAsync<T>(QueryCommandType, query, token)).ConfigureAwait(false);
        }
        /// <summary>
        /// Captures the get data object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="policyName">Name of the policy.</param>
        /// <returns></returns>
        public async Task<PolicyResult<T>> CaptureGetDataObjectAsync<T>(string query, string policyName) where T : class
        {
            //Return this back to the caller
            return await CaptureGetDataObjectAsync<T>(query, GetAsyncPolicy<IAsyncPolicy>(policyName)).ConfigureAwait(false);
        }
        /// <summary>
        /// Captures the get data object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="policy">The policy.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">policy</exception>
        public async Task<PolicyResult<T>> CaptureGetDataObjectAsync<T>(string query, IAsyncPolicy policy,CancellationToken token = default) where T : class
        {
            //Check for null 
            if(policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }

            //Return this back to the caller
            return await policy.ExecuteAndCaptureAsync(async () => await ExecuteSQL.GetDataObjectAsync<T>(QueryCommandType, query, token)).ConfigureAwait(false);
        }
        /// <summary>
        /// Executes the get data object enumerable asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="policyName">Name of the policy.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task<List<T>> ExecuteGetDataObjectEnumerableAsync<T>(string query, string policyName, CancellationToken token = default) where T : class
        {
            //Return this back to the caller
            return await ExecuteGetDataObjectEnumerableAsync<T>(query, GetAsyncPolicy<IAsyncPolicy>(policyName), token).ConfigureAwait(false);
        }
        /// <summary>
        /// Executes the get data object enumerable asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="policy">The policy.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">policy</exception>
        public async Task<List<T>>ExecuteGetDataObjectEnumerableAsync<T>(string query, IAsyncPolicy policy, CancellationToken token = default) where T : class
        {
            //Check for null
            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }

            //Return this back to the caller
            return await policy.ExecuteAsync(async (token) => await ExecuteSQL.GetDataObjectListAsync<T>(QueryCommandType, query, token), token).ConfigureAwait(false);
        }
        /// <summary>
        /// Captures the get data object enumerable asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="policyName">Name of the policy.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task<PolicyResult<List<T>>> CaptureGetDataObjectEnumerableAsync<T>(string query, string policyName, CancellationToken token = default) where T : class
        {
            //Return this back to the caller
            return await CaptureGetDataObjectEnumerableAsync<T>(query, GetAsyncPolicy<IAsyncPolicy>(policyName), token).ConfigureAwait(false);
        }
        /// <summary>
        /// Captures the get data object enumerable asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="policy">The policy.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">policy</exception>
        public async Task<PolicyResult<List<T>>> CaptureGetDataObjectEnumerableAsync<T>(string query, IAsyncPolicy policy, CancellationToken token = default) where T : class
        {
            //Check for null 
            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }

            //Return this back to the caller
            return await policy.ExecuteAndCaptureAsync(async (token) => await ExecuteSQL.GetDataObjectListAsync<T>(QueryCommandType, query, token), token).ConfigureAwait(false);
        }
        #endregion
        #region Data Modification
        #endregion
        #region Utility Methods     
        /// <summary>
        /// Adds the asynchronous policy.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="policy">The policy.</param>
        public void AddAsyncPolicy(string key, IAsyncPolicy policy)
        {
            AsyncRegistry.Add(key, policy);
        }
        /// <summary>
        /// Gets the policy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        public IAsyncPolicy GetAsyncPolicy<T>(string keyName) where T : IAsyncPolicy
        {
            return AsyncRegistry.Get<T>(keyName);
        }
        /// <summary>
        /// Removes the asynchronous policy.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public bool RemoveAsyncPolicy(string key)
        {
            return AsyncRegistry.Remove(key);
        }
        #endregion
        #region Connection Methods   
        /// <summary>
        /// Opens the asynchronous.
        /// </summary>
        /// <param name="keyName">Name of the key.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task ExecuteOpenAsync(string keyName, CancellationToken token = default)
        {
            await ExecuteOpenAsync(GetAsyncPolicy<IAsyncPolicy>(keyName), token).ConfigureAwait(false);
        }
        /// <summary>
        /// Opens the asynchronous.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task ExecuteOpenAsync(IAsyncPolicy policy, CancellationToken token = default)
        { 
            if(ExecuteSQL.Connection.State == ConnectionState.Closed)
            {
                ExecuteSQL.Connection.ConnectionString = ConnectionString;
            }

            await policy.ExecuteAsync(async (token) => await ExecuteSQL.Connection.OpenAsync(token), token).ConfigureAwait(false);
        }
        /// <summary>
        /// Opens the asynchronous.
        /// </summary>
        /// <param name="keyName">Name of the key.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task<PolicyResult> CaptureOpenAsync(string keyName, CancellationToken token = default)
        {
            //Return this back to the caller
            return await CaptureOpenAsync(GetAsyncPolicy<IAsyncPolicy>(keyName), token).ConfigureAwait(false);
        }
        /// <summary>
        /// Opens the asynchronous.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task<PolicyResult> CaptureOpenAsync(IAsyncPolicy policy, CancellationToken token = default)
        {
            //Return thsi back to the caller
            return await policy.ExecuteAndCaptureAsync(async (token) => await ExecuteSQL.Connection.OpenAsync(token), token).ConfigureAwait(false);
        }
        #endregion
    }
}