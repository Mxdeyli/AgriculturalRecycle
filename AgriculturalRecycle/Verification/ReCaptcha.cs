using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgriculturalRecycle
{
    class ReCaptcha
    {
        public static string CreateRandomCode(int CodeLength)
        {
            int rand;
            char code;
            string randomCode = String.Empty;//随机验证码
            //生成一定长度的随机验证码
            Random random = new Random();
            for (int i = 0; i < CodeLength; i++)
            {
                rand = random.Next();
                if (rand % 3 == 1)
                {
                    code = (char)('A' + (char)(rand % 26));
                }
                else if (rand % 3 == 2)
                {
                    code = (char)('a' + (char)(rand % 26));
                }
                else
                {
                    code = (char)('0' + (char)(rand % 10));
                }
                randomCode += code.ToString();
            }
            return randomCode;
        }
    }
}
