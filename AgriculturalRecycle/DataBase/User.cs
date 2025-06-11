using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgriculturalRecycle
{
    public class User
    {
        public User(int v1, string v2, string v3)
        {
            V1 = v1;
            V2 = v2;
            V3 = v3;
        }

        public int UserID { get {return V1; } set { value = V1; } }
        public string Username { get { return V2; }set { value = V2; } }
        public string UserType { get {return V3; } set { value = V3; } }
        public int V1 { get; }
        public string V2 { get; }
        public string V3 { get; }

        internal void Logout()
        {
            this.UserID = 0;
            this.Username = null;
            this.UserType = null;
        }
    }
}
