using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.SqlClient;

namespace NaflimHelperLibrary
{
    /// <summary>
    /// SQLServer帮助类
    /// </summary>
    public class SQLServerHelper
    {
        //连接数据库
        string conStr;

        public SQLServerHelper(string con)
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
            SqlDataAdapter adapter = new SqlDataAdapter(sql, conStr);
            if (parameters != null)
                adapter.SelectCommand.Parameters.AddRange(parameters);
            DataTable data = new DataTable();
            adapter.Fill(data);
            adapter.Dispose();
            return data;
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
    }

    /// <summary>
    /// MySQL帮助类
    /// </summary>
    public class MySQLHelper
    {
        //连接数据库
        string conStr;

        public MySQLHelper(string con)
        {
            conStr = con;
        }

        /// <summary>
        /// 读取数据库
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL语句参数</param>
        /// <returns></returns>
        public DataTable GetData(string sql, params MySqlParameter[] parameters)
        {
            using (MySqlConnection connection = new MySqlConnection(conStr))
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conStr);
                if (parameters != null)
                    adapter.SelectCommand.Parameters.AddRange(parameters);
                DataTable data = new DataTable();
                adapter.Fill(data);
                adapter.Dispose();
                return data;
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
    }

    /// <summary>
    /// Oracle帮助类
    /// </summary>
    public class OracleHelpher
    {
        //连接数据库
        string conStr;

        public OracleHelpher(string con)
        {
            conStr = con;
        }

        /// <summary>
        /// 读取数据库
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL语句参数</param>
        /// <returns></returns>
        public DataTable GetData(string sql, params OracleParameter[] parameters)
        {
            using (OracleConnection connection = new OracleConnection(conStr))
            {
                OracleDataAdapter adapter = new OracleDataAdapter(sql, conStr);
                if (parameters != null)
                    adapter.SelectCommand.Parameters.AddRange(parameters);
                DataTable data = new DataTable();
                adapter.Fill(data);
                adapter.Dispose();
                return data;
            }
        }

        /// <summary>
        /// 对数据库增删改
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL语句条件</param>
        /// <returns></returns>
        public bool GetExecuteNonQuery(string sql, params OracleParameter[] parameters)
        {
            using (OracleConnection connection = new OracleConnection(conStr))
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                OracleCommand command = new OracleCommand(sql, connection);
                if (parameters != null)
                    command.Parameters.AddRange(parameters);
                return command.ExecuteNonQuery() > 0;
            }
        }
    }
}
