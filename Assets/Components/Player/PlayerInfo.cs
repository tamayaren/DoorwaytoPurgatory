using AccountNetwork;
using Fusion;
using TMPro;
using UnityEngine;

public class PlayerInfo : NetworkBehaviour
{
    [Networked] public NetworkString<_16> username { get; set; }
    [Networked] public NetworkString<_16> level { get; set; }
    [Networked] public NetworkString<_16> exp { get; set; }
    [Networked] public NetworkString<_16> lastPlayed { get; set; }

    private float time;
    private ServerAccount account;
    [SerializeField] private TextMeshProUGUI usernameText;
    
    public override void Spawned()
    {
        base.Spawned();
        RemoteGetPlayerAccount();
    }

    public void RemoteGetPlayerAccount()
    {
        if (this.HasInputAuthority)
        {
            string acc = PlayerPrefs.GetString("Account");
            
            if (acc != null)
            {
                this.account = JsonUtility.FromJson<ServerAccount>(acc);

                this.username = this.account.username;
                this.level = this.account.level.ToString();
                this.exp = this.account.exp.ToString();
                this.lastPlayed = this.account.played.ToString();
                
                this.usernameText.text = this.account.username;
            }
        }
    }
    private void Update()
    {
        this.usernameText.transform.LookAt(Camera.main.transform.position + Camera.main.transform.forward * 256f);
    }

    public override void FixedUpdateNetwork()
    {
        if (this.HasInputAuthority)
        {
            if (this.time > 1f)
            {
                IncrementEXP();
                this.time = 0f;
            }
            this.time += this.Runner.DeltaTime;
        }
        
        base.FixedUpdateNetwork();
    }

    private void Save()
    {   
        PlayerPrefs.SetString("Account", JsonUtility.ToJson(this.account));
        ServerAccountSystem.instance.UpdateAccount(this.account);
    }
    
    public void IncrementEXP()
    {
        this.account.exp++;

        Save();
    }
}
