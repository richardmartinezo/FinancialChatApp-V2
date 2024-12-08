using FinancialChatApp_V2.Data;
using FinancialChatApp_V2.Models;
using FinancialChatApp_V2.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinancialChatApp_V2.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly RabbitMQService _rabbitMQService;

        public ChatController(ApplicationDBContext dbContext, RabbitMQService rabbitMQService)
        {
            _dbContext = dbContext;
            _rabbitMQService = rabbitMQService;
        }

        public IActionResult Index()
        {
            // Fetch existing chat messages from the database
            var messages = _dbContext.Messages
                .OrderBy(m => m.Timestamp)
                .ToList();

            return View(messages);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(string message)
        {
            // Save the message to the database
            var chatMessage = new ChatMessage
            {
                Content = message,
                UserName = User.Identity.Name ?? "Anonymous",
                Timestamp = DateTime.Now,
                ChatRoomId = 1
                
            };

            _dbContext.Messages.Add(chatMessage);
            await _dbContext.SaveChangesAsync();

            // Send the message to RabbitMQ
            await _rabbitMQService.SendMessageAsync(message);

            return RedirectToAction("Index");
        }
    }
}
