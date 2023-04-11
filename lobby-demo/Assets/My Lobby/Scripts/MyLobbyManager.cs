using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using UnityEngine;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

public class MyLobbyManager : MonoBehaviour
{
    


    public static MyLobbyManager Instance {get; private set;}

    public const string KEY_PLAYER_NAME = "PlayerName";

    private Lobby joinedLobby;

    private string playerName = "PlayerName";
    private string lobbyName = "Lobby";

    private float heartbeatTimer = 15f;
    private float lobbyPollTimer = 1.1f;


    void Awake(){
        Instance = this;
        Authenticate();
    }

    void Update(){
        HandleLobbyHeartbeat();
        HandleLobbyPolling();
    }

    public async void Authenticate(){
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => { };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateLobby(){
        try{
            Player player = GetPlayer();

            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions{
                Player = player,
                IsPrivate = true,
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, 4, createLobbyOptions);

            joinedLobby = lobby;

            MyLobbyUI.Instance.UpdateLobby(lobby);
            MyLobbyUI.Instance.UpdateCodeRoom(lobby.LobbyCode);
            MyLobbyUI.Instance.UpdateLobbyName(lobby.Name);
            MyLobbyUI.Instance.Show();
        } catch (LobbyServiceException e){
            Debug.Log(e);
        }
    }

    public async void ConnectLobby(string lobbyCode){
        try{
            Player player = GetPlayer();

            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions{
                Player = player,
            };

            Lobby lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);

            joinedLobby = lobby;

            MyLobbyUI.Instance.UpdateLobby(lobby);
            MyLobbyUI.Instance.UpdateCodeRoom(lobby.LobbyCode);
            MyLobbyUI.Instance.UpdateLobbyName(lobby.Name);
            MyLobbyUI.Instance.Show();
        } catch (LobbyServiceException e){
            Debug.Log(e);
        }
    }

    public async void LeaveLobby(){
        try{
            await LobbyService.Instance.QueryLobbiesAsync();
            MyLobbyUI.Instance.Hide();

            joinedLobby = null;
        } catch (LobbyServiceException e){
            Debug.Log(e);
        }
    }

    public async void KickPlayer(string playerID){
        if(IsLobbyHost()){
            try{
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerID);
            } catch (LobbyServiceException e){
                Debug.Log(e);
            }
        }
    }

    private async void HandleLobbyHeartbeat(){
        if (IsLobbyHost()){
            heartbeatTimer -= Time.deltaTime;
            if(heartbeatTimer < 0f){
                float heartbeatTimerMax = 15f;
                heartbeatTimer = heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
            }
        }
    }

    private async void HandleLobbyPolling(){
        if(joinedLobby != null){
            lobbyPollTimer -= Time.deltaTime;
            if(lobbyPollTimer < 0f){
                float lobbyPollTimerMax = 5f;
                lobbyPollTimer = lobbyPollTimerMax;
                try{
                    joinedLobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                    MyLobbyUI.Instance.UpdateLobby(joinedLobby);
                } catch (LobbyServiceException e){
                    Debug.Log(e);
                    joinedLobby = null;
                    MyLobbyUI.Instance.Hide();
                }
            }
        }
    }

    public bool IsLobbyHost(){ return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId; }

    public bool IsPlayerInLobby() {
        if (joinedLobby != null && joinedLobby.Players != null) {
            foreach (Player player in joinedLobby.Players) {
                if (player.Id == AuthenticationService.Instance.PlayerId) {
                    return true;
                }
            }
        }
        return false;
    }

    public Player GetPlayer(){
        return new Player(AuthenticationService.Instance.PlayerId, null, new Dictionary<string, PlayerDataObject>{
            {KEY_PLAYER_NAME, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerName)}
        });
    }
    public void UpdatePlayerName(string playerName){ this.playerName = playerName; }
    public string GetPlayerName(){ return this.playerName; }
    public string GetPlayerID(){ return AuthenticationService.Instance.PlayerId; }
    public string GetCodeRoom() { return joinedLobby.LobbyCode; }
}
