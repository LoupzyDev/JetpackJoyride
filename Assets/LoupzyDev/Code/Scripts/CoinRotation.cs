using UnityEngine;

public class CoinRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f;
    void Update() {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") || other.CompareTag("Wall")) {
            CoinGenerator._instance.ReturnCoinToPool(gameObject);
        }
    }


}
