using System.Reflection.Metadata;

namespace ATMLogic
{
    public class User
    {
        public string CardNumber { get;}
        public string UserID { get; }
        public string PIN { get; }

        public string CVV { get; }

        public string Name { get; }

        public int Amount { get; set; }

        public bool IsLoggedIn { get; set; }

        public User(string cardNumber, string pin, string name, string userId, string cvv, int amount=0, bool online=false )
        {
            CardNumber = cardNumber;
            PIN = pin;
            Name = name;
            Amount = amount;
            IsLoggedIn = online;
            UserID = userId;
            CVV = cvv;
        }

        public User(string cardNumber, string pin, string name, int amount = 0, bool online = false)
        {
            CardNumber = cardNumber;
            PIN = pin;
            Name = name;
            Amount = amount;
            IsLoggedIn = online;
            CVV = IDGenerator.GenerateCVV();
            UserID = IDGenerator.GenerateID();
            CardService.GenerateCard( this );
        }
    }

    
}