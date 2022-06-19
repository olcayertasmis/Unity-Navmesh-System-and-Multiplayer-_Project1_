using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;

public class UserAccountManager : MonoBehaviour
{

    public static UserAccountManager Instance;

    void Awake()
    {
        Instance = this;
    }
    

    public void CreateAccount(string username, string emailAdress, string password)
    {
        PlayFabClientAPI.RegisterPlayFabUser(
            new RegisterPlayFabUserRequest()
            {
                Email = emailAdress,
                Password = password,
                Username = username,
                RequireBothUsernameAndEmail = true
            },
            response =>
            {
                Debug.Log($"Successfull Account Creation: {username}, {emailAdress}");
            },
            error =>
            {
                Debug.Log($"Unsuccessfull Account Creation: {username}, {emailAdress} \n {error.ErrorMessage}");
            }
        );
    }
}
