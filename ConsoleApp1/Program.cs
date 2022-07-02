using ConsoleApp1.Models;
using ConsoleApp1.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO.Compression;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ConsoleApp1;

class Program
{
    static async Task Main()
    {
        var botClient = new TelegramBotClient("5485510895:AAFpvTK2pp8puDzxZiffnvPKXr26A0Hu2VE");
        using var cts = new CancellationTokenSource();

        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>() // получаем все типы обновлений
        };

        UserRepository userRepository = new UserRepository(new ApplicationContext(GetConfig()));
        TelegramStartup telegramStartup = new TelegramStartup(userRepository);

        botClient.StartReceiving(
          updateHandler: telegramStartup.HandleUpdateAsync,
          pollingErrorHandler: telegramStartup.HandlePollingErrorAsync,
          receiverOptions: receiverOptions,
          cancellationToken: cts.Token
      );
        var me = await botClient.GetMeAsync();

        Console.WriteLine($"Бот под именем @{me.Username}, запущен.");
        Console.ReadLine();
        cts.Cancel();
    }    
    static DbContextOptions<ApplicationContext> GetConfig()
    {
        var builder = new ConfigurationBuilder();
        // установка пути к текущему каталогу
        builder.SetBasePath(Directory.GetCurrentDirectory());
        // получаем конфигурацию из файла appsettings.json
        builder.AddJsonFile("appsettings.json");
        // создаем конфигурацию
        var config = builder.Build();
        // получаем строку подключения
        string connectionString = config.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
        var options = optionsBuilder.UseSqlServer(connectionString).Options;

        return options;
    }

}