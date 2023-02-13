using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status {get; private set;}

    private NetworkService _network;

    public void Startup(NetworkService service) {
        Debug.Log("audio manager starting...");

        _network = servbice;

        status = ManagerStatus.Started;
    }
}
