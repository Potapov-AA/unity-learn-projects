using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Lobbies.Models;
using Unity.Services.Authentication;

public class MyLobbyUI : MonoBehaviour
{
    public static MyLobbyUI Instance {get; private set;}
    
    [SerializeField] Canvas createConnectUI;
    [SerializeField] TextMeshProUGUI textCodeRoom;
    [SerializeField] TextMeshProUGUI textLobbyName;
    [SerializeField] TextMeshProUGUI textPlayerCount;
    [SerializeField] Transform container;
    [SerializeField] Transform playerSingleTemplate;

    void Awake(){
        Instance = this;
    }

    void Start(){
        Hide();
    }
    
    public void UpdateLobby(Lobby lobby){
        ClearLobby();

        foreach (Player player in lobby.Players) {
            Transform playerSingleTransform = Instantiate(playerSingleTemplate, container);
            playerSingleTransform.gameObject.SetActive(true);

            MyPlayerSingleUI lobbyPlayerSingleUI = playerSingleTransform.GetComponent<MyPlayerSingleUI>();
            lobbyPlayerSingleUI.SetKickPlayerButtonVisible(
                MyLobbyManager.Instance.IsLobbyHost() &&
                player.Id != AuthenticationService.Instance.PlayerId // Don't allow kick self
            );

            lobbyPlayerSingleUI.UpdatePlayer(player);
        }

        
        textPlayerCount.text = lobby.Players.Count + "/" + lobby.MaxPlayers;
    }

    public void OnClickBackButton(){
        createConnectUI.GetComponent<CreateConnectUI>().Show();
        MyLobbyManager.Instance.LeaveLobby();
    }

    public void UpdateCodeRoom(string codeRoom){
        textCodeRoom.text = codeRoom;
    }

    public void UpdateLobbyName(string lobbyName){
        textLobbyName.text = lobbyName;
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    private void ClearLobby() {
        foreach (Transform child in container) {
            if (child == playerSingleTemplate) continue;
            Destroy(child.gameObject);
        }
    }
}
