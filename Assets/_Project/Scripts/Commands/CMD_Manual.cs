using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_Manual : GameCommand
{
    public string[] args { get; private set; }

    public float cooldownTime => 0;

    public string shortUsage => "Print usage of a command";
    public string longUsage => "<command>    Print usage of the specified command";

    public CMD_Manual()
    {
        args = new string[] { "manual" };
    }

    public CMD_Manual(string[] args)
    {
        this.args = args;
    }

    public int Execute()
    {
        return PrintUsage(args);
    }

    public static int PrintUsage(string[] args)
    {
        GameCommand command;
        if(args.Length != 2)
        {
            command = CommandLineManager.instance.FindCommand(args[0]);
            CommandLineManager.PrintMessage($"<color=#00FF00>{args[0]}</color>\t{command.longUsage}");
            return 0;
        }

        command = CommandLineManager.instance.FindCommand(args[1]);
        if(command.longUsage == "Command not recognised")
        {
            CommandLineManager.PrintMessage(command.longUsage);
            return 1;
        }

        CommandLineManager.PrintMessage($"<color=#00FF00>{args[1]}</color>\t{command.longUsage}");
        return 0;

    }

    public GameCommand instantiate(string[] args)
    {
        return new CMD_Manual(args);
    }


}
