using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;

namespace NaflimHelperLibrary
{
    public class SQLserHelper
    {
        //连接数据库
        string conStr;

        public SQLserHelper(string con)
        {
            conStr = con;
        }
        /// <summary>
        /// 读取数据库
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL语句参数</param>
        /// <returns></returns>
        public DataTable GetData(string sql, params SqlParameter[] parameters)
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conStr);
                if (parameters != null)
                    adapter.SelectCommand.Parameters.AddRange(parameters);
                DataTable data = new DataTable();
                adapter.Fill(data);
                adapter.Dispose();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 对数据库增删改
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="index">选择数据库</param>
        /// <param name="parameters">SQL语句条件</param>
        /// <returns></returns>
        public bool GetExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);
                    if (parameters != null)
                        command.Parameters.AddRange(parameters);
                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class MySQLHelper
    {
        //连接数据库
        string conStr;

        public MySQLHelper(string con)
        {
            conStr = con;
        }

        /// <summary>
        /// 对数据库增删改
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public bool GetExecuteNonQuery(string sql)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(conStr))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    MySqlCommand command = new MySqlCommand(sql, connection);
                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 对数据库增删改
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL语句条件</param>
        /// <returns></returns>
        public bool GetExecuteNonQuery(string sql, params MySqlParameter[] parameters)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(conStr))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    MySqlCommand command = new MySqlCommand(sql, connection);
                    if (parameters != null)
                        command.Parameters.AddRange(parameters);
                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
