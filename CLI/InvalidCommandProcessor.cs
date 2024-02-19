using ReTemplate.CLI.Consts;

namespace ReTemplate.CLI;

public class InvalidCommandProcessor : ICommandProcessor
{
    private readonly string _command;
    public InvalidCommandProcessor(string command)
    {
        _command = command;
    }

    public void Process(List<string> args)
    {
        Console.WriteLine(Error.InvalidCommandError, _command);
    }
}