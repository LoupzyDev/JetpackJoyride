using UnityEngine;
using UnityEngine.InputSystem;

public class PixelArtDrawer : MonoBehaviour {
    public Material targetMaterial;
    public Color32 drawColor = Color.white;
    public int textureWidth = 64;
    public int textureHeight = 64;
    private Texture2D texture;
    private Collider objectCollider;
    private bool isErasing = false;
    [SerializeField] private PlayerInput playerInput;
    private InputAction drawAction;
    private InputAction eraseAction;
    private InputAction saveAction;

    private void Awake() {

        objectCollider = GetComponent<Collider>();

        texture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Repeat;
        ClearTexture();
        ApplyTextureToMaterial();

        drawAction = playerInput.actions["Draw"];
        eraseAction = playerInput.actions["Erase"];
        saveAction = playerInput.actions["Save"];

        eraseAction.performed += ctx => ToggleErase();
    }

    private void OnEnable() {

        drawAction.Enable();
        eraseAction.Enable();
        saveAction.Enable();
    }

    private void OnDisable() {

        drawAction.Disable();
        eraseAction.Disable();
        saveAction.Disable();
    }

    private void Update() {

        if (drawAction.IsPressed()) {
            DrawOrErase();
        }


        if (saveAction.IsPressed()) {
            SaveTextureButton();
        }
    }

    public void SaveTextureButton() {
        SaveTexture();
    }

    public void ToggleErase() {
        isErasing = !isErasing;
        Debug.Log(isErasing ? "Borrando" : "Dibujando");
    }

    private void DrawOrErase() {
        Ray ray = Camera.main.ScreenPointToRay(Pointer.current.position.ReadValue());
        if (objectCollider.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) {
            int x = Mathf.FloorToInt(hit.textureCoord.x * texture.width);
            int y = Mathf.FloorToInt(hit.textureCoord.y * texture.height);
            x = Mathf.Clamp(x, 0, texture.width - 1);
            y = Mathf.Clamp(y, 0, texture.height - 1);

            if (isErasing) {
                ErasePixel(x, y);
            } else {
                DrawPixel(x, y);
            }
        }
    }

    private void DrawPixel(int x, int y) {
        texture.SetPixel(x, y, drawColor);
        texture.Apply();
        ApplyTextureToMaterial();
    }

    private void ErasePixel(int x, int y) {
        texture.SetPixel(x, y, new Color32(255, 255, 255, 0));
        texture.Apply();
        ApplyTextureToMaterial();
    }

    private void ClearTexture() {
        for (int x = 0; x < texture.width; x++) {
            for (int y = 0; y < texture.height; y++) {
                texture.SetPixel(x, y, new Color32(255, 255, 255, 0));
            }
        }
        texture.Apply();
        ApplyTextureToMaterial();
    }

    private void ApplyTextureToMaterial() {
        if (targetMaterial != null) {
            targetMaterial.mainTexture = texture;
        }
    }

    private void SaveTexture() {
        byte[] textureBytes = texture.EncodeToPNG();

        // Determinar la ruta según la plataforma
        string filePath = string.Empty;

#if UNITY_ANDROID
        // En Android, utilizamos el almacenamiento persistente
        string directory = Application.persistentDataPath;  // Directorio accesible en Android
        filePath = System.IO.Path.Combine(directory, "texture.png");

#elif UNITY_EDITOR || UNITY_STANDALONE_WIN
    // En Unity Editor o Windows, usamos la ruta dentro del proyecto de Unity
    filePath = "Assets/LoupzyDev/Code/texture.png";
#endif

        // Guardar la textura en la ruta correspondiente
        System.IO.File.WriteAllBytes(filePath, textureBytes);

        // Si estamos en el editor de Unity, actualizamos la base de datos de assets
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif

        Debug.Log("Texture saved to: " + filePath);

        // Cargar la textura guardada en la variable coinMap
        LoadTexture(filePath);
    }

    private void LoadTexture(string path) {
        // Verificar si el archivo existe en la ruta especificada
        if (System.IO.File.Exists(path)) {
            byte[] fileData = System.IO.File.ReadAllBytes(path);
            Texture2D loadedTexture = new Texture2D(2, 2);  // Usamos un tamaño temporal para cargar la textura
            loadedTexture.LoadImage(fileData);  // Cargar la textura desde los bytes
            CoinGenerator._instance.coinMaps[0] = loadedTexture;  // Asignar la textura cargada a coinMap

            Debug.Log("Texture loaded into coinMap: " + path);
        } else {
            Debug.LogError("Texture file not found at: " + path);
        }
    }


}
