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
        // Obtener el collider del objeto
        objectCollider = GetComponent<Collider>();

        // Crear la textura
        texture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Repeat;
        ClearTexture();
        ApplyTextureToMaterial();

        drawAction = playerInput.actions["Draw"];
        eraseAction = playerInput.actions["Erase"];
        saveAction = playerInput.actions["Save"];

        // Suscribirse a los eventos de la acción de borrar
        eraseAction.performed += ctx => ToggleErase();
    }

    private void OnEnable() {
        // Activar las acciones de entrada
        drawAction.Enable();
        eraseAction.Enable();
        saveAction.Enable();
    }

    private void OnDisable() {
        // Desactivar las acciones de entrada
        drawAction.Disable();
        eraseAction.Disable();
        saveAction.Disable();
    }

    private void Update() {
        // Detectar entrada de dibujo (mouse o touch)
        if (drawAction.IsPressed()) {
            DrawOrErase();
        }

        // Guardar la textura con la tecla "M"
        if (saveAction.IsPressed()) {
            SaveTexture("Assets/LoupzyDev/Code/texture.png");
        }
    }

    private void ToggleErase() {
        // Alternar entre borrar y dibujar al presionar la tecla
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
        texture.SetPixel(x, y, new Color32(255, 255, 255, 0)); // Pixel transparente para borrar
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

    private void SaveTexture(string path) {
        byte[] textureBytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(path, textureBytes);
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
        Debug.Log("Texture saved to: " + path);
    }
}
