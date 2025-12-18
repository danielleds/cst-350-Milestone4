using Milestone4.Models;

namespace Milestone4.Services
{
    public interface IGameBoardDAO
    {
        Task<GameBoardModel> GetBoard(int boardId);
        Task<IEnumerable<GameBoardModel>> GetByUserId(int userId);
        Task<IEnumerable<GameBoardModel>> GetAllBoards();
        Task<int> SaveBoard(GameBoardModel board);
        Task UpdateBoard(GameBoardModel board);
        Task DeleteBoard(int boardId);
    }
}
