namespace ReTemplate.CLI;

public interface ICommandProcessor
{
    public void Process(List<string> args);
}