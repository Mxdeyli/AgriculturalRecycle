using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace AgriculturalRecycle.Encryption
{
    class BCrypt_encryption
    {
        private static string  password = "Admin@123";
        private string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

    }
}
