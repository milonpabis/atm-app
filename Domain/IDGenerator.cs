using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMLogic
{
    public static class IDGenerator
    {
        
        public static string GenerateID()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
            string generatedID = "";
            var random = new Random();

            for(int i = 0; i < 32; i++)
            {
                var a = chars[random.Next(chars.Length)];
                generatedID += a;
            }
            return generatedID;
        }
    }
}
