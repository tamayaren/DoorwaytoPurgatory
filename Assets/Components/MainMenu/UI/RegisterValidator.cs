using System;
using System.Collections;
using System.Net.Mail;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using static System.Net.Mail.MailAddress;

namespace AccountNetwork
{
    public class RegisterValidator : MonoBehaviour
    {
        public static Regex passwordRegex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*)(?=.*[^a-zA-Z]).{8,15}$");
        public static Regex usernameRegex = new Regex("^[a-zA-Z0-9]+$");
        
        [SerializeField] private MenuButtons menuButtons;
        [SerializeField] private TMP_InputField usernameInput;
        [SerializeField] private TMP_InputField passwordInput;
        [SerializeField] private TMP_InputField confirmPasswordInput;
        [SerializeField] private TMP_InputField emailInput;

        [SerializeField] private TextMeshProUGUI errorText;

        private IEnumerator SubmitError(string error)
        {
            this.errorText.text = error;
            yield return new WaitForSeconds(5f);
            this.errorText.text = "";
        }
        
        public void SubmitAccount()
        {
            string username = this.usernameInput.text;
            string password = this.passwordInput.text;
            string confirmPassword = this.confirmPasswordInput.text;
            string email = this.emailInput.text;

            if (!Regex.IsMatch(password, confirmPassword))
            {
                StartCoroutine(SubmitError("PASSWORDS ARE NOT THE SAME."));
                return;
            }
            
            RegistryValidity passed = ValidateRegistry(username, password, email);

            string errorCode = "";
            switch (passed)
            {
                case RegistryValidity.InvalidEmail:
                    errorCode = "EMAIL ADDRESS INVALID.";
                    break;
                case RegistryValidity.InvalidPassword:
                    errorCode = "PASSWORD REQUIRES MINIMUM 8 CHARACTERS, 1 UPPER CHARACTER, 1 SPECIAL CHARACTER.";
                    break;
                case RegistryValidity.InvalidUsername:
                    errorCode = "USERNAME ONLY ACCEPTS ALPHANUMERIC CHARACTERS..";
                    break;
                case RegistryValidity.UsernameTaken:
                    errorCode = "USERNAME IS TAKEN.";
                    break;

                default:
                    ServerAccountSystem.instance.Register(username, password, email);
                    this.menuButtons.LoginButtonMenu();
                    break;
            }

            if (errorCode != "")
            {
                StartCoroutine(SubmitError(errorCode));
                return;
            }
            
        }
        
        private static bool EmailValidity(string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);
                
                return true;
            }
            catch (FormatException exception)
            {
                return false;
            }
        }
        
        public static RegistryValidity ValidateRegistry(string name, string password, string email)
        {
            if (!usernameRegex.IsMatch(name))
                return RegistryValidity.InvalidUsername;
            
            
            if (!passwordRegex.IsMatch(password))
                return RegistryValidity.InvalidPassword;

            if (!EmailValidity(email))
                return RegistryValidity.InvalidEmail;
            
            if (ServerAccountSystem.instance.AccountNameExistence(name))
                return RegistryValidity.UsernameTaken;
            
            return RegistryValidity.Success;
        }
        
    }
    
    public enum RegistryValidity
    {
        Success,
        InvalidPassword,
        InvalidUsername,
        InvalidEmail,
        UsernameTaken,
    }
    
}
