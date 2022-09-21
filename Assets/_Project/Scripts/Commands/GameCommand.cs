
public interface GameCommand
{

    public string[] args { get; }
    public int Execute();

    public string shortUsage { get; }
    public string longUsage { get; }
    
    public float cooldownTime { get; }

    public GameCommand instantiate(string[] args);
}
