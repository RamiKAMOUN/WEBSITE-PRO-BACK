namespace ProgedAPI.Entities
{
    public class Cervice
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public byte[] Avatar { get; set; }
    }
}
