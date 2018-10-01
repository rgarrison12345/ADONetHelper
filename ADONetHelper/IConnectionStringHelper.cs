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
#endregion

namespace ADONetHelper
{
    /// <summary>
    /// Contract class to modify/retrieve values in a connection string
    /// </summary>
    public interface IConnectionStringHelper
    {
        /// <summary>
        /// Adds a property name and value to the current connection string
        /// </summary>
        /// <param name="name">The name of the connection string property</param>
        /// <param name="value">The value to use with the connection string property</param>
        void AddConnectionStringProperty(string name, object value);
        /// <summary>
        /// Removes a connection string property from the connection string by name
        /// </summary>
        /// <param name="name">The name of the connection string property</param>
        void RemoveConnectionStringProperty(string name);
        /// <summary>
        /// Retrieves a connection string property value as an object
        /// </summary>
        /// <param name="name">The name of the connection string property</param>
        /// <returns>Returns a connection string property as an <see cref="object"/></returns>
        object GetConnectionStringPropertyValue(string name);
        /// <summary>
        /// Configures the connection string with the key value pairs passed into the routine
        /// </summary>
        /// <param name="properties">Key value pairs of connection string property names and values</param>
        void ConfigureConnectionString(IDictionary<string, object> properties);
        /// <summary>
        /// Clears the connection string of all key value pairs
        /// </summary>
        void Clear();
    }
}
