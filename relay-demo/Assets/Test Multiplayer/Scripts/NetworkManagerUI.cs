using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button buttonHost;
    [SerializeField] private Button buttonClient;
    [SerializeField] private Button buttonServer;
    [SerializeField] private TMP_InputField joinCode;

    [SerializeField] private TestRelay testRelay;

    void Awake(){
        buttonHost.onClick.AddListener(() => {
            testRelay.CreateRelay();
        });

        buttonClient.onClick.AddListener(() => {
            testRelay.JoinRelay(joinCode.text);
        });

        buttonServer.onClick.AddListener(() => {
            NetworkManager.Singleton.StartServer();
        });
    }
}
