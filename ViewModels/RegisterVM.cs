using System.ComponentModel.DataAnnotations;

namespace Dewi.ViewModels
{
    public record RegisterVM
    {
        [Required(ErrorMessage ="Ad daxil edin")]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Ad daxil edin")]
        [MinLength(3)]
        [MaxLength(50)]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Ad daxil edin")]
        [MinLength(3)]
        [MaxLength(50)]
        public string Username { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }


        [DataType(DataType.Password),Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
