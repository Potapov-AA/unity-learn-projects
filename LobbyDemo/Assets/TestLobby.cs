using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using TMPro;

public class TestLobby : MonoBehaviour
{
    private Lobby hostLobby;
    private Lobby joinedLobby;
    private float heartbeatTimer;
    private float lobbyUpdateTimer;
    private string playerName;

    private async void Start(){
        

        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        playerName = "JESTER" + Random.Range(10, 99);
        Debug.Log(playerName);
    }

    private void Update(){
        HandleLobbyHeartbeat();
        HandleLobbyPollForUpdates();
    }

    private async void HandleLobbyHeartbeat(){
        if(hostLobby != null){
            heartbeatTimer -= Time.deltaTime;
            if(heartbeatTimer < 0){
                float heartbeatTimerMax = 15;
                heartbeatTimer = heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }

    private async void HandleLobbyPollForUpdates(){
        if(joinedLobby != null){
            lobbyUpdateTimer -= Time.deltaTime;
            if(lobbyUpdateTimer < 0){
                float lobbyUpdateTimerMax = 1.1f;
                lobbyUpdateTimer = lobbyUpdateTimerMax;

                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                joinedLobby = lobby;
            }
        }
    }

    public async void CreateLobby(){
        try {
            string lobbyName = "my lobby";
            int maxPlayers = 4;

            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions {
                IsPrivate = true,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>{
                    { "GameMode", new DataObject(DataObject.VisibilityOptions.Public, "CaptureTheFlag")},
                    { "Map", new DataObject(DataObject.VisibilityOptions.Public, "de_dust2")}
                }
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);
            
            hostLobby = lobby;
            joinedLobby = hostLobby;

            Debug.Log("Created lobby! " + lobbyName + " " + maxPlayers + " " + lobby.Id + " " + lobby.LobbyCode);

            PrintPlayers(hostLobby);
            UpdateCode(hostLobby.LobbyCode);
        } catch(LobbyServiceException e){
            Debug.Log(e);
        }
    }

    public async void ListLobbies(){
        try {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions{
                Count = 25,
                Filters = new List<QueryFilter>{
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT) 
                },
                Order = new List<QueryOrder>{
                    new QueryOrder(false, QueryOrder.FieldOptions.Created)
                }
            };

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);

            Debug.Log("Lobbies: " + queryResponse.Results.Count);
            foreach(Lobby lobby in queryResponse.Results){
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
            }
        } catch (LobbyServiceException e){
            Debug.Log(e);
        }
    }

    public async void JoinLobby(){
        try{
            JoinLobbyByIdOptions joinLobbyByIdOptions = new JoinLobbyByIdOptions{
                Player = GetPlayer()
            };

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            Debug.Log("Lobbies: " + queryResponse.Results.Count);
            foreach(Lobby lobby in queryResponse.Results){
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Data["GameMode"].Value + " " + lobby.Data["Map"].Value);
            }
            Lobby lobby = await Lobbies.Instance.JoinLobbyByIdAsync(queryResponse.Results[0].Id, joinLobbyByIdOptions);

            joinedLobby = lobby;

        } catch (LobbyServiceException e){
            Debug.Log(e);
        }
        
    }

    [SerializeField] string lobbyCode;
    [SerializeField] TextMeshProUGUI codeText;

    public async void JoinPrivateLobby(){
        try{
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions{
                Player = GetPlayer()
            };
            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);

            joinedLobby = lobby;

            Debug.Log("Joined lobby with code " + lobbyCode);
            
            PrintPlayers(lobby);
            
        } catch (LobbyServiceException e){
            Debug.Log(e);
        }
    }

    private void UpdateCode(string code){
        codeText.text = code;
    }

    private void PrintPlayers(){
        PrintPlayers(joinedLobby);
    }
    private void PrintPlayers(Lobby lobby){
        Debug.Log("Players in lobby: " + lobby.Name + " " + lobby.Data["GameMode"].Value + " " + lobby.Data["Map"].Value);
        foreach(Player player in lobby.Players){
            Debug.Log(player.Id + " " + player.Data["PlayerName"].Value);
        }
    }

    private Player GetPlayer(){
        return new Player {
            Data = new Dictionary<string, PlayerDataObject>{
                {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName)}
            }
        };
    }

    [SerializeField] TMP_InputField inputField;
    public async void UpdateLobbyGameMode(){
        try{
            string gameMode = inputField.text;

            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions { 
                Data = new Dictionary<string, DataObject>{
                    { "GameMode", new DataObject(DataObject.VisibilityOptions.Public, gameMode)} 
                }
            });
            joinedLobby = hostLobby;

            PrintPlayers(hostLobby);
        } catch (LobbyServiceException e){
            Debug.Log(e);
        }
    }

    private async void UpdatePlayerName(string newPlayerName){
        try{
            playerName = newPlayerName;
            await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions {
                Data = new Dictionary<string, PlayerDataObject>{
                    {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName)}
                }
            });
        } catch (LobbyServiceException e){
            Debug.Log(e);
        }
    }

    private async void LeaveLobbt(){
        try{
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
        } catch (LobbyServiceException e){
            Debug.Log(e);
        }
    }

    private async void KickPlayer(){
        try{
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, joinedLobby.Players[1].Id);
        } catch (LobbyServiceException e){
            Debug.Log(e);
        }
    }

    private async void MigrateLobbyHost(){
        try{
            string gameMode = inputField.text;

            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions { 
                HostId = joinedLobby.Players[1].Id
            });
            joinedLobby = hostLobby;

            PrintPlayers(hostLobby);
        } catch (LobbyServiceException e){
            Debug.Log(e);
        }
    }

    private async void DeleteLobby(){
        try{
            await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
        } catch (LobbyServiceException e){
            Debug.Log(e);
        }
    }
}
