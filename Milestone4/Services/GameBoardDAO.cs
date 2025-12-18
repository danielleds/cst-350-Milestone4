using Milestone4.Models;
using MySql.Data.MySqlClient;

namespace Milestone4.Services
{
    public class GameBoardDAO : IGameBoardDAO
    {
        string connectionString =
            "datasource=localhost;port=3306;username=root;password=root;database=minesweeper";

        public async Task<int> SaveBoard(GameBoardModel board)
        {
            long newId;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand command =
                    new MySqlCommand("INSERT INTO games (UserId, DateSaved, GameData) VALUES (@UserId, @DateSaved, @GameData)", conn);
                command.Parameters.AddWithValue("@UserId", board.UserId);
                command.Parameters.AddWithValue("@DateSaved", board.DateSaved);
                command.Parameters.AddWithValue("@GameData", board.GameData);

                conn.Open();
                await command.ExecuteNonQueryAsync();
                newId = command.LastInsertedId;
                conn.Close();
            }

            return Convert.ToInt32(newId);
        }

        public async Task UpdateBoard(GameBoardModel board)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand command =
                    new MySqlCommand("UPDATE games SET DateSaved = @DateSaved, GameData = @GameData WHERE Id = @Id", conn);
                command.Parameters.AddWithValue("@DateSaved", board.DateSaved);
                command.Parameters.AddWithValue("@GameData", board.GameData);
                command.Parameters.AddWithValue("@Id", board.Id);

                conn.Open();
                await command.ExecuteNonQueryAsync();
                conn.Close();
            }
        }

        public async Task<GameBoardModel> GetBoard(int boardId)
        {
            GameBoardModel board = null;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand command =
                    new MySqlCommand("SELECT * FROM games WHERE Id = @Id", conn);
                command.Parameters.AddWithValue("@Id", boardId);

                conn.Open();
                MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();

                if (reader.Read())
                {
                    board = new GameBoardModel
                    {
                        Id = reader.GetInt32(0),
                        UserId = reader.GetInt32(1),
                        DateSaved = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2),
                        GameData = reader.IsDBNull(3) ? string.Empty : reader.GetString(3)
                    };
                }
                conn.Close();
            }

            return board;
        }

        public async Task<IEnumerable<GameBoardModel>> GetByUserId(int userId)
        {
            List<GameBoardModel> boards = new List<GameBoardModel>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand command =
                    new MySqlCommand("SELECT * FROM games WHERE UserId = @UserId", conn);
                command.Parameters.AddWithValue("@UserId", userId);

                conn.Open();
                MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    boards.Add(new GameBoardModel
                    {
                        Id = reader.GetInt32(0),
                        UserId = reader.GetInt32(1),
                        DateSaved = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2),
                        GameData = reader.IsDBNull(3) ? string.Empty : reader.GetString(3)
                    });
                }
                conn.Close();
            }

            return boards;
        }

        public async Task<IEnumerable<GameBoardModel>> GetAllBoards()
        {
            List<GameBoardModel> boards = new List<GameBoardModel>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand("SELECT * FROM games", conn);
                conn.Open();
                MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    boards.Add(new GameBoardModel
                    {
                        Id = reader.GetInt32(0),
                        UserId = reader.GetInt32(1),
                        DateSaved = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2),
                        GameData = reader.IsDBNull(3) ? string.Empty : reader.GetString(3)
                    });
                }
                conn.Close();
            }

            return boards;
        }

        public async Task DeleteBoard(int boardId)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand command =
                    new MySqlCommand("DELETE FROM games WHERE Id = @Id", conn);
                command.Parameters.AddWithValue("@Id", boardId);

                conn.Open();
                await command.ExecuteNonQueryAsync();
                conn.Close();
            }
        }
    }
}
