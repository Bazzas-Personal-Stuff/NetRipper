using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_NotFound : GameCommand
{
    public float cooldownTime => 0;
    public string[] args { get; private set; }
    public string shortUsage => "Error message when a command is not available";
    public string longUsage => "Command not recognised";


    public CMD_NotFound()
    {
        args = new string[]{ "notfound"};
    }

    public CMD_NotFound(string[] args)
    {
        this.args = args;
    }


    public GameCommand instantiate(string[] args)
    {
        return new CMD_NotFound(args);
    }

    public int Execute()
    {
        CommandLineManager.PrintMessage($"<color=#FF5555>\"{args[0]}\" is not a valid command. Use the \"help\" command to display valid commands.</color>");
        return 0;
    }


}
