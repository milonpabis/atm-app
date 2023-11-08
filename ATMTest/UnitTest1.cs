using FluentAssertions;
using System.Configuration;
using Domain;
using System.ComponentModel.DataAnnotations;

namespace ATMTest
{
    public class UnitTest1
    {

        // only 4 digit pin
        // only 8 digit cardNumber


        public ATM atm = new ATM();
        [Theory]
        [InlineData(1111, 6969, "Milon")]
        [InlineData(9040, 1111, "Milosz")]
        [InlineData(2137, 1234, "Milek")]
        public void Adds_user(int cardNum, int PIN, string userName)
        { 
            var user = new User(cardNumber: cardNum, pin: PIN, name: userName);
            atm.AddUser(user);
            atm.GetUsers().Should().ContainEquivalentOf(new User(cardNum, PIN, userName));
        }


        [Theory]
        [InlineData(2222, 1010, 6969, "kitka", "milon")]
        [InlineData(6969, 2137, 2137, "kiter", "milek")]
        [InlineData(1115, 1115, 1115, "kitkot", "milosz")]
        public void Does_not_add_user_with_exisiting_number(int cardNum, int pin1, int pin2, string user1, string user2)
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
            var user = new User(cardNumber: 2137, pin: 1111, name: "Milosz");
            atm.AddUser(user);
            atm.DeleteUser(2137, 1111);
            atm.GetUsers().Should().BeEmpty();
        }


        
        [Fact]

        public void Logs_exisiting_user()
        {
            var user = new User(6969, 1010, "Fifi");
            atm.AddUser(user);
            var result = atm.LogIn(6969, 1010);
            result.Should().Be(true);
        }



        [Fact]
        public void Does_not_log_non_existing_user()
        {
            var user = new User(6969, 1010, "Fifi");
            atm.AddUser(user);
            var result = atm.LogIn(1010, 6969);
            result.Should().Be(false);
        }


        [Fact]
        public void Does_not_delete_non_existing_user()
        {
            var user = new User(6969, 2137, "kitokot");
            atm.AddUser(user);
            var error = atm.DeleteUser(1111, 1111);
            error.Should().BeOfType<UserDoesNotExist>();
        }



        [Fact]

        public void Returns_user_info()
        {
            var user = new User(6969, 2137, "iwonap", 1000);
            atm.AddUser(user);
            var retrievedUser = atm.RetrieveUser(6969, 2137);
            retrievedUser.GetName().Should().Be("iwonap");
        }


    }
}