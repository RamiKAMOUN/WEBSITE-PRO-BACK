namespace ProgedAPI.Entities
{
    public class PosteDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile AvatarFile { get; set; }
    }
}
