public class CMD_Rip : GameCommand {
    public string[] args { get; }
    public string shortUsage => "Download a file from a remote device";
    public string longUsage => "<file>    Download the specified file from the remote device to your local device";
    public float cooldownTime => 0;

    public CMD_Rip() {
        args = new[] { "rip" };
    }

    public CMD_Rip(string[] args) {
        this.args = args;
    }
    
    
    public int Execute() {
        if (!LevelManager.instance.isConnectedToRemote) {
            CommandLineManager.PrintMessage("Rip failed: Not connected to a file system");
            return 1;
        }

        if (args.Length != 2) {
            CMD_Manual.PrintUsage(args);
        }

        foreach (File file in LevelManager.instance.workingDir.files) {
            if (file.name.Equals(args[1])) {
                CommandLineManager.PrintMessage($"Ripped file {args[1]}");
                file.Rip();
                return 0;
            }
        }

        CommandLineManager.PrintMessage($"Rip failed: File {args[1]} does not exist");
        return 1;
    }

    public GameCommand instantiate(string[] args) {
        return new CMD_Rip(args);
    }
}