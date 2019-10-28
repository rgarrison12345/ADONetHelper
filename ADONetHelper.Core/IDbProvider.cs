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
using System.Data;
using System.Data.Common;
#endregion

namespace ADONetHelper.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDbProvider
    {
        #region Events        
        /// <summary>
        /// Sets the state change event handler.  This event occurs when the <see cref="DbConnection.State"/> changes
        /// </summary>
        /// <value>
        /// The state change handler delegate
        /// </value>
        event StateChangeEventHandler StateChange;
        #endregion
        #region Fields/Properties
        /// <summary>
        /// Whether or not the passed in provider is capable of creating a data source enumerator
        /// </summary>
        bool CanCreateDataSourceEnumerator { get; }
        /// <summary>
        /// Whether or not the <see cref="ConnectionString"/> is readonly
        /// </summary>
        bool ConnectionStringReadonly { get; }
        /// <summary>
        /// Whether the <see cref="ConnectionString"/> has a fixed size
        /// </summary>
        bool ConnectionStringFixedSize { get; }
        /// <summary>
        /// Gets the current number of keys that are contained within the <see cref="ConnectionString"/> property
        /// </summary>
        int ConnectionStringKeyCount { get; }
        /// <summary>
        /// The current <see cref="ConnectionState"/> of the <see cref="DbConnection"/>
        /// </summary>
        ConnectionState State { get; }
        /// <summary>
        /// ConnectionString as a <see cref="string"/> to use when creating a <see cref="DbConnection"/>
        /// </summary>
        string ConnectionString { get; set; }
        /// <summary>
        /// Gets or sets the wait time in seconds before terminating the attempt to execute a command and generating an error.
        /// </summary>
        int CommandTimeout { get; set; }
        /// <summary>
        /// Gets the time to wait in seconds while trying to establish a connection before terminating the attempt and generating an error.
        /// </summary>
        int ConnectionTimeout { get; }
        /// <summary>
        /// Gets the name of the current database after a connection is opened, or the database name specified in the connection string before the <see cref="DbConnection"/> is opened.
        /// </summary>
        string Database { get; }
        /// <summary>
        /// Gets the name of the database server to which to connect.
        /// </summary>
        string DataSource { get; }
        /// <summary>
        /// Gets a string that represents the version of the server to which the object is connected
        /// </summary>
        string ServerVersion { get; }
        #endregion
    }
}