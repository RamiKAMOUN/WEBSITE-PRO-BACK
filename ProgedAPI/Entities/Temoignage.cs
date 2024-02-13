namespace ProgedAPI.Entities
{
    public class Temoignage
    {

        public int Id { get; set; }
        public  string Name { get; set; }
        public string Job { get; set; } = string.Empty;
        public string Quote { get; set; } = string.Empty;
        public byte[] Avatar { get; set; }
      
    }
}
