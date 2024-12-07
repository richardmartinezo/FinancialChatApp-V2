namespace FinancialChatApp_V2.Models
{
    public class ChatRoom
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ChatMessage> Messages { get; set; }
    }
}
