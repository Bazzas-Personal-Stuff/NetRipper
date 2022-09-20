using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_ClearScreen : GameCommand
{

    public string[] args { get; private set; }

    public float cooldownTime => 0;

    public CMD_ClearScreen()
    {
        args = new string[] { "cls" };
    }

    public CMD_ClearScreen(string[] args)
    {
        this.args = args;
    }

    public int Execute()
    {
        CommandLineManager.instance.cliOutputField.text = "";
        return 0;
    }

    public GameCommand instantiate(string[] args)
    {
        return new CMD_ClearScreen(args);
    }

    public string ShortUsage()
    {
        return "Clear the terminal screen";
    }
}
