using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace JsServer;

public class ProfileProperty
{
    private string Name;
    private string Value;
    private string? Signature;
    public ProfileProperty(string Name, string Value, string? Signature = null)
    {
        this.Name = Name;
        this.Value = Value;
        this.Signature = Signature;
    }

    public String GetName()
    {
        return Name;
    }

    public String GetValue()
    {
        return Value;
    }

    
    public bool HasSignature(out String signature)
    {
        signature = this.Signature;
        return this.Signature == null;
    }
}