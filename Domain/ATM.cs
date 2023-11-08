using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ATM
    {
        private List<User> Users { get; set; } = new List<User>();
        public ATM()
        {

        }

        public object? AddUser(User user)
        {
            if (Users.Any(u => u.GetCardNumber().Equals(user.GetCardNumber())))
                return new CardNumberExistsError();
            Users.Add(user);
            return null;
        }

        public object? DeleteUser(int number, int pin)
        {
            if ( !Users.Any(u => u.GetCardNumber().Equals(number) && u.GetPin().Equals(pin) ) ) return new UserDoesNotExist();
            Users.Remove(Users.Single(u => u.GetCardNumber().Equals(number) && u.GetPin().Equals(pin)));
            return null;
        }

        public List<User> GetUsers()
        {
            return Users;
        }

        public bool LogIn(int number, int pin)
        {
            return Users.Any(u => u.GetCardNumber().Equals(number) && u.GetPin().Equals(pin));
        }

        public User RetrieveUser(int number, int pin)
        {
            return Users.Single(u => u.GetCardNumber().Equals(number) && u.GetPin().Equals(pin));
        }

    }
}
