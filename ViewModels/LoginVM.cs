using System.ComponentModel.DataAnnotations;

namespace Dewi.ViewModels
{
    public record LoginVM
    {
        [Required]
        [MinLength(3)]
        [MaxLength(200)]
        public string EmailorUsername { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Password daxil edin")]
        public string Password { get; set; }

        public bool RememberME { get; set; }    


    }
}
