using System.CommandLine;
using ReDI;
using Re.Templates;
using Re.Templates.CLI;

public static class Program
{
    public static int? StatusCode = null;
    
    public static int Main(string[] args)
    {
        var containerBuilder = new ContainerBuilder();
        containerBuilder.AddModule<CliModule>();
        var container = containerBuilder.Build();
        var rootCommandFactory = container.Resolve<RootCliCommandFactory>();
        
        var rootCommand = rootCommandFactory.Create();

        rootCommand.Invoke(args);

        return StatusCode ?? 0;
    }
}