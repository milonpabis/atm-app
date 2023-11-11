using FluentAssertions;
using System.Configuration;
using ATMLogic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ATMTest
{
    public class UnitTest1
    {

        public ATM atm = new ATM();
        [Theory]
        [InlineData("6969", "Milon")]
        [InlineData("1111", "Milosz")]
        [InlineData("1234", "Milek")]
        public void Adds_user(string PIN, string userName)
        { 
            var user = new User(pin: PIN, name: userName);
            atm.AddUser(user);
            atm.GetUsers().Should().ContainEquivalentOf(new User(user.CardNumber, PIN, userName, user.UserID, user.CVV));
            atm.DeleteUser(user);
        }

        [Fact]
        public void Decline_invalid_pin()
        {
            var user = new User("12345", "milon");
            var error = atm.AddUser(user);
            error.Should().BeOfType<TypedDataError>();
        }


        [Theory]
        [InlineData("1010", "6969", "kitka", "milon")]
        [InlineData("2137", "2137", "kiter", "milek")]
        [InlineData("1115", "1115", "kitkot", "milosz")]
        public void Does_not_add_user_with_exisiting_number(string pin1, string pin2, string user1, string user2)
        {
            var t_user = new User(pin1, user1);
            var t_user1 = new User(t_user.CardNumber, pin2, user2, "12345612345612345612345612345612", "555");
            atm.AddUser(t_user);
            var error = atm.AddUser(t_user1);
            error.Should().BeOfType<CardNumberExistsError>();
        }



        [Fact]
        public void Deletes_user()
        {
            var user = new User(pin: "1111", name: "Milosz");
            atm.AddUser(user);
            atm.DeleteUser("21371111", "1111");
            atm.GetUsers().Should().BeEmpty();
        }


        
        [Fact]

        public void Logs_exisiting_user()
        {
            var user = new User("1010", "Fifi");
            atm.AddUser(user);
            var result = atm.CanLogIn(user.CardNumber, "1010");
            result.Should().Be(true);
            atm.DeleteUser(user);
        }



        [Fact]
        public void Does_not_log_non_existing_user()
        {
            var user = new User("1010", "Fifi");
            atm.AddUser(user);
            var result = atm.CanLogIn("10101131", "6969");
            result.Should().Be(false);
            atm.DeleteUser(user);
        }


        [Fact]
        public void Does_not_delete_non_existing_user()
        {
            var user = new User("2137", "kitokot");
            atm.AddUser(user);
            var error = atm.DeleteUser("11111111", "1111");
            error.Should().BeOfType<UserDoesNotExist>();
            atm.DeleteUser(user);
        }

        [Fact]

        public void Returns_user_with_number()
        {
            var user = new User("2137", "iwonap", amount: 1000);
            atm.AddUser(user);
            var retrievedUser = atm.RetrieveUser(user.CardNumber, "2137");
            retrievedUser.Name.Should().Be("iwonap");
            atm.DeleteUser(user);
        }

        [Fact]
        public void Returns_user_with_id()
        {
            var user = new User("1111", "milon");
            atm.AddUser(user);
            var id = user.UserID;
            var retrievedUser = atm.RetrieveUser(id);
            retrievedUser.Name.Should().Be("milon");
            atm.DeleteUser(user);
        }
    }
}