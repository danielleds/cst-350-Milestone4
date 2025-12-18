using Milestone4.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Milestone4.Models
{
    public class GameBoardMapper : IGameBoardMapper
    {
        public GameBoardModel ToGameBoard(GameBoardViewModel gbvm)
        {
            string gameJson = ServiceStack.Text.JsonSerializer.SerializeToString(gbvm);

            if (gbvm.Id < 0)
            { // id is invalid and will be created by the database
                return new GameBoardModel
                {
                    UserId = gbvm.UserId,
                    DateSaved = DateTime.Now,
                    GameData = gameJson
                };
            }
            else
            {
                return new GameBoardModel
                {
                    Id = gbvm.Id,
                    UserId = gbvm.UserId,
                    DateSaved = DateTime.Now,
                    GameData = gameJson
                };
            }
        }

        public GameBoardViewModel ToViewModel(GameBoardModel gbm)
        {
            // Deserialize the data.
            try
            {
                GameBoardViewModel gameBoard = ServiceStack.Text.JsonSerializer.DeserializeFromString<GameBoardViewModel>(gbm.GameData);

                // Get the grid separately since ServiceStack can't deserialize multidimensional arrays
                string gridString = JObject.Parse(gbm.GameData)["Grid"].ToString();
                gameBoard.Grid = JsonConvert.DeserializeObject<GameCellModel[,]>(gridString);

                return gameBoard;
            }
            catch
            {
                return null;
            }
        }
    }
}
