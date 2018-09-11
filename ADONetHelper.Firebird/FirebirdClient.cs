#region Licenses
/*MIT License
Copyright(c) 2018
Robert Garrison

Permission Is hereby granted, free Of charge, To any person obtaining a copy
of this software And associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, And/Or sell
copies Of the Software, And To permit persons To whom the Software Is
furnished To Do so, subject To the following conditions:

The above copyright notice And this permission notice shall be included In all
copies Or substantial portions Of the Software.

THE SOFTWARE Is PROVIDED "AS IS", WITHOUT WARRANTY Of ANY KIND, EXPRESS Or
IMPLIED, INCLUDING BUT Not LIMITED To THE WARRANTIES Of MERCHANTABILITY,
FITNESS For A PARTICULAR PURPOSE And NONINFRINGEMENT. In NO Event SHALL THE
AUTHORS Or COPYRIGHT HOLDERS BE LIABLE For ANY CLAIM, DAMAGES Or OTHER
LIABILITY, WHETHER In AN ACTION Of CONTRACT, TORT Or OTHERWISE, ARISING FROM,
OUT Of Or In CONNECTION With THE SOFTWARE Or THE USE Or OTHER DEALINGS In THE
SOFTWARE*/
#endregion
#region Using Statements
using FirebirdSql.Data.FirebirdClient;
using FirebirdSql.Data.Services;
using FirebirdSql.Data.Isql;
using System;
using System.Collections.Generic;
using System.Data;
#endregion

namespace ADONetHelper.Firebird
{
    /// <summary>
    /// A specialized instance of <see cref="DbClient"/> that is used to query a Firebird database system
    /// </summary>
    /// <seealso cref="DbClient"/>
    public class FirebirdClient : DbClient
    {
        #region Fields/Properties
        private FbRemoteEvent _event = null;
        private FbSecurity _security = null;
        private FbValidation _validation = null;

        /// <summary>
        /// Gets an instance of <see cref="FbRemoteEvent"/> using the current <see cref="DbClient.ConnectionString"/>
        /// </summary>
        public FbRemoteEvent @Event
        {
            get
            {
                //Get the event
                _event = _event ?? new FbRemoteEvent(this.ConnectionString);

                //Return this back to teh caller
                return _event;
            }
        }
        /// <summary>
        /// Gets an instance of <see cref="FbSecurity"/> using the current <see cref="DbClient.ConnectionString"/>
        /// </summary>
        private FbSecurity Security
        {
            get
            {
                _security = _security ?? new FbSecurity(this.ConnectionString);

                //Return this back to the caller
                return _security;
            }
        }
        /// <summary>
        /// Gets an instance of <see cref="FbValidation"/> based on the current <see cref="DbClient.ConnectionString"/>
        /// </summary>
        private FbValidation Validation
        {
            get
            {
                _validation = _validation ?? new FbValidation(this.ConnectionString);

                //Return this back to the caller
                return _validation;
            }
        }
        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        private FbConnection Connection
        {
            get
            {
                //Return this back to the caller
                return (FbConnection)this.ExecuteSQL.Connection;
            }
        }
        /// <summary>
        /// Gets the size of the packet.
        /// </summary>
        public int PacketSize
        {
            get
            {
                //Return this back to the caller
                return this.Connection.PacketSize;
            }
        }
        #endregion
        #region Constructors
        /// <summary>
        /// The overloaded constuctor that will initialize the <paramref name="connectionString"/>, And <paramref name="queryCommandType"/>
        /// </summary>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        public FirebirdClient(string connectionString, CommandType queryCommandType) : base(connectionString, queryCommandType, FirebirdClientFactory.Instance)
        {
        }
        /// <summary>
        /// The overloaded constuctor that will initialize the <paramref name="connectionString"/>
        /// </summary>
        /// <param name="connectionString">The connection string used to query a data store</param>
        public FirebirdClient(string connectionString) : base(connectionString, FirebirdClientFactory.Instance)
        {
        }
        /// <summary>
        /// Intializes the <see cref="FirebirdClient"/> with a <see cref="ISqlExecutor"/>
        /// </summary>
        /// <param name="executor">An instance of <see cref="ISqlExecutor"/></param>
        public FirebirdClient(ISqlExecutor executor) : base(executor)
        {
        }
        /// <summary>
        /// Constructor to query a database using an existing <see cref="FbConnection"/> to initialize the <paramref name="connection"/>
        /// </summary>
        /// <param name="connection">An instance of <see cref="FbConnection"/> to use to query a database </param>
        public FirebirdClient(FbConnection connection) : base(connection)
        {
        }
        /// <summary>
        /// Insantiates a new instance of <see cref="FirebirdClient"/> using the passed in <paramref name="connectionString"/> and <paramref name="factory"/>
        /// </summary>
        /// <param name="connectionString">Connection string to use to query a database</param>
        /// <param name="factory">An instance of <see cref="IDbObjectFactory"/></param>
        public FirebirdClient(string connectionString, IDbObjectFactory factory) : base(connectionString, factory)
        {
        }
        #endregion
        #region Utility Methods
        /// <summary>
        /// Gets an instance of <see cref="FbDatabaseInfo"/> based on the database the <see cref="FbConnection"/> is targeting
        /// </summary>
        /// <returns>Returns an instance of <see cref="FbDatabaseInfo"/></returns>
        public FbDatabaseInfo GetCurrentDatabaseInfo()
        {
            //Return this back to the caller
            return new FbDatabaseInfo(this.Connection);
        }
        /// <summary>
        /// Adds the user.
        /// </summary>
        /// <param name="data">The data.</param>
        public void AddUser(FbUserData data)
        {
            //Add the user
            this.Security.AddUser(data);
        }
        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="data">The data.</param>
        public void DeleteUser(FbUserData data)
        {
            this.Security.DeleteUser(data);
        }
        /// <summary>
        /// Modifies the user.
        /// </summary>
        /// <param name="data">The data.</param>
        public void ModifyUser(FbUserData data)
        {
            this.Security.ModifyUser(data);
        }
        /// <summary>
        /// Gets the users database path based on the current <see cref="FbConnection.ConnectionString"/>
        /// </summary>
        public string GetUsersDbPath()
        {
            //Return this back to the caller
            return this.Security.GetUsersDbPath();
        }
        /// <summary>
        /// Gets a single instance of <see cref="FbUserData"/> that the current <see cref="FbSecurity"/> is targeting based on the <see cref="FbConnection.ConnectionString"/>
        /// </summary>
        /// <param name="userName">The name of a specific user</param>
        /// <returns></returns>
        public FbUserData GetUser(string userName)
        {
            //Return this back to the caller
            return this.Security.DisplayUser(userName);
        }
        /// <summary>
        /// Gets all users that the current <see cref="FbSecurity"/> is targeting based on the <see cref="FbConnection.ConnectionString"/>
        /// </summary>
        /// <returns></returns>
        public FbUserData[] GetUsers()
        {
            return this.Security.DisplayUsers();
        }
        /// <summary>
        /// Backups the database.
        /// </summary>
        /// <param name="files"></param>
        /// <param name="flags"></param>
        /// <param name="verbose"></param>
        public void BackupDatabase(Dictionary<string, int?> files, FbBackupFlags flags, bool verbose)
        {
            FbBackup svc = new FbBackup(this.Connection.ConnectionString)
            {
                //Set the flags
                Options = flags,
                Verbose = verbose
            };

            //Loop through all items in the dictionary
            foreach (KeyValuePair<string, int?> kvp in files)
            {
                //Keep adding backup files
                svc.BackupFiles.Add(GetBackupFile(kvp.Key, kvp.Value));
            }

            //Execute the backup
            svc.Execute();
        }
        /// <summary>
        /// Gets a new instance of <see cref="FbBackup"/> based on the <paramref name="fileName"/> and <paramref name="fileLength"/> passed into the routine
        /// </summary>
        /// <param name="fileName">The name of the file on a file system</param>
        /// <param name="fileLength">The length of the file, not necessary to use</param>
        /// <returns></returns>
        private static FbBackupFile GetBackupFile(string fileName, int? fileLength)
        {
            //Return this back to the caller
            return new FbBackupFile(fileName, fileLength);
        }
        /// <summary>
        /// Loads the script from file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static FbScript LoadScriptFromFile(string fileName)
        {
            //Return this back to the caller
            return FbScript.LoadFromFile(fileName);
        }
        /// <summary>
        /// Parses the script.
        /// </summary>
        /// <param name="scriptText">The script text.</param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public static FbStatementCollection ParseScript(string scriptText, EventHandler<UnknownStatementEventArgs> handler)
        {
            FbScript script = new FbScript(scriptText);

            //Set unkown statement handler
            script.UnknownStatement += handler;

            //Parse the script
            script.Parse();

            //Return the results back to the caller
            return script.Results;
        }
        #endregion
    }
}
