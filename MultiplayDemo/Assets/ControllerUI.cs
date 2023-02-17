using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class ControllerUI : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button cleintButton;
    [SerializeField] private Button serverButton;

    void Awake(){
        hostButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
        });

        cleintButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
        });

        serverButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartServer();
        });  
    }
}
