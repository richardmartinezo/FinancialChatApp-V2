using FinancialChatApp_V2.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialChatApp_V2.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        public DbSet<ChatMessage> Messages { get; set; }
        public DbSet<ChatRoom> Room { get; set; }
    }
}
