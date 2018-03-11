using System;

namespace FastMarketsBot.Services.Telegram
{
    public interface ITelegramService : IDisposable
    {
        void StayingAlive();
        void Register();
    }
}
