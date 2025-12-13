using Milestone4.Models;
using MySql.Data.MySqlClient;

namespace Milestone4.Services
{
    public class UserDAO
    {
        string connectionString =
            "datasource=localhost;port=3306;username=root;password=root;database=minesweeper";

        public UserModel getUserByUserName(string username)
        {
            UserModel userToReturn = new UserModel() { Id = 0 };

            // connect to the mysql server
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            MySqlCommand command =
                new MySqlCommand("SELECT ID, FIRST_NAME, LAST_NAME, SEX, STATE, EMAIL, USERNAME, PASSWORD FROM users WHERE USERNAME = @username", connection);
            command.Parameters.AddWithValue("@username", username);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    userToReturn = new UserModel
                    {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Sex = reader.GetString(3),
                        StateOfResidence = reader.GetString(4),
                        Email = reader.GetString(5),
                        Username = reader.GetString(6),
                        Password = reader.GetString(7),
                    };
                }
            }

            return userToReturn;
        }

        internal int addUser(UserModel user)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            MySqlCommand command = new MySqlCommand("INSERT INTO `users`" +
                "(`FIRST_NAME`, `LAST_NAME`, `SEX`, `STATE`, `EMAIL`, `USERNAME`, `PASSWORD`) " +
                "VALUES (@firstname, @lastname, @sex, @state, @email, @username, @password)",
                connection);

            command.Parameters.AddWithValue("@firstname", user.FirstName);
            command.Parameters.AddWithValue("@lastname", user.LastName);
            command.Parameters.AddWithValue("@sex", user.Sex);
            command.Parameters.AddWithValue("@state", user.StateOfResidence);
            command.Parameters.AddWithValue("@email", user.Email);
            command.Parameters.AddWithValue("@username", user.Username);
            command.Parameters.AddWithValue("@password", user.Password);

            int newRows = command.ExecuteNonQuery();
            connection.Close();

            return newRows;
        }
    }
}
