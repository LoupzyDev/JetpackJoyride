using UnityEngine;

public class CharacterController : MonoBehaviour {
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float gravityScale = 9.81f;
    [SerializeField] private float horizontalSpeed = 5f;
    [SerializeField] private float jumpSpeed = 10f;
    private bool isPress;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; 
    }

    private void Update() {
        rb.linearVelocity = new Vector3(horizontalSpeed, rb.linearVelocity.y, 0);
    }

    private void FixedUpdate() {
        isPress = Input.GetMouseButton(0);

        if (!isPress) {
            rb.linearVelocity += Vector3.down * gravityScale * Time.fixedDeltaTime;
        } else {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpSpeed, 0);
        }
    }
}
