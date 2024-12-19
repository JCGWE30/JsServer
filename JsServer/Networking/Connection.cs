namespace JsServer;

public class Connection
{
    public bool shouldDrop => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - lastActivity > timeontMs;

    private static readonly long timeontMs = (long) TimeSpan.FromSeconds(5).TotalSeconds;
    
    private long lastActivity;
    
    public Connection()
    {
        lastActivity = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}