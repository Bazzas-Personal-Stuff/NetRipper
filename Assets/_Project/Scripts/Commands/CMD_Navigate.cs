using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_Navigate : GameCommand
{
    public string[] args { get; private set; }
    public string shortUsage => "Navigate directories in a file system";
    public string longUsage => "<directory|path>    Move to the specified directory. If a path is specified instead, jump to the directory at the end of the specified path\nAliases: navigate, nav, cd";
    public float cooldownTime { get; private set; }
    

    private float timePerJump = 0.3f;

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
            cooldownTime = timePerJump * (dirsToPing.Count - 1);
            LevelManager.instance.StartCoroutine(DoTraversal(dirsToPing));
            // foreach (Directory d in dirsToPing) {
            //     d.Ping();
            // }
            //
            // LevelManager.instance.workingDir = dirsToPing[^1];
            // LevelManager.instance.workingDir.Visit();
            return 0;
        }
        else {
            CommandLineManager.PrintMessage($"Navigate failed: {args[1]} is not a valid path");
            return 1;
        }
    }

    private IEnumerator DoTraversal(List<Directory> path) {
        for(int i = 0; i < path.Count-1; i++) {
            LevelManager.instance.workingDir = path[i];
            LevelManager.instance.UpdatePlayerPos();
            path[i].Ping();
            yield return new WaitForSeconds(timePerJump);
        }

        LevelManager.instance.workingDir = path[^1];
        LevelManager.instance.workingDir.Visit();
    }
    
    public GameCommand instantiate(string[] args) {
        return new CMD_Navigate(args);
    }
}
