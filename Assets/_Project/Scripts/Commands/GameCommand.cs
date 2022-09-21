
public interface GameCommand
{

    public string[] args { get; }

    public string shortUsage { get; }
    public string longUsage { get; }
    
    public float cooldownTime { get; }

    public int Execute();
    public GameCommand instantiate(string[] args);
}
