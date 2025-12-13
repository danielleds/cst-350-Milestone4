using Milestone4.Models;

namespace Milestone4.Services
{
    public interface IGameBoardService
    {
        Task<GameBoardViewModel> GetBoardViewModel(int boardId);
        Task<GameBoardModel> GetBoard(int boardId);
        Task<IEnumerable<GameBoardModel>> GetAllBoards();
        Task<IEnumerable<GameBoardModel>> GetByUserId(int userId);
        Task<GameBoardViewModel> SaveBoard(GameBoardViewModel board);
        Task UpdateBoard(GameBoardViewModel board);
        Task DeleteBoard(int boardId);
    }
}
