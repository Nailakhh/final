namespace Dewi.Models
{
    public class Member:BaseEntity
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string? ImgUrl { get; set; }
    }
}
