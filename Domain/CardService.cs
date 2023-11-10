using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMLogic
{
    public static class CardService
    {
        public static void GenerateCard(User user)
        {
            string[] lines = { user.UserID };
            try
            {
                File.WriteAllLines($"F:\\Desktop\\repos\\C#\\ATM\\Domain\\cards\\{user.Name}.txt", lines);
            }
            catch (Exception ex)
            {

                //
            }
            
        }

        public static User? RetrieveUser(ATM atm, string filePath)
        {
            try
            {
                string UserID = File.ReadAllText(filePath);
                UserID = UserID.Replace("\n", "").Replace("\r", "");
                return atm.RetrieveUser(UserID);
            }
            catch
            {
                return null;
            }
        }
    }
}
