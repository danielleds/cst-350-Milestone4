using Microsoft.AspNetCore.Mvc;
using Milestone4.Models;
using Milestone4.Services;

namespace Milestone4.Controllers
{
    [ApiController]
    [Route("api")]
    public class MinesweeperRestController : Controller
    {
        private readonly IGameBoardService _gameBoardService;

        public MinesweeperRestController(IGameBoardService gameBoardService)
        {
            _gameBoardService = gameBoardService;
        }

        [HttpGet]
        [Route("ShowSavedGames")]
        public async Task<ActionResult<IEnumerable<GameBoardModel>>> ShowSavedGames()
        {
            IEnumerable<GameBoardModel> games = await _gameBoardService.GetAllBoards();
            return Ok(games);
        }

        [HttpGet("{id}")]
        [Route("ShowSavedGames/{id}")]
        public async Task<ActionResult<IEnumerable<GameBoardModel>>> ShowSavedGames(int id)
        {
            GameBoardModel game = await _gameBoardService.GetBoard(id);
            if (game == null)
            {
                return NotFound();
            }
            return Ok(game);
        }

        [HttpDelete("{id}")]
        [Route("DeleteOneGame/{id}")]
        public async Task<ActionResult> DeleteOneGame(int id)
        {
            // check that the game exists first
            var product = await _gameBoardService.GetBoard(id);
            if (product != null)
            {
                await _gameBoardService.DeleteBoard(id);
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
