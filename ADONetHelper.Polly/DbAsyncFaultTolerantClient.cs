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
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
#endregion

namespace ADONetHelper.Polly
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DbFaultTolerantClient : IAsyncFaultTolerantClient
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
                if (_asyncPolicyRegistry == null)
                {
                    _asyncPolicyRegistry = new PolicyRegistry();
                }

                return _asyncPolicyRegistry;
            }
        }
        #endregion
        #region Data Retrieval       
        /// <summary>
        /// Executes the get data reader asynchronous.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="behavior">The behavior.</param>
        /// <param name="transact">The transact.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task<DbDataReader> ExecuteGetDataReaderAsync(string query, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null, CancellationToken token = default)
        {
            return await ExecuteGetDataReaderAsync(query, DefaultAsyncPolicyKey, behavior, transact, token).ConfigureAwait(false);
        }
        /// <summary>
        /// Executes the get data reader asynchronous.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="policyName">Name of the policy.</param>
        /// <param name="behavior">The behavior.</param>
        /// <param name="transact">The transact.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task<DbDataReader> ExecuteGetDataReaderAsync(string query, string policyName, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null, CancellationToken token = default)
        {
            return await ExecuteGetDataReaderAsync(query, GetAsyncPolicy<IAsyncPolicy>(policyName), behavior, transact, token).ConfigureAwait(false);
        }
        /// <summary>
        /// Executes the get data reader asynchronous.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="policy">The policy.</param>
        /// <param name="behavior">The behavior.</param>
        /// <param name="transact">The transact.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task<DbDataReader> ExecuteGetDataReaderAsync(string query, IAsyncPolicy policy, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null, CancellationToken token = default)
        {
            return await policy.ExecuteAsync(async (token) => await ExecuteSQL.GetDbDataReaderAsync(QueryCommandType, query, token, behavior, transact), token).ConfigureAwait(false);
        }
        /// <summary>
        /// Captures the get data reader asynchronous.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="behavior">The behavior.</param>
        /// <param name="transact">The transact.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task<PolicyResult<DbDataReader>> CaptureGetDataReaderAsync(string query, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null, CancellationToken token = default)
        {
            return await CaptureGetDataReaderAsync(query, DefaultAsyncPolicyKey, behavior, transact, token).ConfigureAwait(false);
        }
        /// <summary>
        /// Captures the get data reader asynchronous.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="policyName">Name of the policy.</param>
        /// <param name="behavior">The behavior.</param>
        /// <param name="transact">The transact.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task<PolicyResult<DbDataReader>> CaptureGetDataReaderAsync(string query, string policyName, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null, CancellationToken token = default)
        {
            return await CaptureGetDataReaderAsync(query, GetAsyncPolicy<IAsyncPolicy>(policyName), behavior, transact, token).ConfigureAwait(false);
        }
        /// <summary>
        /// Captures the get data reader asynchronous.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="policy">The policy.</param>
        /// <param name="behavior">The behavior.</param>
        /// <param name="transact">The transact.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task<PolicyResult<DbDataReader>> CaptureGetDataReaderAsync(string query, IAsyncPolicy policy, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null, CancellationToken token = default)
        {
            return await policy.ExecuteAndCaptureAsync(async (token) => await ExecuteSQL.GetDbDataReaderAsync(QueryCommandType, query, token, behavior, transact), token).ConfigureAwait(false);
        }
        /// <summary>
        /// Executes the get data object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task<T> ExecuteGetDataObjectAsync<T>(string query, CancellationToken token = default) where T : class
        {
            return await ExecuteGetDataObjectAsync<T>(query, DefaultAsyncPolicyKey, token).ConfigureAwait(false);
        }
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
        /// <exception cref="ArgumentNullException">policy</exception>
        public async Task<T> ExecuteGetDataObjectAsync<T>(string query, IAsyncPolicy policy, CancellationToken token = default) where T : class
        {
            //Check for null 
            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }
            //Check if cancelled
            if (token.IsCancellationRequested == true)
            {
                token.ThrowIfCancellationRequested();
            }

            //Return this back to the caller
            return await policy.ExecuteAsync(async (token) => await ExecuteSQL.GetDataObjectAsync<T>(QueryCommandType, query, token), token).ConfigureAwait(false);
        }
        /// <summary>
        /// Captures the get data object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<PolicyResult<T>> CaptureGetDataObjectAsync<T>(string query, CancellationToken token = default) where T : class
        {
            return await CaptureGetDataObjectAsync<T>(query, DefaultAsyncPolicyKey, token).ConfigureAwait(false);
        }
        /// <summary>
        /// Captures the get data object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token"></param>
        /// <param name="query">The query.</param>
        /// <param name="policyName">Name of the policy.</param>
        /// <returns></returns>
        public async Task<PolicyResult<T>> CaptureGetDataObjectAsync<T>(string query, string policyName, CancellationToken token = default) where T : class
        {
            //Return this back to the caller
            return await CaptureGetDataObjectAsync<T>(query, GetAsyncPolicy<IAsyncPolicy>(policyName), token).ConfigureAwait(false);
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
        public async Task<PolicyResult<T>> CaptureGetDataObjectAsync<T>(string query, IAsyncPolicy policy, CancellationToken token = default) where T : class
        {
            //Check for null 
            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }
            //Check if cancelled
            if (token.IsCancellationRequested == true)
            {
                token.ThrowIfCancellationRequested();
            }

            //Return this back to the caller
            return await policy.ExecuteAndCaptureAsync(async () => await ExecuteSQL.GetDataObjectAsync<T>(QueryCommandType, query, token)).ConfigureAwait(false);
        }
        /// <summary>
        /// Executes the get data object enumerable asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task<List<T>> ExecuteGetDataObjectEnumerableAsync<T>(string query, CancellationToken token = default) where T : class
        {
            return await ExecuteGetDataObjectEnumerableAsync<T>(query, DefaultAsyncPolicyKey, token).ConfigureAwait(false);
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
        /// <exception cref="ArgumentNullException">policy</exception>
        public async Task<List<T>> ExecuteGetDataObjectEnumerableAsync<T>(string query, IAsyncPolicy policy, CancellationToken token = default) where T : class
        {
            //Check for null 
            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }
            //Check if cancelled
            if (token.IsCancellationRequested == true)
            {
                token.ThrowIfCancellationRequested();
            }

            //Return this back to the caller
            return await policy.ExecuteAsync(async (token) => await ExecuteSQL.GetDataObjectListAsync<T>(QueryCommandType, query, token), token).ConfigureAwait(false);
        }
        /// <summary>
        /// Captures the get data object enumerable asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task<PolicyResult<List<T>>> CaptureGetDataObjectEnumerableAsync<T>(string query, CancellationToken token = default) where T : class
        {
            return await CaptureGetDataObjectEnumerableAsync<T>(query, DefaultAsyncPolicyKey, token).ConfigureAwait(false);
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
        /// <exception cref="ArgumentNullException">policy</exception>
        public async Task<PolicyResult<List<T>>> CaptureGetDataObjectEnumerableAsync<T>(string query, IAsyncPolicy policy, CancellationToken token = default) where T : class
        {
            //Check for null 
            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }
            //Check if cancelled
            if (token.IsCancellationRequested == true)
            {
                token.ThrowIfCancellationRequested();
            }

            //Return this back to the caller
            return await policy.ExecuteAndCaptureAsync(async (token) => await ExecuteSQL.GetDataObjectListAsync<T>(QueryCommandType, query, token), token).ConfigureAwait(false);
        }
        #endregion
        #region Data Modification
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="Task{Int32}"/></returns>
        public async Task<int> ExecuteNonQueryAsync(string query, CancellationToken token = default)
        {
            return await ExecuteNonQueryAsync(query, DefaultAsyncPolicyKey, token).ConfigureAwait(false);
        }
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="policyName"></param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="Task{Int32}"/></returns>
        public async Task<int> ExecuteNonQueryAsync(string query, string policyName, CancellationToken token = default)
        {
            //Return this back to the caller
            return await ExecuteNonQueryAsync(query, GetAsyncPolicy<IAsyncPolicy>(policyName), token).ConfigureAwait(false);
        }
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="Task{Int32}"/></returns>
        public async Task<int> ExecuteNonQueryAsync(string query, IAsyncPolicy policy, CancellationToken token = default)
        {
            //Check for null 
            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }
            //Check if cancelled
            if (token.IsCancellationRequested == true)
            {
                token.ThrowIfCancellationRequested();
            }

            //Return this back to the caller
            return await policy.ExecuteAsync(async (token) => await ExecuteSQL.ExecuteNonQueryAsync(QueryCommandType, query, token), token).ConfigureAwait(false);
        }
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="Task{Int32}"/></returns>
        public async Task<PolicyResult<int>> CaptureNonQueryAsync(string query, CancellationToken token = default)
        {
            return await CaptureNonQueryAsync(query, DefaultAsyncPolicyKey, token).ConfigureAwait(false);
        }
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="policyName"></param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="Task{Int32}"/></returns>
        public async Task<PolicyResult<int>> CaptureNonQueryAsync(string query, string policyName, CancellationToken token = default)
        {
            //Return this back to the caller
            return await CaptureNonQueryAsync(query, GetAsyncPolicy<IAsyncPolicy>(policyName), token).ConfigureAwait(false);
        }
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="Task{Int32}"/></returns>
        public async Task<PolicyResult<int>> CaptureNonQueryAsync(string query, IAsyncPolicy policy, CancellationToken token = default)
        {
            //Check for null 
            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }
            //Check if cancelled
            if (token.IsCancellationRequested == true)
            {
                token.ThrowIfCancellationRequested();
            }

            //Return this back to the caller
            return await policy.ExecuteAndCaptureAsync(async (token) => await ExecuteSQL.ExecuteNonQueryAsync(QueryCommandType, query, token), token).ConfigureAwait(false);
        }
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
        /// Adds the asynchronous policy range.
        /// </summary>
        /// <param name="policies">The policies.</param>
        public void AddAsyncPolicyRange(IEnumerable<IAsyncPolicy> policies)
        {
            bool noNamePolicies = (policies.Where(x => string.IsNullOrWhiteSpace(x.PolicyKey) == true).Count() > 0);

            //Check that we had policies with names
            if(noNamePolicies == true)
            {
                throw new ArgumentException($"{nameof(policies)} had policies with no keys");
            }

            //Loop through each and see if there's a key
            foreach(IAsyncPolicy policy in policies)
            {
                AsyncRegistry.Add(policy.PolicyKey, policy);
            }
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
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task ExecuteOpenAsync(CancellationToken token = default)
        {
            await ExecuteOpenAsync(GetAsyncPolicy<IAsyncPolicy>(DefaultAsyncPolicyKey), token).ConfigureAwait(false);
        }
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
            //Check for null 
            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }
            //Check if cancelled
            if (token.IsCancellationRequested == true)
            {
                token.ThrowIfCancellationRequested();
            }
            if (ExecuteSQL.Connection.State == ConnectionState.Closed)
            {
                ExecuteSQL.Connection.ConnectionString = ConnectionString;
            }

            await policy.ExecuteAsync(async (token) => await ExecuteSQL.Connection.OpenAsync(token), token).ConfigureAwait(false);
        }
        /// <summary>
        /// Opens the asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task<PolicyResult> CaptureOpenAsync(CancellationToken token = default)
        {
            return await CaptureOpenAsync(DefaultAsyncPolicyKey, token).ConfigureAwait(false);
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
            //Check for null 
            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }
            //Check if cancelled
            if (token.IsCancellationRequested == true)
            {
                token.ThrowIfCancellationRequested();
            }

            //Return thsi back to the caller
            return await policy.ExecuteAndCaptureAsync(async (token) => await ExecuteSQL.Connection.OpenAsync(token), token).ConfigureAwait(false);
        }
        #endregion
    }
};