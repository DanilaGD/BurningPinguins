using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabService : MonoBehaviour
{
    private PlayerAccountData _loggedAccountData = new();

    public string Username { get => _loggedAccountData.AccountName; private set => _loggedAccountData.AccountName = value; }
    public string Email { get => _loggedAccountData.AccountEmail; private set => _loggedAccountData.AccountEmail = value; }
    public string Password { get => _loggedAccountData.AccountPassword; private set => _loggedAccountData.AccountPassword = value; }
    public PlayerAccountData LoggedAccountData { get => _loggedAccountData; private set => _loggedAccountData = value; }
    public bool AccountCreated { get; set; } = false;
    public string AccountCreationMessage { get; set; }

    public static PlayFabService Instance { get; private set; }

    public void OnEnable()
    {
        Instance = this;
    }

    public void OnDisable()
    {
        Instance = null;
    }

    public void CreatePlayFabAccount(string username, string email, string password)
    {
        PlayFabClientAPI.RegisterPlayFabUser(new()
        {
            Username = username,
            Email = email,
            Password = password,
            RequireBothUsernameAndEmail = true,
        }, result =>
        {
            AccountCreated = true;
            AccountCreationMessage = result.ToString();
        }, error =>
        {
            AccountCreated = false;
            AccountCreationMessage = error.ErrorMessage;
            Debug.LogError($"Fail: {error.ErrorMessage}");
        });
    }

    public void ConnectViaPlayFab(string username, string password)
    {
        if (PlayFabClientAPI.IsClientLoggedIn()) return;

        var request = new LoginWithPlayFabRequest
        {
            Username = username,
            Password = password,
        };

        PlayFabClientAPI.LoginWithPlayFab(
            request,
            result =>
            {
                _loggedAccountData.AccountName = username;
                _loggedAccountData.AccountPassword = password;
            },
            error =>
            {
                Debug.LogError($"Fail: {error.ErrorMessage}");
            });
    }
}
