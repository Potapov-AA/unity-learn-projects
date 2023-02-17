using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode;

public class PlayerMove : NetworkBehaviour
{
    [SerializeField] float speed = 5f;
    
    private Rigidbody _rigidbody;

    void Start(){
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    void Update(){
        if (!IsLocalPlayer) return;
        
        _rigidbody.velocity = new Vector3(Input.GetAxis("Horizontal") * speed, 0, Input.GetAxis("Vertical") * speed);
    }
}
