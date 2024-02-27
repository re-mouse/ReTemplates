using System.CommandLine;

namespace ReTemplate.CLI;

public interface ICliCommand
{
    public Command AsCLICommand();
}