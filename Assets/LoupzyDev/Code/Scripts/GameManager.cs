using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject playerRacoon;
    private void Start() {
        mainCamera.GetComponent<CameraFollow>().enabled = false;
        playerRacoon.GetComponent<RacoonController>().enabled = false;
    }
    
    public void StartGame() {
        mainCamera.GetComponent<CameraFollow>().enabled = true;
        playerRacoon.GetComponent<RacoonController>().enabled = true;
    }
}
