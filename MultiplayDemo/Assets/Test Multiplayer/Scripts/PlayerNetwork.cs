using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private Transform spawnedObjectPrefab;

    private Transform spawnedObjectTransform;

    // Синхронизация переменых
    // private NetworkVariable<int> randomNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(
        new MyCustomData {
            _int = 10,
            _bool = true,
        }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner); //Все сетевые переменные должны быть инициализированы
    
    public struct MyCustomData : INetworkSerializable{
        public int _int;
        public bool _bool;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
        }
    } 

    public override void OnNetworkSpawn(){
        randomNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) => {
            Debug.Log(OwnerClientId + "; " + newValue._int + " " + newValue._bool);
        };
        
    }

    void Start(){
        transform.position = new Vector3(0, -10, 0);
    }

    void Update()
    {
        if(!IsOwner) return;

        if(Input.GetKeyDown(KeyCode.T)) {
            spawnedObjectTransform = Instantiate(spawnedObjectPrefab);
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
            // TestClientRpc(); // Можно вызывать только с сервера
            // TestServerRpc(); // Можно вызывать с клиента, но обработка происходит на сервере

            /*
            randomNumber.Value = new MyCustomData {
                _int = Random.Range(0, 100),
                _bool = false,
            };
            */
        }

        if(Input.GetKeyDown(KeyCode.Y)){
            Destroy(spawnedObjectTransform.gameObject);
        }

        Vector3 moveDir = new Vector3(0, 0, 0);

        if(Input.GetKey(KeyCode.W)) moveDir.z += 1f;
        if(Input.GetKey(KeyCode.S)) moveDir.z -= 1f;
        if(Input.GetKey(KeyCode.A)) moveDir.x -= 1f;
        if(Input.GetKey(KeyCode.D)) moveDir.x += 1f;

        float moveSpeed = 10f;

        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    // Еще один способ синхронизации когда мы не мождем доверять клиенту
    [ServerRpc]
    private void TestServerRpc(){
        Debug.Log("TestServerRpc " + OwnerClientId);
    }

    [ClientRpc]
    private void TestClientRpc(){
        Debug.Log("Test client prc");
    }
}
