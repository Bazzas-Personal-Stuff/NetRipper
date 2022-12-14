using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_List : GameCommand
{
    public string[] args { get; private set; }
    public string shortUsage => "List a directory's files and neighbors";
    public string longUsage => "List the files and neighboring directories of the current working directory\nAliases: list, ls";
    public float cooldownTime => 0;

    public CMD_List() {
        args = new[] { "list" };
    }

    public CMD_List(string[] args) {
        this.args = args;
    }
    public int Execute() {
        if (!LevelManager.instance.isConnectedToRemote) {
            CommandLineManager.PrintMessage("List failed: Not connected to a file system");
            return 1;
        }

        Directory currentDir = LevelManager.instance.workingDir;
        currentDir.List();
        
        List<string> outStrings = new();
        foreach (Directory dir in currentDir.connected) {
            dir.Ping();
            outStrings.Add($"(dir) {dir.name}");
        }

        foreach (File file in currentDir.files) {
            file.Ping();
            outStrings.Add($"(file) {file.name}");
        }

        string outStr = String.Join('\n', outStrings);
        CommandLineManager.PrintMessage(outStr);
        
        return 0;
    }
    public GameCommand instantiate(string[] args) {
        return new CMD_List(args);
    }
}
