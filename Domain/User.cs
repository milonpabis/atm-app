using System.Reflection.Metadata;

namespace ATMLogic
{
    public class User
    {
        private string CardNumber { get; set; }
        private string UserID { get; set; }
        private string PIN { get; set; }

        private string Name { get; set; }

        private int Amount { get; set; }

        public bool IsLoggedIn { get; set; }

        public User(string cardNumber, string pin, string name, string userId, int amount=0, bool online=false )
        {
            CardNumber = cardNumber;
            PIN = pin;
            Name = name;
            Amount = amount;
            IsLoggedIn = online;
            UserID = userId;
        }

        public User(string cardNumber, string pin, string name, int amount = 0, bool online = false)
        {
            CardNumber = cardNumber;
            PIN = pin;
            Name = name;
            Amount = amount;
            IsLoggedIn = online;
            UserID = IDGenerator.GenerateID();
            CardService.GenerateCard( this );
        }

        public string GetCardNumber()
        {
            return CardNumber;
        }

        public string GetPin()
        {
            return PIN;
        }

        public string GetName()
        {
            return Name;
        }

        public int GetAmount() 
        {
            return Amount;
        }

        public string GetUserID()
        {
            return UserID;
        }
        
    }

    
}