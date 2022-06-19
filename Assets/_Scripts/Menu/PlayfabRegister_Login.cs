using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabRegister_Login : MonoBehaviour
{
    public InputField mail;
    public InputField pass;


    public InputField Registermail;
    public InputField Registerusername;
    public InputField Registerpass;

    GameObject Login_Status;
    public GameObject LoginStatusPrefab;
    public GameObject LoginErrorPrefab;


    public void Login()
    {
        var requestLogin = new LoginWithEmailAddressRequest { Email = mail.text.ToString(), Password = pass.text.ToString() };
        PlayFabClientAPI.LoginWithEmailAddress(requestLogin, OnLoginSuccess, OnLoginFailure);

        Login_Status = Instantiate(LoginStatusPrefab, transform.position, transform.rotation);
        Login_Status.transform.GetChild(1).GetComponent<Text>().text = "logging in...";
    }
    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Login Success");
        SceneManager.LoadScene(2);
    }

    private void OnLoginFailure(PlayFabError error)
    {
        mail.text = "";
        pass.text = "";
        Debug.LogError("Error information:");
        Debug.LogError(error.GenerateErrorReport());
        Destroy(Login_Status);
        Login_Status = Instantiate(LoginErrorPrefab, transform.position, transform.rotation);
        string oldText = error.GenerateErrorReport();
        int index = oldText.IndexOf("\n");
        string newText = oldText.Substring(index + 1);
        Login_Status.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = newText;
    }

    public void Register()
    {
        RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest()
        {
            Email = Registermail.text.ToString(),
            Username = Registerusername.text.ToString(),
            Password = Registerpass.text.ToString(),
            TitleId = PlayFabSettings.TitleId,
            RequireBothUsernameAndEmail = true
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSucees, OnRegisterFailed);
    }
    private void OnRegisterSucees(RegisterPlayFabUserResult result)
    {
        Debug.Log("Register Success");
        Login_Status = Instantiate(LoginErrorPrefab, transform.position, transform.rotation);
        Login_Status.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = "Registration Successful!";
        Registermail.text = "";
        Registerusername.text = "";
        Registerpass.text = "";
    }
    private void OnRegisterFailed(PlayFabError error)
    {
        Registermail.text = "";
        Registerusername.text = "";
        Registerpass.text = "";
        Debug.LogError("Error information:");
        Debug.LogError(error.GenerateErrorReport());

        Login_Status = Instantiate(LoginErrorPrefab, transform.position, transform.rotation);
        string oldText = error.GenerateErrorReport();
        int index = oldText.IndexOf("\n");
        string newText = oldText.Substring(index + 1);
        Login_Status.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = newText;
    }
}
