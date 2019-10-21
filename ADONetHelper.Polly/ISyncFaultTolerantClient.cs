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
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
#endregion

namespace ADONetHelper.Polly
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISyncFaultTolerantClient
    {
        #region Data Retrieval
        /// <summary>
        /// Executes the get data reader asynchronous.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="behavior">The behavior.</param>
        /// <param name="transact">The transact.</param>
        /// <returns></returns>
        DbDataReader ExecuteGetDataReader(string query, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null);
        /// <summary>
        /// Executes the get data reader asynchronous.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="policyName">Name of the policy.</param>
        /// <param name="behavior">The behavior.</param>
        /// <param name="transact">The transact.</param>
        /// <returns></returns>
        DbDataReader ExecuteGetDataReader(string query, string policyName, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null);
        /// <summary>
        /// Executes the get data reader asynchronous.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="policy">The policy.</param>
        /// <param name="behavior">The behavior.</param>
        /// <param name="transact">The transact.</param>
        /// <returns></returns>
        DbDataReader ExecuteGetDataReader(string query, ISyncPolicy policy, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null);
        /// <summary>
        /// Captures the get data reader asynchronous.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="behavior">The behavior.</param>
        /// <param name="transact">The transact.</param>
        /// <returns></returns>
        PolicyResult<DbDataReader> CaptureGetDataReader(string query, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null);
        /// <summary>
        /// Captures the get data reader asynchronous.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="policyName">Name of the policy.</param>
        /// <param name="behavior">The behavior.</param>
        /// <param name="transact">The transact.</param>
        /// <returns></returns>
        PolicyResult<DbDataReader> CaptureGetDataReader(string query, string policyName, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null);
        /// <summary>
        /// Captures the get data reader asynchronous.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="policy">The policy.</param>
        /// <param name="behavior">The behavior.</param>
        /// <param name="transact">The transact.</param>
        /// <returns></returns>
        PolicyResult<DbDataReader> CaptureGetDataReader(string query, ISyncPolicy policy, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null);
        /// <summary>
        /// Executes the get data object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        T ExecuteGetDataObject<T>(string query) where T : class;
        /// <summary>
        /// Executes the get data object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="policyName">Name of the policy.</param>
        /// <returns></returns>
        T ExecuteGetDataObject<T>(string query, string policyName) where T : class;
        /// <summary>
        /// Executes the get data object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="policy">The policy.</param>
        /// <returns></returns>
        T ExecuteGetDataObject<T>(string query, ISyncPolicy policy) where T : class;
        /// <summary>
        /// Captures the get data object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        PolicyResult<T> CaptureGetDataObject<T>(string query) where T : class;
        /// <summary>
        /// Captures the get data object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="policyName">Name of the policy.</param>
        /// <returns></returns>
        PolicyResult<T> CaptureGetDataObject<T>(string query, string policyName) where T : class;
        /// <summary>
        /// Captures the get data object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="policy">The policy.</param>
        /// <returns></returns>
        PolicyResult<T> CaptureGetDataObject<T>(string query, ISyncPolicy policy) where T : class;
        /// <summary>
        /// Executes the get data object enumerable asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        IEnumerable<T> ExecuteGetDataObjectEnumerable<T>(string query) where T : class;
        /// <summary>
        /// Executes the get data object enumerable asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="policyName">Name of the policy.</param>
        /// <returns></returns>
        IEnumerable<T> ExecuteGetDataObjectEnumerable<T>(string query, string policyName) where T : class;
        /// <summary>
        /// Executes the get data object enumerable asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="policy">The policy.</param>
        /// <returns></returns>
        IEnumerable<T> ExecuteGetDataObjectEnumerable<T>(string query, ISyncPolicy policy) where T : class;
        /// <summary>
        /// Captures the get data object enumerable asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        PolicyResult<IEnumerable<T>> CaptureGetDataObjectEnumerable<T>(string query) where T : class;
        /// <summary>
        /// Captures the get data object enumerable asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="policyName">Name of the policy.</param>
        /// <returns></returns>
        PolicyResult<IEnumerable<T>> CaptureGetDataObjectEnumerable<T>(string query, string policyName) where T : class;
        /// <summary>
        /// Captures the get data object enumerable asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="policy">The policy.</param>
        /// <returns></returns>
        PolicyResult<IEnumerable<T>> CaptureGetDataObjectEnumerable<T>(string query, ISyncPolicy policy) where T : class;
        #endregion
        #region Data Modification
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="int"/></returns>
        int ExecuteNonQuery(string query);
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="policyName"></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="int"/></returns>
        int ExecuteNonQuery(string query, string policyName);
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="int"/></returns>
        int ExecuteNonQuery(string query, ISyncPolicy policy);
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="int"/></returns>
        PolicyResult<int> CaptureNonQuery(string query);
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="policyName"></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="int"/></returns>
        PolicyResult<int> CaptureNonQuery(string query, string policyName);
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="int"/></returns>
        PolicyResult<int> CaptureNonQuery(string query, ISyncPolicy policy);
        #endregion
        #region Utility Methods
        /// <summary>
        /// Adds the asynchronous policy range.
        /// </summary>
        /// <param name="policies">The policies.</param>
        void AddSyncPolicyRange(IEnumerable<ISyncPolicy> policies);
        /// <summary>
        /// Gets the synchronize policy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        ISyncPolicy GetSyncPolicy<T>(string keyName) where T : ISyncPolicy;
        /// <summary>
        /// Adds the synchronize policy.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="policy">The policy.</param>
        void AddSyncPolicy(string key, ISyncPolicy policy);
        /// <summary>
        /// Removes the asynchronous policy.
        /// </summary>
        /// <param name="key">The key.</param>
        bool RemoveSyncPolicy(string key);
        #endregion
        #region Connection Methods
        /// <summary>
        /// Executes the open.
        /// </summary>
        void ExecuteOpen();
        /// <summary>
        /// Opens the connection to a database
        /// </summary>
        void ExecuteOpen(string policyName);
        /// <summary>
        /// Executes the open.
        /// </summary>
        /// <param name="policy">The policy.</param>
        void ExecuteOpen(ISyncPolicy policy);
        /// <summary>
        /// Executes the open.
        /// </summary>
        PolicyResult CaptureOpen();
        /// <summary>
        /// Opens the connection to a database
        /// </summary>
        PolicyResult CaptureOpen(string policyName);
        /// <summary>
        /// Executes the open.
        /// </summary>
        /// <param name="policy">The policy.</param>
        PolicyResult CaptureOpen(ISyncPolicy policy);
        #endregion
    }
}