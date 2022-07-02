using ConsoleApp1.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ConsoleApp1
{
    public class TelegramStartup
    {
        private readonly UserRepository _userRepository;
        private Dictionary<long, string>? nextStep;

        public TelegramStartup(UserRepository userRepository)
        {
            _userRepository = userRepository;
            nextStep = new Dictionary<long, string>();
        }
        private InlineKeyboardMarkup RegisterButtons() => new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Да", callbackData:"registerYes"),
                InlineKeyboardButton.WithCallbackData("Нет", callbackData:"registerNo"),
            }
        });
        private InlineKeyboardMarkup MenuButtons() => new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Добавить заметку", callbackData:"addTask"),
                InlineKeyboardButton.WithCallbackData("Просмотр заметок", callbackData:"showTasks"),
                InlineKeyboardButton.WithCallbackData("Установить напоминание", callbackData:"setReminder")
            }
        });
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
            {
                await HandleCallbackQuery(botClient, update.CallbackQuery);
            }
            if (update.Message is not { } message)
                return;
            if (message.Text is not { } messageText)
                return;
            long userId = message.Chat.Id;
            if (nextStep.TryGetValue(update.Message.Chat.Id, out string? value))
            {
                switch (value)
                {
                    case "regEnterName":
                        {
                            await _userRepository.AddUser(messageText, update.Message.Chat.Username!);
                            await botClient.SendTextMessageAsync
                                  (
                                  chatId: message.Chat.Id,
                                  text: $"Рад приветствовать вас, {message.From.Username!}!\nДоступные действия:",
                                  replyMarkup: MenuButtons(),
                                  cancellationToken: cancellationToken
                                  );
                            break;
                        }
                    default:
                        break;
                }
            }
            else
            {
                switch (messageText)
                {
                    case "/start":
                        {
                            if (_userRepository.IsExist(update.Message.From.Username))
                            {
                                await botClient.SendTextMessageAsync
                                    (
                                    chatId: message.Chat.Id,
                                    text: $"Рад приветствовать вас, {message.From.Username!}!\nДоступные действия:",
                                    replyMarkup: MenuButtons(),
                                    cancellationToken: cancellationToken
                                    );
                            }
                            else
                            {
                                await botClient.SendTextMessageAsync
                                    (
                                    chatId: message.Chat.Id,
                                    text: $"Рад приветствовать вас!\nВы не зарегистрированы, хотите пройти регистрацию?",
                                    replyMarkup: RegisterButtons(),
                                    cancellationToken: cancellationToken
                                    );
                            }
                            break;
                        }
                    default:
                        {
                            await botClient.SendTextMessageAsync(message.Chat, "Нет такой команды!");
                            break;
                        }
                }
                if (nextStep.TryGetValue(userId, out _))
                {
                    nextStep[userId] = "None";
                }
            }
        }
        public async Task HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            long userId = callbackQuery.Message.Chat.Id;
            switch (callbackQuery.Data)
            {
                case "registerYes":
                    {
                        string key = "regEnterName";
                        if (nextStep.TryGetValue(userId, out _))
                        {
                            nextStep[userId] = key;
                        }
                        else
                        {
                            nextStep.Add(userId, key);
                        }
                        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Введите ваше имя: ");
                        break;
                    }
                case "registerNo":
                    {
                        if (nextStep.TryGetValue(userId, out _))
                        {
                            nextStep[userId] = "None";
                        }
                        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Как передумаете, введите комманду /start");
                        break;
                    }
                default:
                    {
                        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Нет такой команды!");
                        break;
                    }
            }
            return;
        }
        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
