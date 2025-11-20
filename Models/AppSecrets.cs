namespace BO_Mobile.Models;

/// <summary>
/// Model used to deserialize API credentials from the local-secrets.json file.
/// This file should NOT be committed to source control.
/// </summary>
public class AppSecrets
{
    // These property names must exactly match the keys in your local-secrets.json file.
    public string ApiUsername { get; set; }
    public string ApiPassword { get; set; }
}