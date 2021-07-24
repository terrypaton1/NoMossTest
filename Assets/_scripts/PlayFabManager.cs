using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PlayFab;
using PlayFab.ClientModels;

using AlCaTrAzzGames.Utilities;

public class PlayFabManager : PersistentSingleton<PlayFabManager>
{
    public bool isLoggedIn;
    public string PlayFabID;

    public delegate void SuccesfulLoginCallback();
    public SuccesfulLoginCallback loginCallback;

    public void LoginWithMobileID(){
        var request = new LoginWithCustomIDRequest { CustomId = SystemInfo.deviceUniqueIdentifier, CreateAccount = true};
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);

        Debug.Log("Attempting login with mobile ID");
    }

    private void OnLoginSuccess(LoginResult result){
        Debug.Log("Successful login");
        Debug.Log(result);
        isLoggedIn = true;
        PlayFabID = result.PlayFabId;

        if(loginCallback != null){
            loginCallback();
        }
    }

    private void OnLoginFailure(PlayFabError result){
        Debug.LogError(result.GenerateErrorReport());
    }

    public void WriteSimpleEvent(string eventName, Dictionary<string, object> eventBody){
        PlayFabClientAPI.WritePlayerEvent(new WriteClientPlayerEventRequest{
            EventName = eventName,
            Body = eventBody 
        }, null, null);
    }

    protected override void OnDestroy(){
        WriteSimpleEvent("player_closed_app", null);
        base.OnDestroy();
    }
}
