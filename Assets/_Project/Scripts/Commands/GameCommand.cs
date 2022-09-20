
public interface GameCommand
{

    public string[] args { get; }
    public int Execute();

    public string ShortUsage();
    
    public float cooldownTime { get; }

    public GameCommand instantiate(string[] args);
}
