#region Using Statements
using System.Data;
using NUnit.Framework;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Polly;
using ADONetHelper.Polly;
using Polly.NoOp;
#endregion

namespace ADONetHelper.Tests.NetCore
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class Tests
    {
        #region Fields/Properties
        public readonly string ConnectionString = "Data Source=unitytest02.easydraft.com;Initial Catalog=EasyDraft;ConnectRetryCount=3;ConnectRetryInterval=10;Persist Security Info=True;Uid=EZUser;Pwd=C2t7K9A8E5;";
        public readonly string FallBackConnectionString = "Data Source=unitytest01.easydraft.com;Initial Catalog=EasyDraft;ConnectRetryCount=3;ConnectRetryInterval=10;Persist Security Info=True;Uid=EZUser;Pwd=C2t7K9A8E5;";
        public readonly string ProviderName = "System.Data.SqlClient";
        public DbFaultTolerantClient _client;
        #endregion
        [SetUp]
        public void Setup()
        {
            _client = new DbFaultTolerantClient(ConnectionString, SqlClientFactory.Instance);
        }
        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
        [Test]
        public async Task OpenCloseConnection()
        {
            try
            {
                PolicyResult result = await _client.CaptureOpenAsync(GetPolicy()).ConfigureAwait(false);

                //Needs to be open
                Assert.True(_client.State == ConnectionState.Open, "Connection didn't open");

                _client.Close();

                //Needs to be closed
                Assert.True(_client.State == ConnectionState.Closed, "Connection didn't close");
            }
            catch (SqlException conEx)
            {

            }
        }
        #region Helper Methods
        private IAsyncPolicy GetPolicy()
        {
            return Policy
            .Handle<SqlException>(ex => ex.Number == 1205 || ex.Number == 53) // sql deadlock
            .FallbackAsync(async (token) =>
            {
                _client.ConnectionString = FallBackConnectionString;

                //Open the connection to the database
                await _client.ExecuteOpenAsync(Policy.NoOpAsync(), token).ConfigureAwait(false);
            });
        }
        #endregion
    }
}