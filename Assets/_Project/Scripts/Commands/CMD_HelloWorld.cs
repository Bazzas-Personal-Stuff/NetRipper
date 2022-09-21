using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_HelloWorld : GameCommand
{
    public float cooldownTime => 0;
    public string[] args { get; private set; }
    public string shortUsage => "Print a nice welcome message";
    public string longUsage => "[names]    Print a nice welcome message. If names are passed in as additional arguments, you will be greeted personally.";


    public CMD_HelloWorld()
    {
        args = new string[]{ "helloworld"};
    }

    public CMD_HelloWorld(string[] args)
    {
        this.args = args;
    }

    public int Execute()
    {
        string message = "Hello,";
        if(args.Length > 1)
        {
            for(int i = 1; i < args.Length; i++)
            {
                message += ' ' + args[i];
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

    public GameCommand instantiate(string[] args)
    {
        return new CMD_HelloWorld(args);
    }


    public string ShortUsage()
    {
        return "[names]\tSay hello!";
    }
}
