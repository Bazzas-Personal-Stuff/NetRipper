using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommandLineManager : MonoBehaviour
{

    public static CommandLineManager instance;

    public Dictionary<string, GameCommand> commandDict = new();

    public Queue<GameCommand> commandQueue = new();
    public Queue<GameCommand> silentCommandQueue = new();
    public bool echoOn = false;
    public float currentCooldown = 2f;

    // References
    public TMP_InputField cliInputField;
    public TMP_Text cliOutputField;
    public ScrollRect cliScrollRect;



    // Register commands here
    public Dictionary<string, GameCommand> allGameCommands = new() {
        {"help", new CMD_Help() },
        {"hello_world", new CMD_HelloWorld()},
        {"clear", new CMD_ClearScreen()},
        {"manual", new CMD_Manual()},
        { "connect", new CMD_Connect()},
        {"disconnect", new CMD_Disconnect()},
        {"list", new CMD_List()},
        {"navigate", new CMD_Navigate()},
        {"read", new CMD_Read()},
        {"rip", new CMD_Rip()},
    };
    public Dictionary<string, GameCommand> hiddenCommandDict = new() {
        // Internal
        { "echo", new CMD_Echo() },

        // Debug
        { "dbg_dialogue", new CMD_RunDialogue() },

        // Aliases
        { "cls", new CMD_ClearScreen() },
        { "man", new CMD_Manual() },
        {"ls", new CMD_List()},
        {"cd", new CMD_Navigate()},
        {"nav", new CMD_Navigate()},
        {"quit", new CMD_Disconnect()},
        {"logout", new CMD_Disconnect()},
        {"exit", new CMD_Disconnect()},
        {"cat", new CMD_Read()},
    };

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }


    private void Start()
    {

        // TODO: Remove this when level loading is in the game
        foreach(var key in allGameCommands.Keys)
        {
            LoadCommand(key);
        }

        //SubmitCommand("os_welcome");
        SubmitSilentCommand("echo <color=#797979>NetWeaver OS v3.4.1\nWelcome! Use the \"help\" command to see your available programs.</color>");
        SubmitCommand("echo on");

    }

  
    public void LoadCommand(string commandName)
    {
        if (allGameCommands.ContainsKey(commandName))
        {
            commandDict.Add(commandName, allGameCommands[commandName]);
        }
    }


    public void Update()
    {

        // TODO: Remove this once ui raycast is back in
        if (!cliInputField.isFocused)
        {
            cliInputField.ActivateInputField();
        }

        if(currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
        else
        {
            if (silentCommandQueue.Count > 0) {
                ExecuteNextSilentCommand();
            }
            else if(commandQueue.Count > 0)
            {
                ExecuteNextCommand();
            }
        }


        // Submit input line
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SubmitCommand(cliInputField.text);
            cliInputField.text = "";
            cliInputField.ActivateInputField();
        }
    }


    public void SubmitCommand(string line)
    {
        if (string.IsNullOrEmpty(line)) return;

        string[] args = line.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);

        commandQueue.Enqueue(FindCommand(args));
    }
    
    public void SubmitSilentCommand(string line)
    {
        if (string.IsNullOrEmpty(line)) return;

        string[] args = line.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);

        silentCommandQueue.Enqueue(FindCommand(args));
    }

    public GameCommand FindCommand(string arg)
    {
        return FindCommand(new string[] { arg });
    }

    public GameCommand FindCommand(string[] args)
    {
        string commandStrLower = args[0].ToLower();
        GameCommand command;
        if (!commandDict.ContainsKey(commandStrLower))
        {
            if (!hiddenCommandDict.ContainsKey(commandStrLower))
            {
                command = new CMD_NotFound(args);
            } else
            {
                command = hiddenCommandDict[commandStrLower].instantiate(args);
            }
        } else
        {
            command = commandDict[commandStrLower].instantiate(args);
        }

        return command;
    }

    public void ExecuteNextCommand()
    {
        GameCommand curCommand = commandQueue.Dequeue();
        if (echoOn)
        {
            string inString = ">";
            foreach(string arg in curCommand.args)
            {
                inString += " " + arg;
            }

            PrintMessage(inString);
        }
        
        curCommand.Execute();
        currentCooldown = curCommand.cooldownTime;
    }

    public void ExecuteNextSilentCommand() {
        GameCommand curCommand = silentCommandQueue.Dequeue();
        curCommand.Execute();
        currentCooldown = curCommand.cooldownTime;
    }

    public void ScrollOutputToBottom()
    {
        StartCoroutine(ScrollOutputToBottomCoroutine());
    }

    private IEnumerator ScrollOutputToBottomCoroutine()
    {
        yield return 0;
        cliScrollRect.normalizedPosition = Vector2.zero;
    }


    public static void PrintMessage(string message)
    {
        // Make sure we don't go over the TMP character limit
        if(instance.cliOutputField.text.Length > 20_000)
        {
            instance.cliOutputField.text = instance.cliOutputField.text.Substring(10_000);
        }

        instance.cliOutputField.text += '\n' + message;
        instance.ScrollOutputToBottom();
    }
}
