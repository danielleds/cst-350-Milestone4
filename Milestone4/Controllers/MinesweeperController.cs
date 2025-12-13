using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Milestone4.Filters;
using Milestone4.Models;
using Milestone4.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Drawing;

namespace Milestone4.Controllers
{
    public class MinesweeperController : Controller
    {
        private readonly IGameBoardService _gameBoardService;
        private readonly IGameBoardMapper _gameBoardMapper;

        public MinesweeperController(IGameBoardService gameBoardService, IGameBoardMapper gameBoardMapper)
        {
            _gameBoardService = gameBoardService;
            _gameBoardMapper = gameBoardMapper;
        }

        // saves a game in the session variables
        private void SaveGame(GameBoardViewModel gameBoard, bool initialSave, bool saveToDatabase)
        {
            if (saveToDatabase)
            {
                if (initialSave)
                {
                    string userJson = HttpContext.Session.GetString("User");
                    int userId = Int32.Parse(JObject.Parse(userJson)["Id"].ToString());
                    gameBoard.UserId = userId;
                    gameBoard = _gameBoardService.SaveBoard(gameBoard).Result;
                }
                else
                {
                    _gameBoardService.UpdateBoard(gameBoard);
                }
            }
            string gameJson = ServiceStack.Text.JsonSerializer.SerializeToString(gameBoard); 
            HttpContext.Session.SetString("Game", gameJson);
        }

        // retrieves the gameboard from session variables
        private GameBoardViewModel RetrieveGame()
        {
            // This is called only within Action Results filtered with GameCheckFilter,
            // so the game will always exist when this is called.

            //Deserialize the data.
            try
            {
                string gameString = HttpContext.Session.GetString("Game");
                GameBoardViewModel gameBoard = ServiceStack.Text.JsonSerializer.DeserializeFromString<GameBoardViewModel>(gameString);

                // Get the grid separately since ServiceStack can't deserialize multidimensional arrays
                string gridString = JObject.Parse(gameString)["Grid"].ToString();
                gameBoard.Grid = JsonConvert.DeserializeObject<GameCellModel[,]>(gridString);

                return gameBoard;
            }
            catch
            {
                return null;
            }
        }

        public IActionResult SaveGameFromBoard(int internalStopwatchTime)
        {
            GameBoardViewModel board = RetrieveGame();
            board.Time = internalStopwatchTime;
            SaveGame(board, false, true);
            return NoContent();
        }

        public IActionResult LoadSavedGame(int id)
        {
            GameBoardModel gameBoardModel = _gameBoardService.GetBoard(id).Result;
            GameBoardViewModel board = _gameBoardMapper.ToViewModel(gameBoardModel);
            SaveGame(board, false, false);
            return RedirectToAction("MineSweeperBoard");
        }

        // user must be logged in to access; will be redirected to login page otherwise
        [SessionCheckFilter]
        public IActionResult Index()
        {
            return View();
        }

        [SessionCheckFilter]
        public IActionResult StartGame()
        {
            return View("Index");
        }

        [SessionCheckFilter]
        public IActionResult Games()
        {
            string userJson = HttpContext.Session.GetString("User");
            int userId = Int32.Parse(JObject.Parse(userJson)["Id"].ToString());

            List<GameBoardModel> games = _gameBoardService.GetByUserId(userId).Result.ToList();

            return View(games);
        }

        [SessionCheckFilter]
        public IActionResult NewGame(GameSettingsModel gameSettings)
        {
            if (gameSettings == null)
            {
                // Settings weren't set up, return to the settings page
                return RedirectToAction("Index");
            }

            // Assume easy, then change if another difficulty was selected
            decimal difficultySelection = 0.10M;

            if (gameSettings.DifficultySelection == "Medium")
            {
                difficultySelection = 0.16M;
            }
            else if (gameSettings.DifficultySelection == "Hard")
            {
                difficultySelection = 0.22M;
            }

            GameBoardViewModel gameBoard = new GameBoardViewModel(gameSettings.Size, difficultySelection);

            // save data in session variables and database
            SaveGame(gameBoard, true, true);

            return RedirectToAction("MineSweeperBoard");
        }

        [SessionCheckFilter]
        [GameCheckFilter]
        public IActionResult NewGameSameSettings()
        {
            GameBoardViewModel oldGameBoard = RetrieveGame();
            if (oldGameBoard == null)
            {
                // There was an error in getting the gameboard; return to the new game screen.
                return RedirectToAction("StartGame");
            }

            GameBoardViewModel gameBoard = new GameBoardViewModel(oldGameBoard.Size, oldGameBoard.Difficulty);

            // save data in session variables and database
            SaveGame(gameBoard, true, true);

            return RedirectToAction("MineSweeperBoard", gameBoard);
        }

        [SessionCheckFilter]
        [GameCheckFilter]
        public IActionResult MineSweeperBoard()
        {
            GameBoardViewModel board = RetrieveGame();
            if (board == null)
            {
                // There was an error in getting the gameboard; return to the new game screen.
                return RedirectToAction("StartGame");
            }

            return View(board);
        }

        [SessionCheckFilter]
        [GameCheckFilter]
        public IActionResult CellLeftClick(string id, int internalStopwatchTime)
        {
            GameBoardViewModel gameBoard = RetrieveGame();
            if (gameBoard == null)
            {
                // There was an error in getting the gameboard; return to the new game screen.
                return RedirectToAction("StartGame");
            }

            // split id string into coordinates
            int[] parsedId = id.Split(',').Select(coord => Int32.Parse(coord)).ToArray();
            GameCellModel currentCell = gameBoard.Grid[parsedId[0], parsedId[1]];

            // proceed only if the cell is not flagged
            if (!currentCell.Flagged)
            {
                gameBoard = CellClick(gameBoard, currentCell);

                // assume an unfinished state; this message will change if ending conditions are met.
                ViewData["Message"] = "";

                if (gameBoard.GameFinished)
                {
                    gameBoard.Time = internalStopwatchTime;
                    Console.WriteLine("Stopwatch: " + internalStopwatchTime);
                    Console.WriteLine("Gametime: " + gameBoard.Time);
                    if (gameBoard.GameWon)
                    {
                        ViewData["Message"] = "You win!";
                    }
                    else
                    {
                        ViewData["Message"] = "Game over";
                    }
                    SaveGame(gameBoard, false, true);
                }
            }

            return PartialView("_GameBoard", gameBoard);
        }

        [SessionCheckFilter]
        [GameCheckFilter]
        public IActionResult CellRightClick(string id)
        {
            GameBoardViewModel gameBoard = RetrieveGame();
            if (gameBoard == null)
            {
                // There was an error in getting the gameboard; return to the new game screen.
                return RedirectToAction("StartGame");
            }

            // split id string into coordinates
            int[] parsedId = id.Split(',').Select(coord => Int32.Parse(coord)).ToArray();
            GameCellModel currentCell = gameBoard.Grid[parsedId[0], parsedId[1]];

            if (currentCell.Flagged)
            {
                currentCell.Flagged = false;
                currentCell.Image = "/img/unexplored.png";
            }
            else
            {
                currentCell.Flagged = true;
                currentCell.Image = "/img/flag.png";
            }

            SaveGame(gameBoard, false, false);
            return PartialView("_GameCell", currentCell);
        }


        private GameBoardViewModel CellClick(GameBoardViewModel gameBoard, GameCellModel currentCell)
        {
            if (gameBoard.FirstMove)
            {
                // Ensure that the first move is safe
                gameBoard.FirstMove = false;
                gameBoard.EnsureSafe(currentCell.Row, currentCell.Col);
            }

            gameBoard.FloodFill(currentCell);

            // Game is finished if a live cell was clicked OR all safe cells were revealed
            if (currentCell.Live)
            {
                gameBoard.EndGame(false);
            }
            else if (gameBoard.SafeCellsRevealed())
            {
                gameBoard.EndGame(true);
            }

            SaveGame(gameBoard, false, false);

            return gameBoard;
        }
    }
}
