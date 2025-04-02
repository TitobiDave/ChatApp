namespace ChatApp.Models
{
    public class Message
    {
        public int Id { get; set; }

        public string message {  get; set; }

        public string senderId { get; set; }

        public string receiverId { get; set; }
    }
}
