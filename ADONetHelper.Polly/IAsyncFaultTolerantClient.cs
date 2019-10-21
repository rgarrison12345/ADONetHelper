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
#region Using Statement
using Polly;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
#endregion

namespace ADONetHelper.Polly
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAsyncFaultTolerantClient
    {
        #region Data Retrieval     
        /// <summary>
        /// Executes the get data reader asynchronous.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="behavior">The behavior.</param>
        /// <param name="transact">The transact.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<DbDataReader> ExecuteGetDataReaderAsync(string query, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null, CancellationToken token = default);
        /// <summary>
        /// Executes the get data reader asynchronous.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="policyName">Name of the policy.</param>
        /// <param name="behavior">The behavior.</param>
        /// <param name="transact">The transact.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<DbDataReader> ExecuteGetDataReaderAsync(string query, string policyName, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null, CancellationToken token = default);
        /// <summary>
        /// Executes the get data reader asynchronous.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="policy">The policy.</param>
        /// <param name="behavior">The behavior.</param>
        /// <param name="transact">The transact.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<DbDataReader> ExecuteGetDataReaderAsync(string query, IAsyncPolicy policy, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null, CancellationToken token = default);
        /// <summary>
        /// Captures the get data reader asynchronous.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="behavior">The behavior.</param>
        /// <param name="transact">The transact.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<PolicyResult<DbDataReader>> CaptureGetDataReaderAsync(string query, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null, CancellationToken token = default);
        /// <summary>
        /// Captures the get data reader asynchronous.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="policyName">Name of the policy.</param>
        /// <param name="behavior">The behavior.</param>
        /// <param name="transact">The transact.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<PolicyResult<DbDataReader>> CaptureGetDataReaderAsync(string query, string policyName, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null, CancellationToken token = default);
        /// <summary>
        /// Captures the get data reader asynchronous.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="policy">The policy.</param>
        /// <param name="behavior">The behavior.</param>
        /// <param name="transact">The transact.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<PolicyResult<DbDataReader>> CaptureGetDataReaderAsync(string query, IAsyncPolicy policy, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null, CancellationToken token = default);
        /// <summary>
        /// Executes the get data object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<T> ExecuteGetDataObjectAsync<T>(string query, CancellationToken token = default) where T : class;
        /// <summary>
        /// Executes the get data object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="policyName">Name of the policy.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<T> ExecuteGetDataObjectAsync<T>(string query, string policyName, CancellationToken token = default) where T : class;
        /// <summary>
        /// Executes the get data object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="policy">The policy.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">policy</exception>
        Task<T> ExecuteGetDataObjectAsync<T>(string query, IAsyncPolicy policy, CancellationToken token = default) where T : class;
        /// <summary>
        /// Captures the get data object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<PolicyResult<T>> CaptureGetDataObjectAsync<T>(string query, CancellationToken token = default) where T : class;
        /// <summary>
        /// Captures the get data object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="policyName">Name of the policy.</param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<PolicyResult<T>> CaptureGetDataObjectAsync<T>(string query, string policyName, CancellationToken token = default) where T : class;
        /// <summary>
        /// Captures the get data object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="policy">The policy.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">policy</exception>
        Task<PolicyResult<T>> CaptureGetDataObjectAsync<T>(string query, IAsyncPolicy policy, CancellationToken token = default) where T : class;
        /// <summary>
        /// Captures the get data object enumerable asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<PolicyResult<List<T>>> CaptureGetDataObjectEnumerableAsync<T>(string query, CancellationToken token = default) where T : class;
        /// <summary>
        /// Executes the get data object enumerable asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="policyName">Name of the policy.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<List<T>> ExecuteGetDataObjectEnumerableAsync<T>(string query, string policyName, CancellationToken token = default) where T : class;
        /// Executes the get data object enumerable asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="policy">The policy.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">policy</exception>
        Task<List<T>> ExecuteGetDataObjectEnumerableAsync<T>(string query, IAsyncPolicy policy, CancellationToken token = default) where T : class;
        /// <summary>
        /// Captures the get data object enumerable asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="policyName">Name of the policy.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<PolicyResult<List<T>>> CaptureGetDataObjectEnumerableAsync<T>(string query, string policyName, CancellationToken token = default) where T : class;
        /// <summary>
        /// Captures the get data object enumerable asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="policy">The policy.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">policy</exception>
        Task<PolicyResult<List<T>>> CaptureGetDataObjectEnumerableAsync<T>(string query, IAsyncPolicy policy, CancellationToken token = default) where T : class;
        #endregion
        #region Data Modification
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="Task{Int32}"/></returns>
        Task<int> ExecuteNonQueryAsync(string query, CancellationToken token = default);
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="policyName"></param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="Task{Int32}"/></returns>
        Task<int> ExecuteNonQueryAsync(string query, string policyName, CancellationToken token = default);
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="Task{Int32}"/></returns>
        Task<int> ExecuteNonQueryAsync(string query, IAsyncPolicy policy, CancellationToken token = default);
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="Task{Int32}"/></returns>
        Task<PolicyResult<int>> CaptureNonQueryAsync(string query, CancellationToken token = default);
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="policyName"></param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="Task{Int32}"/></returns>
        Task<PolicyResult<int>> CaptureNonQueryAsync(string query, string policyName, CancellationToken token = default);
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="token">Structure that propogates a notification that an operation should be cancelled</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="Task{Int32}"/></returns>
        Task<PolicyResult<int>> CaptureNonQueryAsync(string query, IAsyncPolicy policy, CancellationToken token = default);
        #endregion
        #region Utility Methods     
        /// <summary>
        /// Adds the asynchronous policy.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="policy">The policy.</param>
        void AddAsyncPolicy(string key, IAsyncPolicy policy);
        /// <summary>
        /// Adds the asynchronous policy range.
        /// </summary>
        /// <param name="policies">The policies.</param>
        void AddAsyncPolicyRange(IEnumerable<IAsyncPolicy> policies);
        /// <summary>
        /// Gets the policy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        IAsyncPolicy GetAsyncPolicy<T>(string keyName) where T : IAsyncPolicy;
        /// <summary>
        /// Removes the asynchronous policy.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        bool RemoveAsyncPolicy(string key);
        #endregion
        #region Connection Methods
        /// <summary>
        /// Opens the asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task ExecuteOpenAsync(CancellationToken token = default);
        /// <summary>
        /// Opens the asynchronous.
        /// </summary>
        /// <param name="keyName">Name of the key.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task ExecuteOpenAsync(string keyName, CancellationToken token = default);
        /// <summary>
        /// Opens the asynchronous.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task ExecuteOpenAsync(IAsyncPolicy policy, CancellationToken token = default);
        /// <summary>
        /// Opens the asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<PolicyResult> CaptureOpenAsync(CancellationToken token = default);
        /// <summary>
        /// Opens the asynchronous.
        /// </summary>
        /// <param name="keyName">Name of the key.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<PolicyResult> CaptureOpenAsync(string keyName, CancellationToken token = default);
        /// <summary>
        /// Opens the asynchronous.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<PolicyResult> CaptureOpenAsync(IAsyncPolicy policy, CancellationToken token = default);
        #endregion
    }
}
