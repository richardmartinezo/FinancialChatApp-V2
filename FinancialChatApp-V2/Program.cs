using FinancialChatApp_V2.Data;
using FinancialChatApp_V2.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connection = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");

builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(connection));

//create singletons for rabbit mq messages
builder.Services.AddSingleton<RabbitMQService>();
builder.Services.AddSingleton<StockBotService>();




var app = builder.Build();

var rabbitMQService = app.Services.GetRequiredService<RabbitMQService>();
await rabbitMQService.InitializeAsync();

var stockBotService = app.Services.GetRequiredService<StockBotService>();
await stockBotService.StartListening();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
