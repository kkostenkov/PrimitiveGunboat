public interface ICommandSource
{
    bool HasCommand(CommandType commandType);
    InputCommand GetLastCommand(CommandType commandType);
}