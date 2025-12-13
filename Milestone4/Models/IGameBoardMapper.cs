using Milestone4.Models;

namespace Milestone4.Models
{
    public interface IGameBoardMapper
    {
        public GameBoardModel ToGameBoard(GameBoardViewModel gbvm);
        public GameBoardViewModel ToViewModel(GameBoardModel gbm);
    }
}
