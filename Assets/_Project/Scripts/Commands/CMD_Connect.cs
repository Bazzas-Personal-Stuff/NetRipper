using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_Connect : GameCommand
{
    public string[] args { get; private set; }

    public string shortUsage => "Connect to a remote device";

    public string longUsage => "<key>    Connect to the device with the connection key specified, if one exists.";

    public float cooldownTime => 0;


    public CMD_Connect()
    {
        args = new string[] { "connect" };
    }
    public CMD_Connect(string[] args)
    {
        this.args = args;
    }

    public int Execute()
    {
        if(args.Length != 2)
        {
            CMD_Manual.PrintUsage(args);
            return 1;
        }

        // check level keys
        if (LevelManager.instance.levelDict.ContainsKey(args[1])) {
            CommandLineManager.PrintMessage($"Connected to device \"{args[1]}\"");
            LevelManager.instance.Connect(args[1]);
            return 0;
        }
        else {
            CommandLineManager.PrintMessage($"Could not connect to device \"{args[1]}\"");
            return 1;
        }
    }

    public GameCommand instantiate(string[] args)
    {
        return new CMD_Connect(args);
    }
}
