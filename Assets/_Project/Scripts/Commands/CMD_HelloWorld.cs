using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_HelloWorld : GameCommand
{
    public float cooldownTime => 0;
    private string[] _args;

    public int Execute()
    {
        string message = "Hello,";
        if(_args.Length > 1)
        {
            for(int i = 1; i < _args.Length; i++)
            {
                message += ' ' + _args[i];
            }
        }
        else
        {
            message += " World";
        }

        message += "!";
        CommandLineManager.PrintMessage(message);
        return 0;
    }

    public GameCommand instantiate()
    {
        return new CMD_HelloWorld();
    }

    public void SetArgs(string[] args)
    {
        _args = (string[])args.Clone();
    }

    public string ShortUsage()
    {
        return "[names]\tSay hello!";
    }
}
