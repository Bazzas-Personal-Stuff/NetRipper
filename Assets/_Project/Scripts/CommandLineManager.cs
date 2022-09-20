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
    [SerializeField] private TMP_InputField _cliInputField;
    [SerializeField] private TMP_Text _cliOutputField;
    [SerializeField] private ScrollRect _cliScrollRect;


    // Register commands here
    public Dictionary<string, GameCommand> allGameCommands = new() {
        {"help", new CMD_Help() },
        {"helloworld", new CMD_HelloWorld()},
        
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
        if(currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
        else
        {
            if(commandQueue.Count > 0)
            {
                GameCommand curCommand = commandQueue.Dequeue();
                
                curCommand.Execute();
            }
        }


        // Submit input line
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnLineSubmitted(_cliInputField.text);
            _cliInputField.text = "";
            _cliInputField.ActivateInputField();
        }
    }


    public void OnLineSubmitted(string line)
    {
        if (string.IsNullOrEmpty(line)) return;

        string[] args = line.Split(' ');
        args[0] = args[0].ToLower();
        GameCommand command;

        if (!commandDict.ContainsKey(args[0]))
        {
            command = new CMD_NotFound();
        } else
        {
            command = commandDict[args[0]].instantiate();
        }

        command.SetArgs(args);
        commandQueue.Enqueue(command);
    }

    public void ScrollOutputToBottom()
    {
        StartCoroutine(ScrollOutputToBottomCoroutine());
    }

    private IEnumerator ScrollOutputToBottomCoroutine()
    {
        yield return 0;
        _cliScrollRect.normalizedPosition = Vector2.zero;
    }


    public static void PrintMessage(string message)
    {
        // Make sure we don't go over the TMP character limit
        if(instance._cliOutputField.text.Length > 20_000)
        {
            instance._cliOutputField.text = instance._cliOutputField.text.Substring(10_000);
        }

        instance._cliOutputField.text += '\n' + message;
        instance.ScrollOutputToBottom();
    }
}
