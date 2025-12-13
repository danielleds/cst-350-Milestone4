using System.ComponentModel.DataAnnotations;

namespace Milestone4.Models
{
    public class GameSettingsModel
    {
        [Required]
        public int Size { get; set; }

        [Required]
        public string DifficultySelection { get; set; }
    }
}
