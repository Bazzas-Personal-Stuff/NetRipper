using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_Navigate : GameCommand
{
    public string[] args { get; private set; }
    public string shortUsage { get; }
    public string longUsage { get; }
    public float cooldownTime { get; }

    public CMD_Navigate() {
        args = new[] { "navigate" };
    }
    
    public CMD_Navigate(string[] args) {
        this.args = args;
    }
    
    public int Execute() {

        if (!LevelManager.instance.isConnectedToRemote) {
            CommandLineManager.PrintMessage("Navigate failed: Not connected to a file system");
            return 1;
        }
        
        if (args.Length != 2) {
            CMD_Manual.PrintUsage(args);
            return 1;
        }


        string[] path = args[1].Split('/', StringSplitOptions.RemoveEmptyEntries);
        Directory contextDir = LevelManager.instance.workingDir;
        List<Directory> dirsToPing = new(path.Length);
        bool isPathGood = true;
        for (int i = 0; i < path.Length; i++) {
            string searchDir = path[i];
            bool isDirGood = false;
            foreach (Directory d in contextDir.connected) {
                if (d.name.Equals(searchDir)) {
                    dirsToPing.Add(d);
                    isDirGood = true;
                    contextDir = d;
                    break;
                }
            }

            if (!isDirGood) {
                isPathGood = false;
                break;
            }
        }

        if (isPathGood) {
            foreach (Directory d in dirsToPing) {
                d.Ping();
            }

            LevelManager.instance.workingDir = dirsToPing[^1];
            LevelManager.instance.workingDir.Visit();
            return 0;
        }
        else {
            CommandLineManager.PrintMessage($"Navigate failed: {args[1]} is not a valid path");
            return 1;
        }
    }
    
    public GameCommand instantiate(string[] args) {
        return new CMD_Navigate(args);
    }
}
