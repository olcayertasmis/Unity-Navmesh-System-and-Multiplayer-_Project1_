using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICreateAccount : MonoBehaviour
{

    string username, password, emailAdress;

    public void UpdateUsarname(string _username)
    {
        username = _username;
    }
    public void UpdatePassword(string _password)
    {
        password = _password;
    }
    public void UpdateEmailAddress(string _emailAdress)
    {
        emailAdress = _emailAdress;
    }

    public void CreateAccount()
    {
        UserAccountManager.Instance.CreateAccount(username, emailAdress, password);
    }
}
