using FinancialChatApp_V2.Controllers;
using FinancialChatApp_V2.Data;
using FinancialChatApp_V2.Models;
using FinancialChatApp_V2.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FinancialChatApp_V2.UnitTests
{
    public class ChatControllerTests
    {
        [Fact]
        public async Task SendMessage_ShouldSaveMessageToDatabase_AndCallRabbitMQ()
        {
            // Arrange
            var mockDbContext = new Mock<ApplicationDBContext>();
            var mockRabbitMQService = new Mock<RabbitMQService>();
            var controller = new ChatController(mockDbContext.Object, mockRabbitMQService.Object);

            var message = "Hello, World!";

            // Mock the behavior of Add and SaveChangesAsync for DbContext
            mockDbContext.Setup(db => db.SaveChangesAsync(It.IsAny<System.Threading.CancellationToken>())).Returns((Task<int>)Task.CompletedTask);
            mockDbContext.Setup(db => db.Messages.Add(It.IsAny<ChatMessage>()));

            // Act
            var result = await controller.SendMessage(message);

            // Assert
            mockDbContext.Verify(db => db.Messages.Add(It.IsAny<ChatMessage>()), Times.Once); // Ensure Add was called once
            mockDbContext.Verify(db => db.SaveChangesAsync(It.IsAny<System.Threading.CancellationToken>()), Times.Once); // Ensure SaveChangesAsync was called once
            mockRabbitMQService.Verify(rmq => rmq.SendMessageAsync(It.IsAny<string>()), Times.Once); // Ensure RabbitMQ service method was called once

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            // Assert that it redirects to "Index"
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
