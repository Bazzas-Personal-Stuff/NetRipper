
public interface GameCommand
{
    public void SetArgs(string[] args);

    public int Execute();

    public string ShortUsage();
    
    public float cooldownTime { get; }

    public GameCommand instantiate();
}
