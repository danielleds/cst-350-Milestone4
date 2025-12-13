using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Milestone4.Models
{

    public class StateViewModel
    {
        public int Id { get; set; }
        public string StateName { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Sex is required.")]
        public string Sex { get; set; }

        [Required(ErrorMessage = "State is required.")]
        public int StateOfResidence { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }


        public string[] Sexes { get; set; }
        public List<StateViewModel> States { get; set; }
        public SelectList StateSelectList { get; set; }

        public RegisterViewModel()
        {
            Sexes = ["Male", "Female"];

            States = new List<StateViewModel>
            {
                new StateViewModel { Id = 1, StateName = "Alabama" },
                new StateViewModel { Id = 2, StateName = "Alaska" },
                new StateViewModel { Id = 3, StateName = "Arizona" },
                new StateViewModel { Id = 4, StateName = "Arkansas" },
                new StateViewModel { Id = 5, StateName = "California" },
                new StateViewModel { Id = 6, StateName = "California" },
                new StateViewModel { Id = 7, StateName = "Colorado" },
                new StateViewModel { Id = 8, StateName = "Connecticut" },
                new StateViewModel { Id = 9, StateName = "Delaware" },
                new StateViewModel { Id = 10, StateName = "Florida" },
                new StateViewModel { Id = 11, StateName = "Georgia" },
                new StateViewModel { Id = 12, StateName = "Hawaii" },
                new StateViewModel { Id = 13, StateName = "Idaho" },
                new StateViewModel { Id = 14, StateName = "Illinois" },
                new StateViewModel { Id = 15, StateName = "Indiana" },
                new StateViewModel { Id = 16, StateName = "Iowa" },
                new StateViewModel { Id = 17, StateName = "Kansas" },
                new StateViewModel { Id = 18, StateName = "Kentucky" },
                new StateViewModel { Id = 19, StateName = "Louisiana" },
                new StateViewModel { Id = 20, StateName = "Maine" },
                new StateViewModel { Id = 21, StateName = "Maryland" },
                new StateViewModel { Id = 22, StateName = "Massachusetts" },
                new StateViewModel { Id = 23, StateName = "Michigan" },
                new StateViewModel { Id = 24, StateName = "Minnesota" },
                new StateViewModel { Id = 25, StateName = "Mississippi" },
                new StateViewModel { Id = 26, StateName = "Missouri" },
                new StateViewModel { Id = 27, StateName = "Montana" },
                new StateViewModel { Id = 28, StateName = "Nebraska" },
                new StateViewModel { Id = 29, StateName = "Nevada" },
                new StateViewModel { Id = 30, StateName = "New Hampshire" },
                new StateViewModel { Id = 31, StateName = "New Jersey" },
                new StateViewModel { Id = 32, StateName = "New Mexico" },
                new StateViewModel { Id = 33, StateName = "New York" },
                new StateViewModel { Id = 34, StateName = "North Carolina" },
                new StateViewModel { Id = 35, StateName = "North Dakota" },
                new StateViewModel { Id = 36, StateName = "Ohio" },
                new StateViewModel { Id = 37, StateName = "Oklahoma" },
                new StateViewModel { Id = 38, StateName = "Oregon" },
                new StateViewModel { Id = 38, StateName = "Pennsylvania" },
                new StateViewModel { Id = 40, StateName = "Rhode Island" },
                new StateViewModel { Id = 41, StateName = "South Carolina" },
                new StateViewModel { Id = 42, StateName = "South Dakota" },
                new StateViewModel { Id = 43, StateName = "Tennessee" },
                new StateViewModel { Id = 44, StateName = "Texas" },
                new StateViewModel { Id = 45, StateName = "Utah" },
                new StateViewModel { Id = 46, StateName = "Vermont" },
                new StateViewModel { Id = 47, StateName = "Virginia" },
                new StateViewModel { Id = 48, StateName = "Washington" },
                new StateViewModel { Id = 49, StateName = "West Virginia" },
                new StateViewModel { Id = 50, StateName = "Wisconsin" },
                new StateViewModel { Id = 51, StateName = "Wyoming" },
            };

            StateSelectList = new SelectList(States, "Id", "StateName");
        }
    }
}
