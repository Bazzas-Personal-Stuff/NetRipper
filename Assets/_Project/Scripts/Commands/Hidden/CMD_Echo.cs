using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_Echo : GameCommand
{
    public string[] args {get; private set;}

    public string shortUsage => "Turn input echo on or off";
    public string longUsage => "<on|off>    Turn input echo on or off";

    public float cooldownTime => 0;

    public CMD_Echo()
    {
        args = new string[] { "echo" };
    }
    public CMD_Echo(string[] args)
    {
        this.args = args;
    }

    public int Execute()
    {
        if(args.Length > 1)
        {
            if (args[1].ToLower().Equals("on")){
                CommandLineManager.instance.echoOn = true;
                return 0;
            }
            if (args[1].ToLower().Equals("off"))
            {
                CommandLineManager.instance.echoOn = false;
                return 0;
            }

            string[] messageArr = new string[args.Length - 1];
            Array.Copy(args, 1, messageArr, 0, args.Length - 1);
            string message = String.Join(' ', messageArr);
            CommandLineManager.PrintMessage(message);
            return 0;
        }

        CommandLineManager.PrintMessage(CommandLineManager.instance.echoOn ? "on" : "off");
        return 0;
    }

    public GameCommand instantiate(string[] args)
    {
        return new CMD_Echo(args);
    }
}
