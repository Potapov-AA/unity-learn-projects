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

    void Awake(){
        Instance = this;
        Authenticate();
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

            MyLobbyUI.Instance.UpdateCodeRoom(lobby.LobbyCode);
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

            MyLobbyUI.Instance.UpdateCodeRoom(lobby.LobbyCode);
            MyLobbyUI.Instance.Show();
        } catch (LobbyServiceException e){
            Debug.Log(e);
        }
    }

    public async void LeaveLobby(){
        try{
            await LobbyService.Instance.QueryLobbiesAsync();
        } catch (LobbyServiceException e){
            Debug.Log(e);
        }
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
