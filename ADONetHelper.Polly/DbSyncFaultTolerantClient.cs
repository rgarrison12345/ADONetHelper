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
#endregion

namespace ADONetHelper.Polly
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DbFaultTolerantClient : ISyncFaultTolerantClient, IDisposable
    {
        #region Fields/Properties
        private PolicyRegistry _syncPolicyRegistry = null;

        /// <summary>
        /// Gets the asynchronous registry.
        /// </summary>
        /// <value>
        /// The asynchronous registry.
        /// </value>
        private PolicyRegistry SyncRegistry
        {
            get
            {
                //Check for null, might need to instantiate a new instance
                if (_syncPolicyRegistry == null)
                {
                    _syncPolicyRegistry = new PolicyRegistry();
                }

                return _syncPolicyRegistry;
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
        /// <returns></returns>
        public DbDataReader ExecuteGetDataReader(string query, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null)
        {
            return ExecuteGetDataReader(query, DefaultSyncPolicyKey, behavior, transact);
        }
        /// <summary>
        /// Executes the get data reader asynchronous.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="policyName">Name of the policy.</param>
        /// <param name="behavior">The behavior.</param>
        /// <param name="transact">The transact.</param>
        /// <returns></returns>
        public DbDataReader ExecuteGetDataReader(string query, string policyName, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null)
        {
            return ExecuteGetDataReader(query, GetSyncPolicy<ISyncPolicy>(policyName), behavior, transact);
        }
        /// <summary>
        /// Executes the get data reader asynchronous.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="policy">The policy.</param>
        /// <param name="behavior">The behavior.</param>
        /// <param name="transact">The transact.</param>
        /// <returns></returns>
        public DbDataReader ExecuteGetDataReader(string query, ISyncPolicy policy, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null)
        {
            return policy.Execute(() => ExecuteSQL.GetDbDataReader(QueryCommandType, query, behavior, transact));
        }
        /// <summary>
        /// Captures the get data reader asynchronous.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="behavior">The behavior.</param>
        /// <param name="transact">The transact.</param>
        /// <returns></returns>
        public PolicyResult<DbDataReader> CaptureGetDataReader(string query, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null)
        {
            return CaptureGetDataReader(query, DefaultSyncPolicyKey, behavior, transact);
        }
        /// <summary>
        /// Captures the get data reader asynchronous.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="policyName">Name of the policy.</param>
        /// <param name="behavior">The behavior.</param>
        /// <param name="transact">The transact.</param>
        /// <returns></returns>
        public PolicyResult<DbDataReader> CaptureGetDataReader(string query, string policyName, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null)
        {
            return CaptureGetDataReader(query, GetSyncPolicy<ISyncPolicy>(policyName), behavior, transact);
        }
        /// <summary>
        /// Captures the get data reader asynchronous.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="policy">The policy.</param>
        /// <param name="behavior">The behavior.</param>
        /// <param name="transact">The transact.</param>
        /// <returns></returns>
        public PolicyResult<DbDataReader> CaptureGetDataReader(string query, ISyncPolicy policy, CommandBehavior behavior = CommandBehavior.Default, DbTransaction transact = null)
        {
            return policy.ExecuteAndCapture(() => ExecuteSQL.GetDbDataReader(QueryCommandType, query, behavior, transact));
        }
        /// <summary>
        /// Executes the get data object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public T ExecuteGetDataObject<T>(string query) where T : class
        {
            return ExecuteGetDataObject<T>(query, DefaultSyncPolicyKey);
        }
        /// <summary>
        /// Executes the get data object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="policyName">Name of the policy.</param>
        /// <returns></returns>
        public T ExecuteGetDataObject<T>(string query, string policyName) where T : class
        {
            //Return this back to the caller
            return ExecuteGetDataObject<T>(query, GetSyncPolicy<ISyncPolicy>(policyName));
        }
        /// <summary>
        /// Executes the get data object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="policy">The policy.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">policy</exception>
        public T ExecuteGetDataObject<T>(string query, ISyncPolicy policy) where T : class
        {
            //Check for null 
            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }

            //Return this back to the caller
            return policy.Execute(() => ExecuteSQL.GetDataObject<T>(QueryCommandType, query));
        }
        /// <summary>
        /// Captures the get data object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public PolicyResult<T> CaptureGetDataObject<T>(string query) where T : class
        {
            //Return this back to the caller
            return CaptureGetDataObject<T>(query, DefaultSyncPolicyKey);
        }
        /// <summary>
        /// Captures the get data object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="policyName">Name of the policy.</param>
        /// <returns></returns>
        public PolicyResult<T> CaptureGetDataObject<T>(string query, string policyName) where T : class
        {
            //Return this back to the caller
            return CaptureGetDataObject<T>(query, GetSyncPolicy<ISyncPolicy>(policyName));
        }
        /// <summary>
        /// Captures the get data object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="policy">The policy.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">policy</exception>
        public PolicyResult<T> CaptureGetDataObject<T>(string query, ISyncPolicy policy) where T : class
        {
            //Check for null 
            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }

            //Return this back to the caller
            return policy.ExecuteAndCapture(() => ExecuteSQL.GetDataObject<T>(QueryCommandType, query));
        }
        /// <summary>
        /// Executes the get data object enumerable asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public IEnumerable<T> ExecuteGetDataObjectEnumerable<T>(string query) where T : class
        {
            return ExecuteGetDataObjectEnumerable<T>(query, DefaultSyncPolicyKey);
        }
        /// <summary>
        /// Executes the get data object enumerable asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="policyName">Name of the policy.</param>
        /// <returns></returns>
        public IEnumerable<T> ExecuteGetDataObjectEnumerable<T>(string query, string policyName) where T : class
        {
            //Return this back to the caller
            return ExecuteGetDataObjectEnumerable<T>(query, GetSyncPolicy<ISyncPolicy>(policyName));
        }
        /// <summary>
        /// Executes the get data object enumerable asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="policy">The policy.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">policy</exception>
        public IEnumerable<T> ExecuteGetDataObjectEnumerable<T>(string query, ISyncPolicy policy) where T : class
        {
            //Check for null 
            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }

            //Return this back to the caller
            return policy.Execute(() => ExecuteSQL.GetDataObjectEnumerable<T>(QueryCommandType, query));
        }
        /// <summary>
        /// Captures the get data object enumerable asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public PolicyResult<IEnumerable<T>> CaptureGetDataObjectEnumerable<T>(string query) where T : class
        {
            return CaptureGetDataObjectEnumerable<T>(query, DefaultSyncPolicyKey);
        }
        /// <summary>
        /// Captures the get data object enumerable asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="policyName">Name of the policy.</param>
        /// <returns></returns>
        public PolicyResult<IEnumerable<T>> CaptureGetDataObjectEnumerable<T>(string query, string policyName) where T : class
        {
            //Return this back to the caller
            return CaptureGetDataObjectEnumerable<T>(query, GetSyncPolicy<ISyncPolicy>(policyName));
        }
        /// <summary>
        /// Captures the get data object enumerable asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="policy">The policy.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">policy</exception>
        public PolicyResult<IEnumerable<T>> CaptureGetDataObjectEnumerable<T>(string query, ISyncPolicy policy) where T : class
        {
            //Check for null 
            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }

            //Return this back to the caller
            return policy.ExecuteAndCapture(() => ExecuteSQL.GetDataObjectEnumerable<T>(QueryCommandType, query));
        }
        #endregion
        #region Data Modification
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="int"/></returns>
        public int ExecuteNonQuery(string query)
        {
            //Return this back to the caller
            return ExecuteNonQuery(query, DefaultSyncPolicyKey);
        }
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="policyName"></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="int"/></returns>
        public int ExecuteNonQuery(string query, string policyName)
        {
            //Return this back to the caller
            return ExecuteNonQuery(query, GetSyncPolicy<ISyncPolicy>(policyName));
        }
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="int"/></returns>
        public int ExecuteNonQuery(string query, ISyncPolicy policy)

        {
            //Check for null 
            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }

            //Return this back to the caller
            return policy.Execute(() => ExecuteSQL.ExecuteNonQuery(QueryCommandType, query));
        }
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="int"/></returns>
        public PolicyResult<int> CaptureNonQuery(string query)
        {
            return CaptureNonQuery(query, DefaultSyncPolicyKey);
        }
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="policyName"></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="int"/></returns>
        public PolicyResult<int> CaptureNonQuery(string query, string policyName)
        {
            //Return this back to the caller
            return CaptureNonQuery(query, GetSyncPolicy<ISyncPolicy>(policyName));
        }
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>Returns the number of rows affected by this query as a <see cref="int"/></returns>
        public PolicyResult<int> CaptureNonQuery(string query, ISyncPolicy policy)
        {
            //Check for null 
            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }

            //Return this back to the caller
            return policy.ExecuteAndCapture(() => ExecuteSQL.ExecuteNonQuery(QueryCommandType, query));
        }
        #endregion
        #region Utility Methods              
        /// <summary>
        /// 
        /// </summary>
        /// <param name="policies"></param>
        public void AddSyncPolicyRange(IEnumerable<ISyncPolicy> policies)
        {
            bool noNamePolicies = policies.Where(x => string.IsNullOrWhiteSpace(x.PolicyKey) == true).Count() > 0;

            //Check that we had policies with names
            if (noNamePolicies == true)
            {
                throw new ArgumentException($"{nameof(policies)} had policies with no keys");
            }

            //Loop through each and see if there's a key
            foreach (ISyncPolicy policy in policies)
            {
                SyncRegistry.Add(policy.PolicyKey, policy);
            }
        }
        /// <summary>
        /// Gets the synchronize policy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        public ISyncPolicy GetSyncPolicy<T>(string keyName) where T : ISyncPolicy
        {
            return SyncRegistry.Get<T>(keyName);
        }
        /// <summary>
        /// Adds the synchronize policy.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="policy">The policy.</param>
        public void AddSyncPolicy(string key, ISyncPolicy policy)
        {
            SyncRegistry.Add(key, policy);
        }
        /// <summary>
        /// Removes the asynchronous policy.
        /// </summary>
        /// <param name="key">The key.</param>
        public bool RemoveSyncPolicy(string key)
        {
            return SyncRegistry.Remove(key);
        }
        #endregion
        #region Connection Methods
        /// <summary>
        /// Starts a <see cref="DbTransaction"/> using the underlying <see cref="DbConnection"/>
        /// </summary>
        /// <returns>Returns an instance of <see cref="DbTransaction"/></returns>
        public DbTransaction GetDbTransaction()
        {
            //Return this back to the caller
            return ExecuteSQL.Connection.BeginTransaction();
        }
        /// <summary>
        /// Starts a <see cref="DbTransaction"/> using the underlying <see cref="DbConnection"/> with the <paramref name="level"/>
        /// </summary>
        /// <param name="level">The <see cref="System.Data.IsolationLevel"/> to describe the locking behavior for the transaction</param>
        /// <returns>Returns an instance of <see cref="DbTransaction"/></returns>
        public DbTransaction GetDbTransaction(IsolationLevel level)
        {
            //Return this back to the caller
            return ExecuteSQL.Connection.BeginTransaction(level);
        }
        /// <summary>
        /// Changes the current <see cref="DbConnection"/> to target a different database
        /// </summary>
        /// <param name="databaseName">The name of a database as a <see cref="string"/></param>
        public void ChangeDatabase(string databaseName)
        {
            //Now change the database
            ExecuteSQL.Connection.ChangeDatabase(databaseName);
        }
        /// <summary>
        /// Disposes of the <see cref="DbConnection"/> being used by this instance, clears any <see cref="DbParameter"/>
        /// assocatied with the current <see cref="DbConnection"/>
        /// </summary>
        public void Close()
        {
            //Clear params from this connection
            ClearParameters();

            //Dispose of the database connection
            ExecuteSQL.Connection.Dispose();
        }
        /// <summary>
        /// Executes the open.
        /// </summary>
        public void ExecuteOpen()
        {
            ExecuteOpen(DefaultSyncPolicyKey);
        }
        /// <summary>
        /// Opens the connection to a database
        /// </summary>
        public void ExecuteOpen(string policyName)
        {
            ExecuteOpen(GetSyncPolicy<ISyncPolicy>(policyName));
        }
        /// <summary>
        /// Executes the open.
        /// </summary>
        /// <param name="policy">The policy.</param>
        public void ExecuteOpen(ISyncPolicy policy)
        {
            policy.Execute(() => ExecuteSQL.Connection.Open());
        }
        /// <summary>
        /// Executes the open.
        /// </summary>
        public PolicyResult CaptureOpen()
        {
            return CaptureOpen(DefaultSyncPolicyKey);
        }
        /// <summary>
        /// Opens the connection to a database
        /// </summary>
        public PolicyResult CaptureOpen(string policyName)
        {
            return CaptureOpen(GetSyncPolicy<ISyncPolicy>(policyName));
        }
        /// <summary>
        /// Executes the open.
        /// </summary>
        /// <param name="policy">The policy.</param>
        public PolicyResult CaptureOpen(ISyncPolicy policy)
        {
            return policy.ExecuteAndCapture(() => ExecuteSQL.Connection.Open());
        }
        #endregion
        #region IDisposable Support
        /// <summary>
        /// Dispose of any unmanged resorces if disposing passed in is true 
        /// </summary>
        /// <param name="disposing">Whether or not we need to explicitly close unmanaged resources</param>
        protected virtual void Dispose(bool disposing)
        {
            //Check if we have disposed before
            if (!disposedValue)
            {
                //Check if we are disposing
                if (disposing)
                {
                    //Close connection to the database
                    Close();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                disposedValue = true;
            }
        }
        /// <summary>
        /// Dispose of any unmanged resources
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}