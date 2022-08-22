using System.ComponentModel.DataAnnotations;

namespace YGOPICS.Models.ViewModels
{
    public class UserViewModel
    {
        [Required]
        [Display(Name="nombre")]
        public string Name { get; set; }

        [Required]
        public string Mail { get; set; }

        [Required]
        [Display(Name = "contraseña")]
        public string Password { get; set; }

    }
}
