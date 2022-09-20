using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CommandLineManager : MonoBehaviour
{

    public static CommandLineManager instance;

    public Dictionary<string, GameCommand> commandDict = new();

    public Queue<GameCommand> commandQueue = new();
    public float currentCooldown;

    // References
    [SerializeField] private TMP_InputField _cliInputField;
    [SerializeField] private TMP_Text _cliOutputField;


    // Register commands here
    public Dictionary<string, GameCommand> allGameCommands = new() {
        {"help", new CMD_Help() },
        
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

        line = line.ToLower();
        string[] args = line.Split(' ');
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


    public static void PrintMessage(string message)
    {
        print(message);
        instance._cliOutputField.text += '\n' + message;
    }
}
