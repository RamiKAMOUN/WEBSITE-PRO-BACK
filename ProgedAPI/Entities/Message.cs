namespace ProgedAPI.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Sujet { get; set; } = string.Empty;
        public string Messag { get; set; } = string.Empty;
    }
}
