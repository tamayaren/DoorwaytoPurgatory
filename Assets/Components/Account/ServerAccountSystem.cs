using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

namespace AccountNetwork
{
    public class ServerAccountSystem : MonoBehaviour
    {
        public static ServerAccountSystem instance;
        private string serverPath;
        private ServerAccountDatabase database;
        
        private ServerAccountDatabase ReadServerDatabase()
        {
            bool exists = File.Exists(this.serverPath);

            if (exists)
            {
                string json = File.ReadAllText(this.serverPath);
                ServerAccountDatabase database = JsonUtility.FromJson<ServerAccountDatabase>(json);
                
                if (database == null || database.accounts == null)
                    database = new ServerAccountDatabase();
                
                return database;
            }
            
            this.database = new ServerAccountDatabase();
            SaveDatabase();
            
            return this.database;
        }

        public ServerAccount GetServerAccount(string name, string password)
        {
            foreach (ServerAccount serverAccount in this.database.accounts)
            {
                if (serverAccount.username == name)
                    if (Hash128.Compute(password).ToString() == serverAccount.pass)
                        return serverAccount;
                    
            }

            return null;
        }
    
        private void Awake()
        {
            instance = this;

            this.serverPath = Path.Combine(Application.dataPath, "server_database.json");
        }

        public void SaveDatabase()
        {
            Debug.Log(this.database);
            string json = JsonUtility.ToJson(this.database);
            Debug.Log(json);
            File.WriteAllText(this.serverPath, json);
        }

        public void Initialize()
        {
            this.database = ReadServerDatabase();
            
            Debug.Log(this.database);
        }

        private void Start() => Initialize();
        
        public bool AccountNameExistence(string name)
        {
            foreach (ServerAccount account in this.database.accounts)
            {
                if (account.username == name) return true;
            }

            return false;
        }

        public void Register(string name, string pass, string email)
        {
            ServerAccount account = new ServerAccount
            {
                username = name,
                pass = Hash128.Compute(pass).ToString(),
                email = Hash128.Compute(email).ToString()
            };

            this.database.accounts.Add(account);
            SaveDatabase();
        }

        public void UpdateAccount(ServerAccount account)
        {
            ServerAccount serverAccount = JsonUtility.FromJson<ServerAccount>(PlayerPrefs.GetString("account"));

            foreach (ServerAccount acc in this.database.accounts)
            {
                if (acc.username == account.username && acc.pass.Equals(account.pass))
                {
                    this.database.accounts.Remove(acc);
                    this.database.accounts.Add(account);
                    SaveDatabase();
                    break;
                }
            }
        }
    }

    [System.Serializable]
    public class ServerAccount
    {
        // implement security later
        public string username;
        public string email;
        public string pass;
        public int level = 0;
        public int exp  = 0;
        public int played = 0;
    }

    [System.Serializable]
    public class ServerAccountDatabase
    {
        public List<ServerAccount> accounts;
    }
}
