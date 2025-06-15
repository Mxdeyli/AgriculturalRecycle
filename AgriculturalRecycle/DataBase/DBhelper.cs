using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgriculturalRecycle
{
    class DBhelper
    {
        private static MySqlConnection myConnection;
        private static string connectionString = "server=127.0.0.1;uid=root;pwd=123456;database=AgriculturalRecycledb";
        public static MySqlConnection GetConnection()
        {
            myConnection = new MySqlConnection(connectionString);
            return myConnection;
        }
        public static DataTable ExecuteQuery(string sql, params MySqlParameter[] parameters) //执行查询操作
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    if (parameters != null && parameters.Length > 0)
                        cmd.Parameters.AddRange(parameters);
                    DataTable dt = new DataTable();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public static int ExecuteNonQuery(string sql, params MySqlParameter[] parameters) //执行非查询操作
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    if (parameters != null && parameters.Length > 0)
                        cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public static object ExecuteScalar(string sql, MySqlParameter[] parameters)//执行查询并返回单个值
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteScalar();
                }
            }
        }

        public static object ExecuteScalar(string sql, MySqlParameter mySqlParameter)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(mySqlParameter);
                    return cmd.ExecuteScalar();
                }
            }
        }

        internal static int ExecuteScalar(string sqlcheck, MySqlParameter mySqlParameter1, MySqlParameter mySqlParameter2)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sqlcheck, conn))
                {
                    cmd.Parameters.Add(mySqlParameter1);
                    cmd.Parameters.Add(mySqlParameter2);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
    }
}
