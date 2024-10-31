using System.CommandLine;

namespace Re.Templates.CLI;

public interface ICliCommand
{
    public Command AsCLICommand();
}