namespace ProgedAPI.Entities
{
    public class Poste
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public byte[] Avatar { get; set; }
    }
}
