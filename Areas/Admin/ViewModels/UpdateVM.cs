using System.ComponentModel.DataAnnotations;

namespace Dewi.Areas.Admin.ViewModels
{
    public class UpdateVM
    {
        [Required]
        public string Name { get; set; }
       
        [Required]
        public string Position { get; set; }
        public string? CurentImgUrl { get; set; }
        public IFormFile? File { get; set; }
    }
}
