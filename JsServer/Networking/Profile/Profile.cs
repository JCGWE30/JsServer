namespace JsServer;

public class Profile
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<ProfileProperty> Properties { get; set; }
    public List<object> ProfileActions { get; set; }
}