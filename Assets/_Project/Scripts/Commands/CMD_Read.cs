using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_Read : GameCommand
{
    public string[] args { get; }
    public string shortUsage => "Read the contents of a file";
    public string longUsage => "<file>    Read the contents of the specified file\nAliases: read, cat";
    public float cooldownTime => 0;

    public CMD_Read() {
        args = new[] { "read" };
    }

    public CMD_Read(string[] args) {
        this.args = args;
    }
    
    public int Execute() {
        if (!LevelManager.instance.isConnectedToRemote) {
            CommandLineManager.PrintMessage("Read failed: Not connected to a file system");
            return 1;
        }

        if (args.Length != 2) {
            CMD_Manual.PrintUsage(args);
            return 1;
        }

        foreach (File file in LevelManager.instance.workingDir.files) {
            if (file.name.Equals(args[1])) {
                CommandLineManager.PrintMessage(file.readableText);
                file.Read();
                return 0;
            }
        }

        CommandLineManager.PrintMessage($"Read failed: File {args[1]} does not exist");
        return 1;

    }

    public GameCommand instantiate(string[] args) {
        return new CMD_Read(args);
    }
}
