using System;
using UnityEngine;

[System.Serializable]
public class colorToPrefab {
    public GameObject prefab;
    public Color32 color;
}

public class CoinGenerator : MonoBehaviour {
    [SerializeField] private Texture2D coinMap;
    [SerializeField] private colorToPrefab[] colortoPrefab;
    [SerializeField] private GameObject parentObj;

    private Color32[] mapColors;

    private void Start() {
        GenerateMap();
    }

    private void GenerateMap() {
        mapColors = coinMap.GetPixels32();
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
                Vector3 pos = new Vector3(x, y, parentObj.transform.position.z);
                Instantiate(obj.prefab, pos, Quaternion.identity, parentObj.transform);
            }
        }
    }
}
