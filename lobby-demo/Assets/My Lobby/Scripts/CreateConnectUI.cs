using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreateConnectUI : MonoBehaviour
{
    [SerializeField] Canvas mainMenuUI;
    [SerializeField] Canvas LobbyUI;
    
    [SerializeField] private TextMeshProUGUI idPlayerText;
    [SerializeField] private TMP_InputField inputPlayerName;
    [SerializeField] private TMP_InputField inputLobbyCode;
    


    void Start(){
        Hide();
        inputPlayerName.text = MyLobbyManager.Instance.GetPlayerName();
        idPlayerText.text = MyLobbyManager.Instance.GetPlayerID();
    }

    public void OnClickCreateButton(){
        MyLobbyManager.Instance.UpdatePlayerName(inputPlayerName.text);
        MyLobbyManager.Instance.CreateLobby();

    }

    public void OnClickConnectButton(){
        MyLobbyManager.Instance.ConnectLobby(inputLobbyCode.text);
    }

    public void OnClickBackButton(){
        Hide();
        mainMenuUI.GetComponent<MainMenuUI>().Show();
    }

    public void Hide(){
        gameObject.SetActive(false);
    }

    public void Show(){
        gameObject.SetActive(true);
    }
}
