using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private float _offsetX;
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime = 0.3f;
    private float _currentVelocityX = 0f;

    private void Awake() {
        _offsetX = transform.position.x - target.position.x;
    }

    private void LateUpdate() {

        float targetX = target.position.x + _offsetX;
        float newX = Mathf.SmoothDamp(transform.position.x, targetX, ref _currentVelocityX, smoothTime);

        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
