using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_NotFound : GameCommand
{
    public float cooldownTime => 0;

    private string commandName;

    public GameCommand instantiate()
    {
        return new CMD_NotFound();
    }

    public int Execute()
    {
        CommandLineManager.PrintMessage($"\"{commandName}\" is not a valid command. Use the \"help\" command to display valid commands.");
        return 0;
    }

    public string ShortUsage()
    {
        return "";
    }

    public void SetArgs(string[] args)
    {
        commandName = args[0];
    }
}
