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

        public static string GenerateCVV()
        {
            var random = new Random();
            string chars = "123456789";
            string generatedCVV = "";

            for(int i = 0; i < 3; i++)
            {
                generatedCVV += chars[random.Next(chars.Length)];
            }
            return generatedCVV;
        }
    }
}
