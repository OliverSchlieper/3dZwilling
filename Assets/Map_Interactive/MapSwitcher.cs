using UnityEngine;
// No extra namespaces needed for OVRInput as long as the Oculus/Meta SDK is installed

public class MapCycler : MonoBehaviour
{
    [Header("Map Settings")]
    [Tooltip("Drag your Normal, Satellite, and other map versions here.")]
    public Texture2D[] mapTextures; 

    private Renderer mapRenderer;
    private int currentIndex = 0;

    void Start()
    {
        // automatically grab the renderer from the object this script is attached to
        mapRenderer = GetComponent<Renderer>();

        // Check if we have textures to use
        if (mapTextures.Length > 0 && mapRenderer != null)
        {
            ApplyTexture(0);
        }
        else
        {
            Debug.LogError("MapCycler: Missing Renderer or Map Textures!");
        }
    }

    void Update()
    {
        // 'Button.Three' is specifically the X button on the Left Controller
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            CycleNextMap();
        }

        // OPTIONAL: Keep Spacebar for testing on your computer without the headset
        if (Application.isEditor && Input.GetKeyDown(KeyCode.Space))
        {
            CycleNextMap();
        }
    }

    public void CycleNextMap()
    {
        // Move to the next index number
        currentIndex++;

        // If we go past the last map, loop back to 0
        if (currentIndex >= mapTextures.Length)
        {
            currentIndex = 0;
        }

        ApplyTexture(currentIndex);
    }

    private void ApplyTexture(int index)
    {
        if (mapRenderer != null && mapTextures.Length > index)
        {
            mapRenderer.material.mainTexture = mapTextures[index];
        }
    }
}