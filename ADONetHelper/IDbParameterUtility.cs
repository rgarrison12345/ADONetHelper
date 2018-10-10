#region Licenses
/*MIT License
Copyright(c) 2018
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

namespace ADONetHelper
{
    /// <summary>
    /// Contract class for modifying and searching parameters in a DbParameter collection
    /// </summary>
    public interface IDbParameterUtility
    {
        #region Helper Methods
        /// <summary>
        /// Removes a <see cref="DbParameter"/> from the parameters collection by using the parameter name
        /// </summary>
        /// <param name="parameterName">The name of the parameter to identify the parameter to remove from the collection</param>
        /// <returns>Returns true if item was successully removed, false otherwise if item was not found in the list</returns>
        bool RemoveParameter(string parameterName);
        /// <summary>
        /// Removes a <see cref="DbParameter"/> from the parameters collection by using the index of the parameter
        /// </summary>
        /// <param name="index">The index of the parameter in the parameters collection to identify the parameter to remove from the collection</param>
        /// <returns>Returns true if item was successully removed, false otherwise if item was not found in the list</returns>
        bool RemoveParameter(int index);
        /// <summary>
        /// Replaces an existing parameter with the new <see cref="DbParameter"/> with an existing <see cref="DbParameter.ParameterName"/>
        /// </summary>
        /// <param name="parameterName">The index as a <c>string</c> to use when searching for the existing parameter</param>
        /// <param name="param">A new instance of <see cref="DbParameter"/></param>
        void ReplaceParameter(string parameterName, DbParameter param);
        /// <summary>
        /// Replaces an existing parameter with the new <see cref="DbParameter"/> passed in at the <paramref name="index"/>
        /// </summary>
        /// <param name="index">The index as an <see cref="int"/> to use when searching for the existing parameter</param>
        /// <param name="param">A new instance of <see cref="DbParameter"/></param>
        void ReplaceParameter(int index, DbParameter param);
        /// <summary>
        /// Clears all parameters from the parameters collection
        /// </summary>
        void ClearParameters();
        /// <summary>
        /// Retrieves a DbParameter object by using the passed in parameter name
        /// </summary>
        /// <param name="parameterName">The name of the parameter to use to find the parameter value</param>
        /// <returns>The specified <see cref="DbParameter"/> object from the parameters collection</returns>
        DbParameter GetParameter(string parameterName);
        /// <summary>
        /// Retrieves a parameter from the parameters collection by using the index of the parameter
        /// </summary>
        /// <param name="index">The index of the parameter in the parameters collection to identify the parameter to retrieve from the collection</param>
        /// <returns>Returns the <see cref="DbParameter"/> object located at this index</returns>
        DbParameter GetParameter(int index);
        /// <summary>
        /// Retrieves the entire <see cref="List{T}"/> of <see cref="DbParameter"/> that are currently in use
        /// </summary>
        /// <returns>Returns a <see cref="List{T}"/> of <see cref="DbParameter"/></returns>
        List<DbParameter> GetAllParameters();
        /// <summary>
        /// Adds a new parameter to the parameters collection
        /// </summary>
        /// <param name="type">The data type of the parameter being sent to the data store with the query</param>
        /// <param name="size">The maximum size, in bytes, of the data being sent to the datastore.  If parameter is a variable length don't set for input parameters</param>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="parameterValue">The value of the parameter</param>
        /// <param name="paramDirection">The direction of the parameter, defaults to input.  The size must be set for output parameters</param>
        /// <returns>Returns a <see cref="DbParameter"/></returns>
        DbParameter AddParameter(string parameterName, object parameterValue, DbType type, int? size = null, ParameterDirection paramDirection = ParameterDirection.Input);
        /// <summary>
        /// Adds the passed in parameter to the parameters collection
        /// </summary>
        /// <param name="param">An instance of the <see cref="DbParameter"/> object, that is created the by the caller</param>
        /// <returns>Returns a <see cref="DbParameter"/></returns>
        DbParameter AddParameter(DbParameter param);
        /// <summary>
        /// Adds a new parameter using the name and value passed into the routine
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="value">The value of the parameter</param>
        /// <returns>Returns a <see cref="DbParameter"/></returns>
        DbParameter AddParameter(string name, object value);
        /// <summary>
        /// Sets the value of an existing <see cref="DbParameter"/> by using the <paramref name="parameterName"/> and passed in <paramref name="value"/>
        /// </summary>
        /// <param name="parameterName">The name of the parameter to identify the parameter</param>
        /// <param name="value">The value of the parameter as an <see cref="object"/></param>
        void SetParamaterValue(string parameterName, object value);
        /// <summary>
        /// Sets the value of an existing <see cref="DbParameter"/> by using the <paramref name="index"/> and passed in <paramref name="value"/>
        /// </summary>
        /// <param name="index">The index of the parameter in the parameters collection to identify the parameter to retrieve from the collection</param>
        /// <param name="value">The value of the parameter as an <see cref="object"/></param>
        void SetParamaterValue(int index, object value);
        /// <summary>
        /// Adds an <see cref="IEnumerable{DbParameter}"/> objects to the helpers underlying db parameter collection
        /// </summary>
        /// <param name="dbParams">An <see cref="IEnumerable{DbParameter}"/> to add to the underlying db parameter collection for the connection</param>
        void AddParameterRange(IEnumerable<DbParameter> dbParams);
        /// <summary>
        /// Adds an <see cref="IDictionary{TKey, TValue}"/> object to add to the helpers underlying db parameter collection
        /// </summary>
        /// <param name="dbParams">An <see cref="IDictionary{TKey, TValue}"/> of <see cref="KeyValuePair{TKey, TValue}"/> where the key is a parameter name and the value is the value of a parameter</param>
        void AddParameterRange(IDictionary<string, object> dbParams);
        /// <summary>
        /// Checks for a paremeter in the parameters list with the passed in name
        /// </summary>
        /// <param name="parameterName">The name of the parameter to use when searching the Parameters list</param>
        /// <returns>True if this parameter exists in the parameters collection, false otherwise</returns>
        bool Contains(string parameterName);
        /// <summary>
        /// Checks for a paremeter in the parameters list with the passed in index
        /// </summary>
        /// <param name="index">The index of the parameter in the parameters collection to identify the parameter to remove from the collection</param>
        /// <returns>Returns true if item was found in the paramerters collection, false otherwise if item was not found in the collection</returns>
        bool Contains(int index);
        #endregion
    }
}
