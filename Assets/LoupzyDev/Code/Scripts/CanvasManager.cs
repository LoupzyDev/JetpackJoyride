using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager _instance;
    [SerializeField] GameObject planeDraw;
    [SerializeField] GameObject panelButtons;


    private void Awake() {
        _instance = this;
        planeDraw.SetActive(true);
        panelButtons.SetActive(true);
    }

    public void StartGameplay() {
        planeDraw.SetActive(false);
        panelButtons.SetActive(false);
        CoinGenerator._instance.StartCoinGeneration();
    }

}
