using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace ATMLogic
{


    public class DataService
    {
        SqlConnection connection;
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=F:\\Desktop\\repos\\C#\\ATM\\Domain\\data\\db.mdf;Integrated Security=True";
        
        public DataService()
        {
            connection = new SqlConnection(connectionString);
        }

        public void AddUser(User user)
        {
            try
            {
                string query = "INSERT INTO Users VALUES ( @UserID, @Name, @PIN, @Amount, @CardNumber );";
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                connection.Open();
                sqlCommand.Parameters.AddWithValue("@UserID", user.GetUserID());
                sqlCommand.Parameters.AddWithValue("@Name", user.GetName());
                sqlCommand.Parameters.AddWithValue("@PIN", user.GetPin());
                sqlCommand.Parameters.AddWithValue("@Amount", user.GetAmount());
                sqlCommand.Parameters.AddWithValue("@CardNumber", user.GetCardNumber());
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        public void DeleteUser(User user)
        {
            try
            {
                string query = "DELETE FROM Users WHERE UserID = @UserID;";
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void UpdateInfo(User user)
        {
            try
            {
                string query = "UPDATE Users SET Amount=@Amount WHERE UserID = @UserID;";
                SqlCommand sqlCommand = new SqlCommand( query, connection );
                connection.Open();
                sqlCommand.Parameters.AddWithValue("@Amount", user.GetAmount());
                sqlCommand.Parameters.AddWithValue("@UserID", user.GetUserID());
                sqlCommand.ExecuteScalar();
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        public List<User> RetrieveUsers()
        {
                string query = "SELECT * FROM Users;";
                SqlCommand sqlCommand = new SqlCommand( query, connection );
                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);

                using (adapter)
                {
                    DataTable usersTable = new DataTable();
                    adapter.Fill(usersTable);
                    return ConvertDataTable(usersTable);
                }   
        }

        public List<User> ConvertDataTable(DataTable dt)
        {
            List<User> users = new List<User>();

            foreach(DataRow dr in dt.Rows)
            {
                string? cardNumber = dr["CardNumber"].ToString();
                string? Pin = dr["PIN"].ToString();
                string? Name = dr["Name"].ToString();
                int Amount = (int)dr["Amount"];
                string? UserID = dr["UserID"].ToString();
                if( cardNumber != null && Pin != null && Name != null && UserID != null)
                {
                    User user = new User(cardNumber, Pin, Name, UserID, Amount);
                    users.Add(user);
                }
            }
            return users;
        }

    }
}
