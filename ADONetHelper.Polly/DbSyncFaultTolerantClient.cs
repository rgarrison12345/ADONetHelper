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
using System.Data;
using System.Data.Common;
#endregion

namespace ADONetHelper.Polly
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DbFaultTolerantClient : IDisposable
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
        #region Utility Methods                
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
        /// Opens the connection to a database
        /// </summary>
        public void Open()
        {
            //Call this again
            ExecuteSQL.Connection.Open();
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