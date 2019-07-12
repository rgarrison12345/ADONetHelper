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
#region Using statements
using System;
using System.Collections.Generic;
using System.Data.Common;
#endregion

namespace ADONetHelper
{
    /// <summary>
    /// The contract class for a ISqlExecutor based class
    /// </summary>
    /// <seealso cref="IDisposable"/>
    public partial interface ISqlExecutor : IDisposable
    {
        #region Fields/Properties
        /// <summary>
        /// The character symbol to use when binding a variable in a given providers SQL query
        /// </summary>
        string VariableBinder { get; set; }
        /// <summary>
        /// Represents an instance of <see cref="DbConnection"/>
        /// </summary>
        DbConnection Connection { get; }
        /// <summary>
        /// <see cref="DbConnectionStringBuilder"/> to use to build a connection string based off of the data provider DB helper is using
        /// </summary>
        DbConnectionStringBuilder ConnectionStringBuilder { get; }
        /// <summary>
        /// Represents an instance of <see cref="IDbObjectFactory"/>
        /// </summary>
        IDbObjectFactory Factory { get; }
        /// <summary>
        /// The list of query database parameters that are associated with a query
        /// </summary>
        List<DbParameter> Parameters { get; set; }
        /// <summary>
        /// Gets or sets the wait time in seconds before terminating the attempt to execute a command and generating an error.
        /// </summary>
        int CommandTimeout { get; set; }
        #endregion
    }
}
