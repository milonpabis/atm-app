using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ATMLogic
{
    public class ATM
    {
        private List<User> Users { get; set; } = new List<User>();
        private DataService dataService = new DataService();
        public int Limit { get; set; }
        public bool ToDeposit { get; set; }
        public ATM()
        {
            SynchronizeData();
        }

        public object? AddUser(User user)
        {
            if (user.PIN.Length != 4 || user.PIN.Contains(" "))
                return new TypedDataError();
            if (Users.Any(u => u.CardNumber.Equals(user.CardNumber)))
                return new CardNumberExistsError();
            if( user != null )
            {
                dataService.AddUser(user);
                SynchronizeData();
                CardService.GenerateCard(user);
            }
            return null;
        }

        public object? DeleteUser(string number, string pin)
        {
            if ( !Users.Any(u => u.CardNumber.Equals(number) && u.PIN.Equals(pin) ) ) return new UserDoesNotExist();
            User user = Users.Single(u => u.CardNumber.Equals(number) && u.PIN.Equals(pin));
            if( user != null )
            {
                dataService.DeleteUser(user);
                SynchronizeData();
            }
            return null;
        }

        public object? DeleteUser(User user)
        {
            return DeleteUser(user.CardNumber, user.PIN);
        }

        public List<User> GetUsers()
        {
            return Users;
        }

        public bool CanLogIn(string number, string cvv)
        {
            return Users.Any(u => u.CardNumber.Equals(number) && u.CVV.Equals(cvv));
        }

        public User RetrieveUser(string number, string cvv)
        {
            return Users.Single(u => u.CardNumber.Equals(number) && u.CVV.Equals(cvv));
        }

        public User RetrieveUser(string userID)
        {
            return Users.Single(u => u.UserID.Equals(userID));
        }

        public void SynchronizeData()
        {
            Users = dataService.RetrieveUsers();
        }

        public object? ChangeAmount(User user, int amount)
        {
            int userAmount = user.Amount;
            if (ToDeposit && amount <= Limit)
            {
                userAmount += amount;
            }
            else if(!ToDeposit && amount <= Limit && userAmount >= amount)
            {
                userAmount -= amount;
            }
            else
            {
                if (amount > Limit) return new LimitExceededError();
                else return new UnsufficientAmountError();
            }
            
            dataService.UpdateInfo(user, userAmount);
            SynchronizeData();
            return null;
        }

    }
}
