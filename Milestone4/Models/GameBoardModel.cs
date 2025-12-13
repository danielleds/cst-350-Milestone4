namespace Milestone4.Models
{
    public class GameBoardModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime DateSaved { get; set; }
        public string GameData { get; set; }
    }
}
