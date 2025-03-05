using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class colorToPrefab {
    public GameObject prefab;
    public Color32 color;
}

public class CoinGenerator : MonoBehaviour {
    public static CoinGenerator _instance;

    public List<Texture2D> coinMaps; 
    [SerializeField] private colorToPrefab[] colortoPrefab;
    [SerializeField] private GameObject parentObj;
    [SerializeField] private float delayCoin;
    [SerializeField] private int poolSize = 100; 

    private Queue<GameObject> coinPool = new Queue<GameObject>(); 
    private int currentMapIndex = 0;

    private void Awake() {
        _instance = this;
        InitializeCoinPool();
    }

    private void InitializeCoinPool() {
        foreach (var obj in colortoPrefab) {
            for (int i = 0; i < poolSize / colortoPrefab.Length; i++) {
                GameObject coin = Instantiate(obj.prefab);
                coin.SetActive(false);
                coinPool.Enqueue(coin);
            }
        }
    }

    public void StartCoinGeneration() {
        StartCoroutine(StartGeneratingCoins());
    }

    private IEnumerator StartGeneratingCoins() {
        while (true) {
            GenerateMap();
            yield return new WaitForSeconds(delayCoin);
            currentMapIndex = (currentMapIndex + 1) % coinMaps.Count; 
        }
    }

    private void GenerateMap() {
        Texture2D coinMap = coinMaps[currentMapIndex];
        Color32[] mapColors = coinMap.GetPixels32();

        for (int x = 0; x < coinMap.width; x++) {
            for (int y = 0; y < coinMap.height; y++) {
                GenerateCoins(x, y, mapColors[x + y * coinMap.width]);
            }
        }
    }

    private bool ColorsAreEqual(Color32 a, Color32 b, byte tolerance = 25) {
        return Mathf.Abs(a.r - b.r) < tolerance &&
               Mathf.Abs(a.g - b.g) < tolerance &&
               Mathf.Abs(a.b - b.b) < tolerance &&
               Mathf.Abs(a.a - b.a) < tolerance;
    }

    private void GenerateCoins(int x, int y, Color32 mapColor) {
        foreach (colorToPrefab obj in colortoPrefab) {
            if (ColorsAreEqual(obj.color, mapColor)) {
                Vector3 pos = new Vector3(x + parentObj.transform.position.x,
                                          y + parentObj.transform.position.y,
                                          parentObj.transform.position.z);

                GameObject coin = GetCoinFromPool();
                if (coin != null) {
                    coin.transform.position = pos;
                    coin.SetActive(true);
                }
            }
        }
    }

    private GameObject GetCoinFromPool() {
        if (coinPool.Count > 0) {
            return coinPool.Dequeue();
        }
        return null; 
    }

    public void ReturnCoinToPool(GameObject coin) {
        coin.SetActive(false);
        coinPool.Enqueue(coin);
    }
}
