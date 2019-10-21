#region Using Statements
#endregion

namespace ADONetHelper.Polly
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDbFaultTolerantClient
    {
        /// <summary>
        /// Gets or sets the default asynchronous policy key.
        /// </summary>
        /// <value>
        /// The default asynchronous policy key.
        /// </value>
        string DefaultAsyncPolicyKey { get; set; }
        /// <summary>
        /// Gets or sets the default synchronize policy key.
        /// </summary>
        /// <value>
        /// The default synchronize policy key.
        /// </value>
        string DefaultSyncPolicyKey { get; set; }
    }
}
