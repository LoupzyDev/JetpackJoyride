using UnityEngine;
using UnityEngine.InputSystem;

public class RacoonController : MonoBehaviour {
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float gravityScale = 9.81f;
    [SerializeField] private float horizontalSpeed = 5f;
    [SerializeField] private float jumpSpeed = 10f;
    private bool isPress;
    [SerializeField] private PlayerInput playerInput;
    private InputAction jumpAction;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        jumpAction = playerInput.actions["RacoonJump"];
    }

    private void Update() {

        rb.linearVelocity = new Vector3(horizontalSpeed, rb.linearVelocity.y, 0);
    }

    private void FixedUpdate() {

        isPress = jumpAction.ReadValue<float>() > 0.5f; 

        if (!isPress) {
            rb.linearVelocity += Vector3.down * gravityScale * Time.fixedDeltaTime;
        } else {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpSpeed, 0);
        }
    }
}
