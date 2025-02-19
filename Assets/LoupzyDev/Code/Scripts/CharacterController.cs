using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float gravityScale;
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private bool isPress;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    private void Update()
    {
        rb.linearVelocity = new Vector3(horizontalSpeed, rb.linearVelocity.y, rb.linearVelocity.z);
        
    }
    private void FixedUpdate()
    {
        isPress = Input.GetMouseButton(0);
        if(!isPress){
            rb.linearVelocity += Vector3.down * gravityScale * Time.fixedDeltaTime;
        }
        else{
            rb.linearVelocity = new Vector3 (rb.linearVelocity.x, jumpSpeed, rb.linearVelocity.z);
        }
    }

}
