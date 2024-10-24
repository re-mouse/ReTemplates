using System.CommandLine;

namespace Re.Template.CLI;

public interface ICliCommand
{
    public Command AsCLICommand();
}