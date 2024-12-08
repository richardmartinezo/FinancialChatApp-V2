using FinancialChatApp_V2.Models;

namespace FinancialChatApp_V2.Services
{
    public class StockBotService
    {
        private readonly RabbitMQService _rabbitMQService;
        public StockBotService(RabbitMQService rabbitMQService) 
        {
            _rabbitMQService = rabbitMQService;
        
        }
        public async Task StartListening() 
        {
            await _rabbitMQService.ReceiveMessageAsync(async (message) => {

                if (message.StartsWith("/stock=")) 
                {
                    var stockCode = message.Substring(7);
                    var stockPrice = await GetStockPriceAsync(stockCode);

                    var response = stockPrice != null
                        ? $"{stockCode.ToUpper()} quote is {stockPrice} per share"
                        : $"Failed to fetch stock price for {stockCode}";

                    await _rabbitMQService.SendMessageAsync(response);
                }
            
            });
        }
        public async Task<string?> GetStockPriceAsync(string stockCode) 
        {
            var url = $"https://stooq.com/q/l/?s={stockCode}&f=sd2t2ohlcv&h&e=csv";
            using var client = new HttpClient();

            var response = await client.GetStringAsync(url);
            var lines = response.Split('\n');
            if (lines.Length > 1)
            {
                var fields = lines[1].Split(',');
                return fields.Length > 6 ? fields[6] : null;
            }

            return string.Empty;
        }


    }
}
