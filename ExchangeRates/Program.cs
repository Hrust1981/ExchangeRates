using GetRequestCBRF;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExchangeRates
{
    class Program
    {
        private static ITelegramBotClient botClient;
        private static GetRequest? request;
        private static XmlGetData getData;
        private static InlineKeyboardMarkup inlineKeyboard;
        private static List<Currency> currencies;
        private static List<string> currenciesList;
        private static string? date;

        static void Main(string[] args)
        {
            botClient = new TelegramBotClient(Settings.TOKEN);

            inlineKeyboard = new InlineKeyboardMarkup (
                new[] 
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(
                            Strings.exRateToday, "ExRateToday")
                    }
            });
            
            var me = botClient.GetMeAsync().Result;

            Console.WriteLine($"Bot ID - {me.Id} : bot's name - {me.FirstName}");
            currenciesList = new List<string>()
        {
            "GBP",
            "USD",
            "EUR",
            "JPY",
            "CHF"
        };

            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
            
            botClient.OnCallbackQuery += BotClient_OnCallbackQuery;

            Console.ReadKey();
        }

        private async static void BotClient_OnCallbackQuery(object? sender, CallbackQueryEventArgs e)
        {
            InlineKeyboardMarkup iKCurrencyChoise = null;
            string exRate = null, ticker = null, text = null;

            date = DateTime.Today.ToString("dd/MM/yyyy");

            if (e.CallbackQuery.Data == "ExRateToday")
            {
                iKCurrencyChoise = new InlineKeyboardMarkup(
                    new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("USD", "USD"),
                            InlineKeyboardButton.WithCallbackData("EUR", "EUR"),
                            InlineKeyboardButton.WithCallbackData("GBP", "GBP"),
                            InlineKeyboardButton.WithCallbackData("JPY", "JPY"),
                            InlineKeyboardButton.WithCallbackData("CHF", "CHF")
                        }
                    });

                request = new GetRequest($"http://www.cbr.ru/scripts/XML_daily.asp?date_req={date}");
                request.Run();

                var response = request.Response;
                getData = new XmlGetData(response);
                currencies = getData.GetExchangeRate();
                
                text = "Выбери пож-та валюту\n";

            }
            else
            {
                foreach (var currencyList in currenciesList)
                {
                    if (e.CallbackQuery.Data == currencyList) ticker = currencyList;
                }
            }

            var data = new DataSelection(currencies);
            exRate = data.Run(ticker);

            if (exRate != null) text = $"Курс валют на {getData.GetDate()}\n";

            await botClient.SendTextMessageAsync(
                    chatId: e.CallbackQuery.Message.Chat.Id,
                    text: text + exRate,
                    replyMarkup: iKCurrencyChoise
            ).ConfigureAwait(false);
        }

        private static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            var message = e?.Message?.Text;

            if (message == "/start")
            {
                await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: Strings.welcome,
                    replyMarkup: inlineKeyboard
                ).ConfigureAwait(false);
            }
        }
    }
}