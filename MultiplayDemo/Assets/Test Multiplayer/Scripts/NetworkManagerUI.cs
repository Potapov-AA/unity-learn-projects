using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button buttonHost;
    [SerializeField] private Button buttonClient;
    [SerializeField] private Button buttonServer;

    void Awake(){
        buttonHost.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
        });

        buttonClient.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
        });

        buttonServer.onClick.AddListener(() => {
            NetworkManager.Singleton.StartServer();
        });
    }
}
