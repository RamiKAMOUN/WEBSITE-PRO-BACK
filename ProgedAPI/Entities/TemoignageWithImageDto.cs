namespace ProgedAPI.Entities
{
    public class TemoignageWithImageDto
    {
        public string Name { get; set; }
        public string Job { get; set; }
        public string Quote { get; set; }
        public IFormFile AvatarFile { get; set; }
        
    }
}
