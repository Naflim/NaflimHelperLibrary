using System;
using System.Data;
using System.Data.SqlClient;

namespace NaflimHelperLibrary
{
    public class DBhelper
    {
        //连接数据库
        static readonly string conStr = "Data Source=sodsofta.myzonten.com,2020;Initial Catalog=ssmjdata;User ID=sa;Password=ZHENG2020zt;";

        /// <summary>
        /// 读取数据库
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="index">选择数据库</param>
        /// <param name="parameters">SQL语句参数</param>
        /// <returns></returns>
        public static DataTable GetData(string sql, params SqlParameter[] parameters)
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
        public static bool GetExecuteNonQuery(string sql, params SqlParameter[] parameters)
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
}
