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
    public float currentCooldown;

    // References
    public TMP_InputField cliInputField;
    public TMP_Text cliOutputField;
    public ScrollRect cliScrollRect;


    // Register commands here
    public Dictionary<string, GameCommand> allGameCommands = new() {
        {"help", new CMD_Help() },
        {"helloworld", new CMD_HelloWorld()},
        {"clear", new CMD_ClearScreen()},
        {"manual", new CMD_Manual()},
        
    };
    public Dictionary<string, GameCommand> hiddenCommandDict = new(){
        { "dl_run", new CMD_RunDialogue()},
        {"cls", new CMD_ClearScreen()},
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
            if(commandQueue.Count > 0)
            {
                ExecuteNextCommand();
            }
        }


        // Submit input line
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnLineSubmitted(cliInputField.text);
            cliInputField.text = "";
            cliInputField.ActivateInputField();
        }
    }


    public void OnLineSubmitted(string line)
    {
        if (string.IsNullOrEmpty(line)) return;

        string[] args = line.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
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

        commandQueue.Enqueue(command);
    }

    public void ExecuteNextCommand()
    {
        GameCommand curCommand = commandQueue.Dequeue();
        string inString = ">";
        foreach(string arg in curCommand.args)
        {
            inString += " " + arg;
        }

        PrintMessage(inString);
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
