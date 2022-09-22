using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_Help : GameCommand
{
    public float cooldownTime => 0;
    public string[] args { get; private set; }
    public string shortUsage => "Display available commands";
    public string longUsage => shortUsage;
    public string regularColor = "#00FF00";
    public string helpHintColor = "#FFCC00";

    public CMD_Help()
    {
        args = new string[]{ "help"};
    }
    public CMD_Help(string[] args)
    {
        this.args = args;
    }


    public int Execute()
    {
        // Print all available commands
        var commandKeys = CommandLineManager.instance.commandDict.Keys;
        string[] commandNames = new string[commandKeys.Count];
        commandKeys.CopyTo(commandNames, 0);
        Array.Sort(commandNames);
        CommandLineManager.PrintMessage("Available commands:");

        foreach(string commandName in commandNames) {
            string commandColor = CommandLineManager.instance.hintCommands.Contains(commandName)
                ? helpHintColor
                : regularColor;
            CommandLineManager.PrintMessage($"<color={commandColor}>{commandName}</color>\t {CommandLineManager.instance.commandDict[commandName].shortUsage}");
        }

        return 0;
    }

    public GameCommand instantiate(string[] args)
    {
        return new CMD_Help(args);
    }

}
