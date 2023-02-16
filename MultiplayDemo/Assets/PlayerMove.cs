using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    
    private Rigidbody _rigidbody;

    void Start(){
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    void Update(){
        _rigidbody.velocity = new Vector3(Input.GetAxis("Horizontal") * speed, 0, Input.GetAxis("Vertical") * speed);
    }
}
