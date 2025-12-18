using Milestone4.Models;

namespace Milestone4.Services
{
    public class GameBoardService : IGameBoardService
    {
        private readonly IGameBoardDAO _gameBoardDAO;
        private readonly IGameBoardMapper _gameBoardMapper;

        public GameBoardService(IGameBoardDAO gameBoardDAO, IGameBoardMapper gameBoardMapper)
        {
            _gameBoardDAO = gameBoardDAO;
            _gameBoardMapper = gameBoardMapper;
        }

        public async Task<GameBoardViewModel> SaveBoard(GameBoardViewModel board)
        {
            int id = await _gameBoardDAO.SaveBoard(_gameBoardMapper.ToGameBoard(board));
            board.Id = id;
            await _gameBoardDAO.UpdateBoard(_gameBoardMapper.ToGameBoard(board));
            return board;
        }

        public async Task UpdateBoard(GameBoardViewModel board)
        {
            await _gameBoardDAO.UpdateBoard(_gameBoardMapper.ToGameBoard(board));
        }

        public async Task<GameBoardViewModel> GetBoardViewModel(int boardId)
        {
            GameBoardModel gameBoard = await _gameBoardDAO.GetBoard(boardId);

            if (gameBoard == null)
            {
                return _gameBoardMapper.ToViewModel(gameBoard);
            }
            return null;
        }

        public async Task<GameBoardModel> GetBoard(int boardId)
        {
            GameBoardModel gameBoard = await _gameBoardDAO.GetBoard(boardId);
            return gameBoard;
        }

        public async Task<IEnumerable<GameBoardModel>> GetAllBoards()
        {
            return await _gameBoardDAO.GetAllBoards();
        }

        public async Task<IEnumerable<GameBoardModel>> GetByUserId(int userId)
        {
            IEnumerable<GameBoardModel> gameBoardModels = await _gameBoardDAO.GetByUserId(userId);
            return gameBoardModels;
        }

        public async Task DeleteBoard(int boardId)
        {
            await _gameBoardDAO.DeleteBoard(boardId);
        }
    }
}
