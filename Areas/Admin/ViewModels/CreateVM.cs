using System.ComponentModel.DataAnnotations;

namespace Dewi.Areas.Admin.ViewModels
{
    public record CreateVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Position { get; set; }
        public IFormFile? File { get; set; }
    }
}
