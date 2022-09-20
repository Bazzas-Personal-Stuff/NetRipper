using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_Help : GameCommand
{
    public float cooldownTime => 0;
    public string[] args { get; private set; }



    public CMD_Help()
    {
        args = new string[]{ "help"};
    }
    public CMD_Help(string[] args)
    {
        this.args = args;
    }

    public string ShortUsage()
    {
        return "Display this message";
    }


    public int Execute()
    {
        // Print all available commands
        var commandKeys = CommandLineManager.instance.commandDict.Keys;
        string[] commandNames = new string[commandKeys.Count];
        commandKeys.CopyTo(commandNames, 0);
        Array.Sort(commandNames);

        foreach(string commandName in commandNames)
        {
            CommandLineManager.PrintMessage($"{commandName}\t {CommandLineManager.instance.commandDict[commandName].ShortUsage()}");
        }

        return 0;
    }

    public GameCommand instantiate(string[] args)
    {
        return new CMD_Help(args);
    }

}
