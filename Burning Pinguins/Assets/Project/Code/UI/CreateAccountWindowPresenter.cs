using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class CreateAccountWindowPresenter : MonoBehaviour, IUiWindow
{
    [SerializeField] private Button _createAccountButton;
    [SerializeField] private Button _backToMenuButton;

    [SerializeField] private TMP_Text _usernameInputField;
    [SerializeField] private TMP_Text _passwordInputField;
    [SerializeField] private TMP_Text _emailInputField;

    public static Canvas Canvas { get; private set; }

    private void OnEnable()
    {
        Canvas = GetComponent<Canvas>();
        _createAccountButton.onClick.AddListener(CreateAccount);
        _backToMenuButton.onClick.AddListener(SwitchToMainMenu);
    }

    private void OnDisable()
    {
        Canvas = null;
        _createAccountButton.onClick.RemoveListener(CreateAccount);
    }

    private void CreateAccount()
    {
        MainMenuEntryPoint.PlayFabService.CreatePlayFabAccount(_usernameInputField.text, _emailInputField.text, _passwordInputField.text);
        SwitchToMainMenu();
        MainMenuEntryPoint.PlayFabService.ConnectViaPlayFab(_usernameInputField.text, _passwordInputField.text);
    }

    private void SwitchToMainMenu()
    {
        Canvas.enabled = false;
        MainMenuPresenter.Canvas.enabled = true;
    }
}
