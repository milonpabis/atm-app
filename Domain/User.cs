using System.Reflection.Metadata;

namespace Domain
{
    public class User
    {
        private int CardNumber { get; set; }
        //public Guid UserID { get; set; }
        private int PIN { get; set; }

        private string Name { get; set; }

        private int Amount { get; set; }

        public bool IsLoggedIn { get; set; }

        public User(int cardNumber, int pin, string name, int amount=0)
        {
            CardNumber = cardNumber;
            PIN = pin;
            Name = name;
            Amount = amount;
        }

        public int GetCardNumber()
        {
            return CardNumber;
        }

        public int GetPin()
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
        
    }

    
}