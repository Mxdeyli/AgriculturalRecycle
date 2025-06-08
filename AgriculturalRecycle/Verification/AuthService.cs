using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AgriculturalRecycle
{
    class AuthService
    {
        public static User ValidateUser(string account, string password)
        {
            string sql = @"SELECT UserID, Account, UserType, Password
                      FROM users WHERE Account = @account";
            var dt = DBhelper.ExecuteQuery(sql,
                new MySqlParameter("@account", account));

            if (dt.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                var row = dt.Rows[0];
                string BCryptHash = row["Password"].ToString();
                bool isVerified = BCrypt.Net.BCrypt.Verify(password, BCryptHash);
                if (!isVerified)
                {
                    return null;
                }
                else
                {
                    return new User(
                        Convert.ToInt32(row["UserID"]),
                        row["Account"].ToString(),
                        row["UserType"].ToString()
                    );
                }
            }
                
        }
    }
}
