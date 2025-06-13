using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgriculturalRecycle
{
    public class Item
    {
        public Item(int e1,string e2,int e3,int e4)
        {
            E1 = e1;
            E2 = e2;
            E3 = e3;
            E4 = e4;
        }

        public int EquipmentID { get { return  E1; } set {value=E1; } }
        public string ProductName { get { return E2; } set { value = E2; } }
        public int ProductID { get { return E3; } set { value = E3; } }
        public int CategoryID { get { return E4; } set { value = E4; } }
        public int E1 { get;}
        public string E2 { get; }
        public int E3 { get; }
        public int E4 { get; }

        internal void Turnout()
        {
            this.EquipmentID = 0;
            this.ProductName = null;
            this.ProductID = 0;
            this.CategoryID = 0;
        }
    }
}
