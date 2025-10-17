using JetBrains.Annotations;
using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject[] uiListable;

    private void DisableAll()
    {
        foreach (GameObject ui in this.uiListable)
            ui.SetActive(false);
    }
    public void PlayButtonMenu()
    {
        // CHECK ACCOUNT
        string accountJson = PlayerPrefs.GetString("Account");
        
        DisableAll();
        this.uiListable[0].SetActive(true);
    }

    public void RegisterButtonMenu()
    {   
        DisableAll();
        this.uiListable[1].SetActive(true);
    }

    public void LoginButtonMenu()
    {
        DisableAll();
        this.uiListable[0].SetActive(true);
    }
}
