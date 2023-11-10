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
        [InlineData("11112352", "6969", "Milon")]
        [InlineData("90403333", "1111", "Milosz")]
        [InlineData("21373333", "1234", "Milek")]
        public void Adds_user(string cardNum, string PIN, string userName)
        { 
            var user = new User(cardNumber: cardNum, pin: PIN, name: userName);
            atm.AddUser(user);
            atm.GetUsers().Should().ContainEquivalentOf(new User(cardNum, PIN, userName, user.UserID, user.CVV));
            atm.DeleteUser(user);
        }

        [Fact]
        public void Decline_invalid_card_number()
        {
            var user = new User("1111", "1111", "milon");
            var error = atm.AddUser(user);
            error.Should().BeOfType<TypedDataError>();
        }

        [Fact]
        public void Decline_invalid_pin()
        {
            var user = new User("12112252", "12345", "milon");
            var error = atm.AddUser(user);
            error.Should().BeOfType<TypedDataError>();
        }


        [Theory]
        [InlineData("22223333", "1010", "6969", "kitka", "milon")]
        [InlineData("69694444", "2137", "2137", "kiter", "milek")]
        [InlineData("11154444", "1115", "1115", "kitkot", "milosz")]
        public void Does_not_add_user_with_exisiting_number(string cardNum, string pin1, string pin2, string user1, string user2)
        {
            var t_user = new User(cardNum, pin1, user1);
            var t_user1 = new User(cardNum, pin2, user2);
            atm.AddUser(t_user);
            var error = atm.AddUser(t_user1);
            error.Should().BeOfType<CardNumberExistsError>();
        }



        [Fact]
        public void Deletes_user()
        {
            var user = new User(cardNumber: "21371111", pin: "1111", name: "Milosz");
            atm.AddUser(user);
            atm.DeleteUser("21371111", "1111");
            atm.GetUsers().Should().BeEmpty();
        }


        
        [Fact]

        public void Logs_exisiting_user()
        {
            var user = new User("69691211", "1010", "Fifi");
            atm.AddUser(user);
            var result = atm.LogIn("69691211", "1010");
            result.Should().Be(true);
            atm.DeleteUser(user);
        }



        [Fact]
        public void Does_not_log_non_existing_user()
        {
            var user = new User("69691131", "1010", "Fifi");
            atm.AddUser(user);
            var result = atm.LogIn("10101131", "6969");
            result.Should().Be(false);
            atm.DeleteUser(user);
        }


        [Fact]
        public void Does_not_delete_non_existing_user()
        {
            var user = new User("69914311", "2137", "kitokot");
            atm.AddUser(user);
            var error = atm.DeleteUser("11111111", "1111");
            error.Should().BeOfType<UserDoesNotExist>();
            atm.DeleteUser(user);
        }



        [Fact]

        public void Returns_user_with_number()
        {
            var user = new User("69701111", "2137", "iwonap", amount: 1000);
            atm.AddUser(user);
            var retrievedUser = atm.RetrieveUser("69701111", "2137");
            retrievedUser.Name.Should().Be("iwonap");
            atm.DeleteUser(user);
        }

        [Fact]
        public void Returns_user_with_id()
        {
            var user = new User("77778888", "1111", "milon");
            atm.AddUser(user);
            var id = user.UserID;
            var retrievedUser = atm.RetrieveUser(id);
            retrievedUser.Name.Should().Be("milon");
            atm.DeleteUser(user);
        }
    }
}