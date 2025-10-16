
using System.IO;
using System.Xml.Schema;
using JetBrains.Annotations;
using UnityEngine;

public interface IAccount
{
    string username { get; }
    string email { get; set; }
    Hash128 pass { get; }
    int level { get; set; }
    int exp { get; set; }
    int played { get; set; }
}
public class Account : MonoBehaviour, IAccount
{
    public string username { get; }
    public string email { get; set; }
    public Hash128 pass { get;  }
    public int level { get; set; }
    public int exp { get; set; }
    public int played { get; set; }

    public Account(string name, string email, [CanBeNull] string pass)
    {
        string? account = PlayerPrefs.GetString("Account");
        Account existing = null;
        if (account != null)
            existing = JsonUtility.FromJson<Account>(account);
        
        this.username = existing ? existing.username : name;
        
        if (pass != null)
            this.pass = Hash128.Parse(pass);
        
        this.email = existing ? existing.email : email;
        
        this.level = existing != null ? existing.level : 0;
        this.exp = existing != null ? existing.exp : 0;
        this.played = existing != null ? existing.played : 0;
    }
}
