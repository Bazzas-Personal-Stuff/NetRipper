using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_Manual : GameCommand
{
    public string[] args { get; private set; }

    public float cooldownTime => 0;

    public int Execute()
    {
        throw new System.NotImplementedException();
    }

    public GameCommand instantiate(string[] args)
    {
        throw new System.NotImplementedException();
    }

    public string ShortUsage()
    {
        throw new System.NotImplementedException();
    }

}
