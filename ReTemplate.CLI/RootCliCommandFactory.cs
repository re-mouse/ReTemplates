using System.CommandLine;
using ReDI;

namespace ReTemplate.CLI;

public class RootCliCommandFactory
{
    [Inject] private List<ICliCommand> _commands;

    public RootCommand Create()
    {
        var rootCommand = new RootCommand("Create templates and save them in repository, create files and directories from that template");
        foreach (var subcommand in _commands)
        {
            rootCommand.AddCommand(subcommand.AsCLICommand());
        }

        rootCommand.TreatUnmatchedTokensAsErrors = true;

        return rootCommand;
    }
}