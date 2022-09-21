using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_Disconnect : GameCommand
{
    public string[] args { get; private set; }
    public string shortUsage => "Disconnect from a remote device";
    public string longUsage => shortUsage;
    public float cooldownTime => 0;

    public CMD_Disconnect() {
        args = new[] { "disconnect" };
    }

    public CMD_Disconnect(string[] args) {
        this.args = args;
    }
    
    public int Execute() {
        if (!LevelManager.instance.isConnectedToRemote) {
            CommandLineManager.PrintMessage("Could not disconnect: not connected to a remote device");
            return 1;
        }
        
        LevelManager.instance.Disconnect();
        CommandLineManager.PrintMessage("Disconnected from remote device");
        return 0;
    }

    public GameCommand instantiate(string[] args) {
        return new CMD_Disconnect(args);
    }
}
