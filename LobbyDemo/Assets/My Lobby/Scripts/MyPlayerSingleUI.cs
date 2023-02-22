using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class MyPlayerSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private Button kickPlayerButton;

    private Player player;

    public void OnClickKickButton(){
        if(player != null){
            MyLobbyManager.Instance.KickPlayer(player.Id);
        }
    }
    
    public void UpdatePlayer(Player player) {
        this.player = player;
        playerNameText.text = player.Data[MyLobbyManager.KEY_PLAYER_NAME].Value;
    }

    public void SetKickPlayerButtonVisible(bool visible) {
        kickPlayerButton.gameObject.SetActive(visible);
    }
}
