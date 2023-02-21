using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MyLobbyUI : MonoBehaviour
{
    public static MyLobbyUI Instance {get; private set;}
    
    [SerializeField] Canvas createConnectUI;
    [SerializeField] TextMeshProUGUI textCodeRoom;

    void Awake(){
        Instance = this;
    }

    void Start(){
        Hide();
    }

    public void OnClickBackButton(){
        createConnectUI.GetComponent<CreateConnectUI>().Show();
        MyLobbyManager.Instance.LeaveLobby();
        Hide();
    }

    public void UpdateCodeRoom(string codeRoom){
        textCodeRoom.text = codeRoom;
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
    }
}
