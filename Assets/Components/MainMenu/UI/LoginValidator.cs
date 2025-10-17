using System.Collections;
using AccountNetwork;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginValidator : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private TMP_InputField passwordField;
    
    [SerializeField] private TextMeshProUGUI errorText;
    
    private IEnumerator SubmitError(string error)
    {
        this.errorText.text = error;
        yield return new WaitForSeconds(5f);
        this.errorText.text = "";
    }

    public void AttemptLogin()
    {
        string username = this.usernameField.text;
        string password = this.passwordField.text;
        
        ServerAccount acc = ServerAccountSystem.instance.GetServerAccount(username, password);
        if (acc == null)
        {
            StartCoroutine(SubmitError("INVALID USERNAME OR PASSWORD."));
            return;
        }
        
        PlayerPrefs.SetString("Account", JsonUtility.ToJson(acc));
        SceneManager.LoadScene(0);
    }
}
