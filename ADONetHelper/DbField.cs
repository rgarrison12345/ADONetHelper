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
using System;
#endregion

namespace ADONetHelper
{
    /// <summary>
    /// Attribute class that defines a field that goes into and comes out of a database
    /// </summary>
    /// <seealso cref="Attribute"/>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DbField : Attribute
    {
        #region Fields/Properties
        private readonly object _valueIfNull = null;
        private readonly string _databaseFielName = "";

        /// <summary>
        /// The default value as a <see cref="object"/> in the instance where a value from the database is <see cref="DBNull.Value"/>
        /// </summary>
        public object DefaultValueIfNull
        {
            get
            {
                return _valueIfNull;
            }
        }
        /// <summary>
        /// The name of a field that is being pulled from a query
        /// </summary>
        public string DatabaseFieldName
        {
            get
            {
                return _databaseFielName;
            }
        }
        #endregion
        #region Constuctors
        /// <summary>
        /// Initializes a new instance of <see cref="DbField"/>
        /// </summary>
        /// <param name="dbFieldName">The name of a field that exists in a database table</param>
        /// <param name="valueIfNull">The default value if the field coming from a query has a value of <see cref="DBNull.Value"/></param>
        public DbField(string dbFieldName, object valueIfNull = null)
        {
            _valueIfNull = valueIfNull;
            _databaseFielName = dbFieldName;
        }
        #endregion
    }
}
