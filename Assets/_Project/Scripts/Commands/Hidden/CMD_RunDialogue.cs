using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_RunDialogue : GameCommand
{
    public string[] args { get; private set; }

    public float cooldownTime => 0;

    public CMD_RunDialogue()
    {
        args = new string[] { "dl_run"};
    }

    public CMD_RunDialogue(string[] args)
    {
        this.args = args;
    }

    public int Execute()
    {
        if(args.Length < 2)
        {
            return 1;
        }

        if (Int32.TryParse(args[1], out int diaIndex))
        {
            DialogueManager.instance.RunDialogue(diaIndex);
            return 0;
        }

        return 1;
    }

    public GameCommand instantiate(string[] args)
    {
        return new CMD_RunDialogue(args);
    }

    public string ShortUsage()
    {
        throw new System.NotImplementedException();
    }

}
