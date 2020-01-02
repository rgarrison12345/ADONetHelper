#region Licenses
/*MIT License
Copyright(c) 2020
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
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
#endregion

namespace ADONetHelper.Core
{
    /// <summary>
    /// Transfer object that contains the information about a query to use when querying a datastore
    /// </summary>
    public class SQLQuery
    {
        #region Fields/Properties
        /// <summary>
        /// Represents how a command should be interpreted by the data provider
        /// </summary>
        public CommandType QueryType { get; set; }
        /// <summary>
        /// The query command text or name of stored procedure to execute against the data store
        /// </summary>
        public string QueryText { get; set; }
        /// <summary>
        /// The list of query database parameters that are associated with a query
        /// </summary>
        public List<DbParameter> ParameterList { get; set; }
        #endregion
        #region Constructors
        /// <summary>
        /// Instantiates the SQL Query with text, command type, and parameter list
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="type">Represents how a command should be interpreted by the data provider</param>
        /// <param name="list">The list of query database parameters that are associated with a query</param>
        public SQLQuery(string query, CommandType type, List<DbParameter> list)
        {
            QueryText = query;
            QueryType = type;
            ParameterList = list;
        }
        /// <summary>
        /// Instantiates the SQL Query with text, and command type
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="type">Represents how a command should be interpreted by the data provider</param>
        public SQLQuery(string query, CommandType type)
        {
            QueryText = query;
            QueryType = type;
        }
        /// <summary>
        /// Empty constructor to instantiate object
        /// </summary>
        public SQLQuery()
        {
        }
        #endregion
    }
}

