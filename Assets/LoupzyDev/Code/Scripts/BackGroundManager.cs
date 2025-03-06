using UnityEngine;

public class BackGroundManager : MonoBehaviour {
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float width;

    void Update() {

        float leftBoundary = GetCameraLeftBoundary();

        Debug.Log("Posición del fondo: " + transform.position.x);
        Debug.Log("Límite izquierdo de la cámara: " + leftBoundary);
        if (transform.position.x + width / 2 < leftBoundary ) {
            Reposition();
        }
    }

    private float GetCameraLeftBoundary() {

        float distanceToBackground = Mathf.Abs(transform.position.z - mainCamera.transform.position.z);
        float halfWidth = Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad) * distanceToBackground * mainCamera.aspect;
        return mainCamera.transform.position.x - halfWidth;
    }

    public void Reposition() {
        GameObject[] backgrounds = GameObject.FindGameObjectsWithTag("Background");
        float maxX = float.MinValue;

        foreach (GameObject bg in backgrounds) {
            if (bg.transform.position.x > maxX) {
                maxX = bg.transform.position.x;
            }
        }
        transform.position = new Vector3(maxX + width, transform.position.y, transform.position.z);
    }
}
