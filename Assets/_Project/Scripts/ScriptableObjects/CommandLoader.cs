using UnityEngine;

[CreateAssetMenu(fileName = "CommandLoader", menuName = "ScriptableObjects/CommandLoader")]
public class CommandLoader : ScriptableObject {
    public string[] commands;

    public void LoadCommands() {
        CommandLineManager.instance.LoadCommand(this);
    }
}