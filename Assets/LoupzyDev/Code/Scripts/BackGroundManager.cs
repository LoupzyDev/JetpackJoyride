using UnityEngine;

public class BackGroundManager : MonoBehaviour
{
    [SerializeReference] private Camera mainCamera;
    [SerializeReference] private float offset;
    [SerializeReference] private float width;

    void Update()
    {
            Debug.Log("Mi transform es: "+ (transform.position.x + width));
            Debug.Log("Camara transform es: " + (mainCamera.transform.position.x - mainCamera.orthographicSize * 2));

        if (transform.position.x + width / 2 < mainCamera.transform.position.x - mainCamera.orthographicSize * 2)
        {
            Reposition();
        }
    }

    public void Reposition()
    {

        GameObject[] backgrounds = GameObject.FindGameObjectsWithTag("Background");
        float maxX = float.MinValue;

        foreach (GameObject bg in backgrounds)
        {
            if (bg.transform.position.x > maxX)
            {
                maxX = bg.transform.position.x;
            }
        }

        transform.position = new Vector3(maxX + (width/2), transform.position.y, transform.position.z);
    }
}

