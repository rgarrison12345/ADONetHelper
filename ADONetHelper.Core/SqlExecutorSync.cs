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
#region Using Declarations
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
#endregion

namespace ADONetHelper.Core
{
    public partial class SqlExecutor
    {
        #region Data Retrieval
        /// <summary>
        /// Gets an instance of <see cref="DataSet"/>
        /// </summary>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="query">SQL query to use to build a <see cref="DataSet"/></param>
        /// <returns>Returns an instance of <see cref="DataSet"/> based on the <paramref name="query"/> passed into the routine</returns>
        public DataSet GetDataSet(CommandType queryCommandType, string query)
        {
            //Return this back to the caller
            return GetDataSet(queryCommandType, query, Connection);
        }
        /// <summary>
        /// Gets an instance of <see cref="DataSet"/>
        /// </summary>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="connection">An instance of <see cref="DbConnection"/></param>
        /// <param name="query">SQL query to use to build a <see cref="DataSet"/></param>
        /// <returns>Returns an instance of <see cref="DataSet"/> based on the <paramref name="query"/> passed into the routine</returns>
        public DataSet GetDataSet(CommandType queryCommandType, string query, DbConnection connection)
        {
            //Open the database connection
            Utilities.OpenDbConnection(connection);

            //Wrap this automatically to dispose of resources
            using (DbDataAdapter adap = Factory.GetDbDataAdapter())
            {
                //Wrap this automatically to dispose of resources
                using (DbCommand command = Factory.GetDbCommand(queryCommandType, query, Parameters, connection, CommandTimeout))
                {
                    DataSet set = new DataSet();

                    //Fill out the dataset
                    adap.SelectCommand = command;
                    adap.Fill(set);

                    //Return this back to the caller
                    return set;
                }
            }
        }
        /// <summary>
        /// Gets an instance of <see cref="DataTable"/>
        /// </summary>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="query">SQL query to use to build a result set</param>
        /// <returns>Returns an instance of <see cref="DataTable"/></returns>
        public DataTable GetDataTable(CommandType queryCommandType, string query)
        {
            //Return this back to the caller
            return GetDataTable(queryCommandType, query, Connection);
        }
        /// <summary>
        /// Gets an instance of <see cref="DataTable"/>
        /// </summary>
        /// <param name="connection">An instance of <see cref="DbConnection"/></param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="query">SQL query to use to build a result set</param>
        /// <returns>Returns an instance of <see cref="DataTable"/></returns>
        public DataTable GetDataTable(CommandType queryCommandType, string query, DbConnection connection)
        {
            //Open the database connection
            Utilities.OpenDbConnection(connection);

            //Return this back to the caller
            using (DbDataReader reader = GetDbDataReader(queryCommandType, query, Connection))
            {
                DataTable dt = new DataTable();

                //Load in the result set
                dt.Load(reader);

                //Return this back to the caller
                return dt;
            }
        }
        /// <summary>
        /// Gets a single instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants created from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns an instance of the <typeparamref name="T"/> based on the fields in the passed in query.  Returns the default value for the <typeparamref name="T"/> if a record is not found</returns>
        public T GetDataObject<T>(CommandType queryCommandType, string query) where T : class
        {
            //Return this back to the caller
            return GetDataObject<T>(queryCommandType, query, Connection);
        }
        /// <summary>
        /// Gets a single instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants created from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <returns>Returns an instance of the <typeparamref name="T"/> based on the fields in the passed in query.  Returns the default value for the <typeparamref name="T"/> if a record is not found</returns>
        public T GetDataObject<T>(CommandType queryCommandType, string query, string connectionString) where T : class
        {
            //Wrap this in a using statement to automatically dispose of resources
            using (DbConnection connection = Factory.GetDbConnection(connectionString))
            {
                //Return this back to the caller
                return GetDataObject<T>(queryCommandType, query, connection);
            }
        }
        /// <summary>
        /// Gets a single instance of <typeparamref name="T"/> based on the <paramref name="query"/> passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants created from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="connection">An instance of a <see cref="DbConnection"/> object to use to query a datastore</param>
        /// <returns>Returns an instance of the <typeparamref name="T"/> based on the fields in the passed in query.  Returns the default value for the <typeparamref name="T"/> if a record is not found</returns>
        public T GetDataObject<T>(CommandType queryCommandType, string query, DbConnection connection) where T : class
        {
            //Open the database connection if necessary
            Utilities.OpenDbConnection(connection);

            //Wrap this to automatically handle disposing of resources
            using (DbDataReader reader = GetDbDataReader(queryCommandType, query, connection, CommandBehavior.SingleRow))
            {
                //Get the field name and value pairs out of this query
                IEnumerable<IDictionary<string, object>> results = Utilities.GetDynamicResultsEnumerable(reader);
                IEnumerator<IDictionary<string, object>> enumerator = results.GetEnumerator();
                bool canMove = enumerator.MoveNext();

                //Check if we need to return the default for the type
                if (canMove == false)
                {
                    //Return the default for the type back to the caller
                    return default;
                }
                else
                {
                    //Return this back to the caller
                    return Utilities.GetSingleDynamicType<T>(enumerator.Current);
                }
            }
        }
        /// <summary>
        /// Gets a <see cref="List{T}"/> of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants created from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns a <see cref="List{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public List<T> GetDataObjectList<T>(CommandType queryCommandType, string query)
        {
            //Return this back to the caller
            return GetDataObjectList<T>(queryCommandType, query, Connection);
        }
        /// <summary>
        /// Gets a <see cref="List{T}"/> of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants created from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <returns>Returns a <see cref="List{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public List<T> GetDataObjectList<T>(CommandType queryCommandType, string query, string connectionString)
        {
            //Wrap this in a using statement to automatically dispose of resources
            using (DbConnection connection = Factory.GetDbConnection(connectionString))
            {
                //Return this back to the caller
                return GetDataObjectList<T>(queryCommandType, query, connection);
            }
        }
        /// <summary>
        /// Gets a <see cref="List{T}"/> of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants created from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="connection">An instance of a <see cref="DbConnection"/> object to use to query a datastore</param>
        /// <returns>Returns a <see cref="List{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public List<T> GetDataObjectList<T>(CommandType queryCommandType, string query, DbConnection connection)
        {
            //Open the database connection if necessary
            Utilities.OpenDbConnection(connection);

            //Wrap this to automatically handle disposing of resources
            using (DbDataReader reader = GetDbDataReader(queryCommandType, query, connection, CommandBehavior.SingleResult))
            {
                //Return this back to the caller
                return Utilities.GetDynamicTypeList<T>(Utilities.GetDynamicResultsList(reader));
            }
        }
        /// <summary>
        /// Gets a <see cref="IEnumerable{T}"/> of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants created from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns a <see cref="IEnumerable{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public IEnumerable<T> GetDataObjectEnumerable<T>(CommandType queryCommandType, string query)
        {
            //Return this back to the caller
            return GetDataObjectEnumerable<T>(queryCommandType, query, Connection);
        }
        /// <summary>
        /// Gets a list of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants created from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <returns>Returns a <see cref="IEnumerable{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public IEnumerable<T> GetDataObjectEnumerable<T>(CommandType queryCommandType, string query, string connectionString)
        {
            //Wrap this in a using statement to automatically dispose of resources
            using (DbConnection connection = Factory.GetDbConnection(connectionString))
            {
                //Return this back to the caller
                return GetDataObjectEnumerable<T>(queryCommandType, query, connection);
            }
        }
        /// <summary>
        /// Gets a list of the type parameter object that creates an object based on the query passed into the routine
        /// </summary>
        /// <typeparam name="T">An instance of the type caller wants created from the query passed into procedure</typeparam>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="connection">An instance of a <see cref="DbConnection"/> object to use to query a datastore</param>
        /// <returns>Returns a <see cref="IEnumerable{T}"/> based on the results of the passed in <paramref name="query"/></returns>
        public IEnumerable<T> GetDataObjectEnumerable<T>(CommandType queryCommandType, string query, DbConnection connection)
        {
            //Open the database connection if necessary
            Utilities.OpenDbConnection(connection);

            //Wrap this to automatically handle disposing of resources
            using (DbDataReader reader = GetDbDataReader(queryCommandType, query, connection, CommandBehavior.SingleResult))
            {
                //Get the field name and value pairs out of this query
                IEnumerable<IDictionary<string, object>> results = Utilities.GetDynamicResultsEnumerable(reader);
                IEnumerator<IDictionary<string, object>> enumerator = results.GetEnumerator();

                //Keep moving through the enumerator
                while (enumerator.MoveNext() == true)
                {
                    //Return this back to the caller
                    yield return Utilities.GetSingleDynamicType<T>(enumerator.Current);
                }
            }
        }
        /// <summary>
        /// Utility method for returning a <see cref="DbDataReader"/> object created
        /// </summary>
        /// <param name="transact">An instance of <see cref="DbTransaction"/></param>
        /// <param name="behavior">Provides a description of the results of the query and its effect on the database.  Defaults to <see cref="CommandBehavior.CloseConnection"/></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>A <see cref="DbDataReader"/> object, the caller is responsible for handling closing the DataReader.  Once the data reader is closed, the underlying database connection will be closed as well</returns>
        public DbDataReader GetDbDataReader(CommandType queryCommandType, string query, CommandBehavior behavior = CommandBehavior.CloseConnection, DbTransaction transact = null)
        {
            //Return this back to the caller
            return GetDbDataReader(queryCommandType, query, Connection, behavior, transact);
        }
        /// <summary>
        /// Utility method for returning a <see cref="DbDataReader"/> object created from the passed in Db Helper object
        /// </summary>
        /// <param name="behavior">Provides a description of the results of the query and its effect on the database.  Defaults to <see cref="CommandBehavior.CloseConnection"/></param>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns an instance of <see cref="DbDataReader"/> object, the caller is responsible for handling closing the DataReader.  Once the data reader is closed, the underlying database connection will be closed as well</returns>
        public DbDataReader GetDbDataReader(CommandType queryCommandType, string connectionString, string query, CommandBehavior behavior = CommandBehavior.CloseConnection)
        {
            //Wrap this in a using statement to handle disposing of resources
            DbConnection connection = Factory.GetDbConnection(connectionString);

            //Return the reader back to the caller
            return GetDbDataReader(queryCommandType, query, connection, behavior);
        }
        /// <summary>
        /// Utility method for returning a <see cref="DbDataReader"/> object
        /// </summary>
        /// <param name="transact">An instance of <see cref="DbTransaction"/></param>
        /// <param name="behavior">Provides a description of the results of the query and its effect on the database.  Defaults to <see cref="CommandBehavior.CloseConnection"/></param>
        /// <param name="connection">An instance of a <see cref="DbConnection"/> object to use to query a datastore</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns an instance of <see cref="DbDataReader"/> object, the caller is responsible for handling closing the DataReader</returns>
        public DbDataReader GetDbDataReader(CommandType queryCommandType, string query, DbConnection connection, CommandBehavior behavior = CommandBehavior.CloseConnection, DbTransaction transact = null)
        {
            //Open the database connection if necessary
            Utilities.OpenDbConnection(connection);

            //Wrap this in a using statement to handle disposing of resources
            using (DbCommand command = Factory.GetDbCommand(queryCommandType, query, Parameters, connection, CommandTimeout, transact))
            {
                //Return this back to the caller
                return command.ExecuteReader(behavior);
            }
        }
        /// <summary>
        /// Utility method for acting on a <see cref="DbDataReader"/>
        /// </summary>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="act">Action methods that takes in a <see cref="DbDataReader"/></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <returns>A <see cref="DbDataReader"/> object, the caller is responsible for handling closing the <see cref="DbDataReader"/>.  Once the data reader is closed, the database connection will be closed as well</returns>
        public void GetDbDataReader(CommandType queryCommandType, string query, Action<DbDataReader> act)
        {
            //Return this back to the caller
            GetDbDataReader(queryCommandType, query, act, Connection);
        }
        /// <summary>
        /// Utility method for returning a <see cref="DbDataReader"/>
        /// </summary>
        /// <param name="act"></param>
        /// <param name="connection">An instance of a <see cref="DbConnection"/> object to use to query a datastore</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>A <see cref="DbDataReader"/> object, the caller is responsible for handling closing the DataReader</returns>
        public void GetDbDataReader(CommandType queryCommandType, string query, Action<DbDataReader> act, DbConnection connection)
        {
            //Open the database connection if necessary
            Utilities.OpenDbConnection(connection);

            //Wrap this in a using statement to handle disposing of resources
            using (DbCommand command = Factory.GetDbCommand(queryCommandType, query, Parameters, connection, CommandTimeout))
            {
                //Invoke the method
                act(command.ExecuteReader());
            }
        }
        /// <summary>
        /// Utility method for returning a scalar value from the database
        /// </summary>
        /// <param name="transact">An instance of <see cref="DbTransaction"/></param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the value of the first column in the first row returned from the passed in query as an object</returns>
        public object GetScalarValue(CommandType queryCommandType, string query, DbTransaction transact = null)
        {
            //Return this back to the caller
            return GetScalarValue(queryCommandType, query, Connection, transact);
        }
        /// <summary>
        /// Utility method for returning a scalar value from the database
        /// </summary>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the value of the first column in the first row returned from the passed in query as an object</returns>
        public object GetScalarValue(CommandType queryCommandType, string query, string connectionString)
        {
            //Wrap this in a using statement to handle disposing of resources
            using (DbConnection connection = Factory.GetDbConnection(connectionString))
            {
                //Return this back to the caller
                return GetScalarValue(queryCommandType, query, connection);
            }
        }
        /// <summary>
        /// Utility method for returning a scalar value from the database
        /// </summary>
        /// <param name="transact">An instance of <see cref="DbTransaction"/></param>
        /// <param name="connection">An instance of a <see cref="DbConnection"/> object to use to query a datastore</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the value of the first column in the first row returned from the passed in query as an object</returns>
        public object GetScalarValue(CommandType queryCommandType, string query, DbConnection connection, DbTransaction transact = null)
        {
            //Open the connection to the database
            Utilities.OpenDbConnection(connection);

            //Wrap this in a using statement to handle disposing of resources
            using (DbCommand command = Factory.GetDbCommand(queryCommandType, query, Parameters, connection, CommandTimeout, transact))
            {
                //Return this back to the caller
                return command.ExecuteScalar();
            }
        }
        #endregion
        #region Data Modification
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the number of rows affected by this query</returns>
        public int ExecuteNonQuery(CommandType queryCommandType, string query)
        {
            //Execute this query
            return ExecuteNonQuery(queryCommandType, query, Connection);
        }
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the number of rows affected by this query</returns>
        public int ExecuteNonQuery(CommandType queryCommandType, string query, string connectionString)
        {
            //Wrap this in a using statement to handle disposing of resources
            using (DbConnection connection = Factory.GetDbConnection(connectionString))
            {
                //Execute this query
                return ExecuteNonQuery(queryCommandType, query, connection);
            }
        }
        /// <summary>
        /// Utility method for executing an Ad-Hoc query or stored procedure without a transaction
        /// </summary>
        /// <param name="connection">An instance of a <see cref="DbConnection"/> object to use to query a datastore</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the number of rows affected by this query</returns>
        public int ExecuteNonQuery(CommandType queryCommandType, string query, DbConnection connection)
        {
            //Open the database connection
            Utilities.OpenDbConnection(connection);

            //Wrap this in a using statement to automatically handle disposing of resources
            using (DbCommand command = Factory.GetDbCommand(queryCommandType, query, Parameters, connection, CommandTimeout))
            {
                //Return the amount of records affected by this query back to the caller
                return command.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Utility method for executing batches of queries or stored procedures in a SQL transaction
        /// </summary>
        /// <param name="commands">The list of query database parameters that are associated with a query</param>
        /// <returns>Returns the number of rows affected by all queries passed in</returns>
        public List<int> ExecuteBatchedNonQuery(IEnumerable<SQLQuery> commands)
        {
            //Return this back to the caller
            return ExecuteBatchedNonQuery(commands, Connection);
        }
        /// <summary>
        /// Utility method for executing batches of queries or stored procedures in a SQL transaction
        /// </summary>
        /// <param name="commands">The list of query database parameters that are associated with a query</param>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <returns>Returns the number of rows affected by all queries passed in</returns>
        public List<int> ExecuteBatchedNonQuery(string connectionString, IEnumerable<SQLQuery> commands)
        {
            //Open the connection to the database if necessary
            using (DbConnection connection = Factory.GetDbConnection(connectionString))
            {
                //Return this back to the caller
                return ExecuteBatchedNonQuery(commands, connection);
            }
        }
        /// <summary>
        /// Utility method for executing batches of queries or stored procedures in a SQL transaction
        /// </summary>
        /// <param name="commands">The list of query database parameters that are associated with a query</param>
        /// <param name="connection">An instance of a <see cref="DbConnection"/> object to use to query a datastore</param>
        /// <returns>Returns the number of rows affected by all queries passed in, assuming all are succesful</returns>
        public List<int> ExecuteBatchedNonQuery(IEnumerable<SQLQuery> commands, DbConnection connection)
        {
            List<int> returnList = new List<int>();

            //Open the database connection
            Utilities.OpenDbConnection(connection);

            //Wrap this in a using statement to automatically handle disposing of resources
            using (DbCommand command = Factory.GetDbCommand(CommandTimeout))
            {
                command.Connection = connection;

                //Now loop through all queries
                foreach (SQLQuery query in commands)
                {
                    //Set query information for this query
                    command.CommandType = query.QueryType;
                    command.CommandText = query.QueryText;
                    command.Parameters.AddRange(query.ParameterList.ToArray());

                    //Now execute the query
                    returnList.Add(command.ExecuteNonQuery());
                }
            }

            //Return this back to the caller
            return returnList;
        }
        /// <summary>
        /// Utility method for executing a query or stored procedure in a SQL transaction
        /// </summary>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the number of rows affected by this query</returns>
        public int ExecuteTransactedNonQuery(CommandType queryCommandType, string query)
        {
            //Return this back to the caller
            return ExecuteTransactedNonQuery(queryCommandType, query, Connection);
        }
        /// <summary>
        /// Utility method for executing a query or stored procedure in a SQL transaction
        /// </summary>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the number of rows affected by this query</returns>
        public int ExecuteTransactedNonQuery(CommandType queryCommandType, string query, string connectionString)
        {
            //Wrap this in a using statement to automatically handle disposing of resources
            using (DbConnection connection = Factory.GetDbConnection(connectionString))
            {
                //Open the database connection
                Utilities.OpenDbConnection(connection);

                //Wrap this in a using statement to automatically handle disposing of resources
                using (DbTransaction transact = Factory.GetDbTransaction(connection))
                {
                    //Get records affected
                    return ExecuteTransactedNonQuery(queryCommandType, connection, transact, query, true);
                }
            }
        }
        /// <summary>
        /// Utility method for executing a query or stored procedure in a SQL transaction
        /// </summary>
        /// <param name="transact">An instance of a <see cref="DbTransaction"/> class</param>
        /// <param name="connection">An instance of a <see cref="DbConnection"/> object to use to query a datastore</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the number of rows affected by this query</returns>
        public int ExecuteTransactedNonQuery(CommandType queryCommandType, string query, DbConnection connection, DbTransaction transact)
        {
            //Return this back to the caller
            return ExecuteTransactedNonQuery(queryCommandType, query, connection, transact);
        }
        /// <summary>
        /// Utility method for executing a query or stored procedure in a SQL transaction
        /// </summary>
        /// <param name="connection">An instance of a <see cref="DbConnection"/> object to use to query a datastore</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <returns>Returns the number of rows affected by this query</returns>
        public int ExecuteTransactedNonQuery(CommandType queryCommandType, string query, DbConnection connection)
        {
            //Open the database connection
            Utilities.OpenDbConnection(connection);

            //Wrap this in a using statement to automatically handle disposing of resources
            using (DbTransaction transact = Factory.GetDbTransaction(connection))
            {
                //Get recrods affected
                return ExecuteTransactedNonQuery(queryCommandType, connection, transact, query);
            }
        }
        /// <summary>
        /// Utility method for executing a query or stored procedure in a SQL transaction
        /// </summary>
        /// <param name="transact">An instance of a <see cref="DbTransaction"/> class</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="commitTransaction">Whether or not to commit the transaction that was passed in if successful</param>
        /// <returns>Returns the number of rows affected by this query</returns>
        public int ExecuteTransactedNonQuery(CommandType queryCommandType, DbTransaction transact, string query, bool commitTransaction = true)
        {
            //Return this back to the caller
            return ExecuteTransactedNonQuery(queryCommandType, Connection, transact, query, commitTransaction);
        }
        /// <summary>
        /// Utility method for executing a query or stored procedure in a SQL transaction
        /// </summary>
        /// <param name="transact">An instance of a <see cref="DbTransaction"/> class</param>
        /// <param name="connection">An instance of a <see cref="DbConnection"/> object to use to query a datastore</param>
        /// <param name="query">The query command text or name of stored procedure to execute against the data store</param>
        /// <param name="queryCommandType">Represents how a command should be interpreted by the data provider</param>
        /// <param name="commitTransaction">Whether or not to commit the transaction that was passed in if successful</param>
        /// <returns>Returns the number of rows affected by this query</returns>
        public int ExecuteTransactedNonQuery(CommandType queryCommandType, DbConnection connection, DbTransaction transact, string query, bool commitTransaction = true)
        {
            int recordsAffected = 0;

            //Open the database connection
            Utilities.OpenDbConnection(connection);

            //Wrap this in a using statement to automatically handle disposing of resources
            using (DbCommand command = Factory.GetDbCommand(queryCommandType, query, Parameters, connection, CommandTimeout, transact))
            {
                try
                {
                    //Now execute the query
                    recordsAffected = command.ExecuteNonQuery();

                    //Only commit if specified
                    if (commitTransaction == true)
                    {
                        transact.Commit();
                    }
                }
                catch (Exception outerEx)
                {
                    try
                    {
                        //Automatically roll back the error
                        transact.Rollback();

                        //Need to throw this up the stack
                        throw;
                    }
                    catch (Exception innerEx)
                    {
                        throw;
                    }
                }
            }

            //Return this back to the caller
            return recordsAffected;
        }
        /// <summary>
        /// Utility method for executing batches of queries or stored procedures in a SQL transaction
        /// </summary>
        /// <param name="commands">The list of query database parameters that are associated with a query</param>
        /// <returns>Returns the number of rows affected by all queries passed in, assuming all are succesful</returns>
        public List<int> ExecuteTransactedBatchedNonQuery(IEnumerable<SQLQuery> commands)
        {
            //Return this back to the caller
            return ExecuteTransactedBatchedNonQuery(commands, Connection);
        }
        /// <summary>
        /// Utility method for executing batches of queries or stored procedures in a SQL transaction
        /// </summary>
        /// <param name="transact">An instance of a <see cref="DbTransaction"/> class</param>
        /// <param name="commands">The list of query database parameters that are associated with a query</param>
        /// <returns>Returns the number of rows affected by all queries passed in, assuming all are succesful</returns>
        public List<int> ExecuteTransactedBatchedNonQuery(IEnumerable<SQLQuery> commands, DbTransaction transact)
        {
            //Return this back to the caller
            return ExecuteTransactedBatchedNonQuery(commands, transact);
        }
        /// <summary>
        /// Utility method for executing batches of queries or stored procedures in a SQL transaction
        /// </summary>
        /// <param name="connectionString">The connection string used to query a data store</param>
        /// <param name="commands">The list of query database parameters that are associated with a query</param>
        /// <returns>Returns the number of rows affected by all queries passed in, assuming all are succesful</returns>
        public List<int> ExecuteTransactedBatchedNonQuery(string connectionString, IEnumerable<SQLQuery> commands)
        {
            //Wrap this in a using statement to automatically handle disposing of resources
            using (DbConnection connection = Factory.GetDbConnection(connectionString))
            {
                connection.Open();

                //Wrap this in a using statement to automatically handle disposing of resources
                using (DbTransaction transact = Factory.GetDbTransaction(connection))
                {
                    return ExecuteTransactedBatchedNonQuery(commands, connection, transact);
                }
            }
        }
        /// <summary>
        /// Utility method for executing batches of queries or stored procedures in a SQL transaction
        /// </summary>
        /// <param name="connection">An instance of a <see cref="DbConnection"/> object to use to query a datastore</param>
        /// <param name="commands">The list of query database parameters that are associated with a query</param>
        /// <returns>Returns the number of rows affected by all queries passed in, assuming all are succesful</returns>
        public List<int> ExecuteTransactedBatchedNonQuery(IEnumerable<SQLQuery> commands, DbConnection connection)
        {
            //Open the database connection
            Utilities.OpenDbConnection(connection);

            //Wrap this in a using statement to automatically handle disposing of resources
            using (DbTransaction transact = Factory.GetDbTransaction(connection))
            {
                return ExecuteTransactedBatchedNonQuery(commands, connection, transact);
            }
        }
        /// <summary>
        /// Utility method for executing batches of queries or stored procedures in a SQL transaction
        /// </summary>
        /// <param name="commands">The list of query database parameters that are associated with a query</param>
        /// <param name="connection">An instance of a <see cref="DbConnection"/> object to use to query a datastore</param>
        /// <param name="transact">An instance of a <see cref="DbTransaction"/> class</param>
        /// <returns>Returns the number of rows affected by all queries passed in, assuming all are succesful</returns>
        public List<int> ExecuteTransactedBatchedNonQuery(IEnumerable<SQLQuery> commands, DbConnection connection, DbTransaction transact)
        {
            List<int> returnList = new List<int>();
            bool commandPrepared = false;

            try
            {
                //Open the database connection
                Utilities.OpenDbConnection(connection);

                //Wrap this in a using statement to automatically handle disposing of resources
                using (DbCommand command = Factory.GetDbCommand(connection, transact, CommandTimeout))
                {
                    //Now loop through all queries
                    foreach (SQLQuery query in commands)
                    {
                        //Set query information for this query
                        command.CommandType = query.QueryType;
                        command.CommandText = query.QueryText;
                        command.Parameters.AddRange(query.ParameterList.ToArray());

                        //Check if we need to prepare the command
                        if (commandPrepared == false)
                        {
                            //Prepare the command
                            command.Prepare();
                        }

                        //Now execute the query
                        returnList.Add(command.ExecuteNonQuery());
                    }

                    //We made it this far, we can commit
                    transact.Commit();
                }
            }
            catch (Exception outerEx)
            {
                try
                {
                    //Automatically roll back
                    transact.Rollback();

                    //Throw this back up the stack
                    throw;
                }
                catch (Exception innerEx)
                {
                    throw;
                }
                finally
                {
                }
            }
            finally
            {

            }

            //Return this back to the caller
            return returnList;
        }
        #endregion
        #region Helper Methods
        #endregion
    }
}