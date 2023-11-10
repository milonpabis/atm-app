using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMLogic
{
    public class ATM
    {
        private List<User> Users { get; set; } = new List<User>();
        private DataService dataService = new DataService();
        public ATM()
        {
            SynchronizeData();
        }

        public object? AddUser(User user)
        {
            if (user.PIN.Length != 4)
                return new TypedDataError();
            if (user.CardNumber.Length != 8)
                return new TypedDataError();
            if (Users.Any(u => u.CardNumber.Equals(user.CardNumber)))
                return new CardNumberExistsError();
            if( user != null )
            {
                dataService.AddUser(user);
                SynchronizeData();
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

        public bool LogIn(string number, string pin)
        {
            return Users.Any(u => u.CardNumber.Equals(number) && u.PIN.Equals(pin));
        }

        public User RetrieveUser(string number, string pin)
        {
            return Users.Single(u => u.CardNumber.Equals(number) && u.PIN.Equals(pin));
        }

        public User RetrieveUser(string userID)
        {
            return Users.Single(u => u.UserID.Equals(userID));
        }

        public void SynchronizeData()
        {
            Users = dataService.RetrieveUsers();
        }

    }
}
