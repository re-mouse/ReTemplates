﻿using System.CommandLine;
using ReDI;
using Re.Template;
using Re.Template.CLI;

public static class Program
{
    public static int Main(string[] args)
    {
        var containerBuilder = new ContainerBuilder();
        containerBuilder.AddModule<CliModule>();
        var container = containerBuilder.Build();
        var rootCommandFactory = container.Resolve<RootCliCommandFactory>();
        
        var rootCommand = rootCommandFactory.Create();

        return rootCommand.Invoke(args);
    }
}