namespace FastMarketsBot.WebHookRunner
{
    public interface IWebApiConfiguration
    {
        string HostName { get; }
        string BotUrl { get; }
        string SubscribeKey { get; }
    }
}
