namespace FastMarketsBot.Services.Telegram.Commands
{
    public interface ICommandFactory
    {
        ICommand GetCommand(CommandType commandType);
        ICommand GetCommand(string messageText);
    }
}
