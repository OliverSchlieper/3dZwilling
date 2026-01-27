using UnityEngine;
using UnityEngine.InputSystem;

public class FloorManager : MonoBehaviour
{
    [Header("Floor Layers (0: Street, 1: U1, 2: U2)")]
    public GameObject[] layers; 

    [Header("Input Settings")]
    public InputActionProperty moveAction; 
    
    [Range(0.1f, 0.9f)]
    public float threshold = 0.5f;

    private int currentFloorIndex = 0; 
    private bool canToggle = true; 

    private void OnEnable()
    {
        // IMPORTANT: Custom actions must be enabled manually or they won't "listen"
        if (moveAction.action != null)
        {
            moveAction.action.Enable();
            Debug.Log("FloorManager: Move Action Enabled.");
        }
        else
        {
            Debug.LogError("FloorManager: No Action assigned to Move Action!");
        }
    }

    private void OnDisable()
    {
        if (moveAction.action != null)
            moveAction.action.Disable();
    }

    void Start()
    {
        if (layers == null || layers.Length == 0)
        {
            Debug.LogWarning("FloorManager: No layers assigned in the inspector!");
        }
        UpdateVisibility();
    }

    void Update()
    {
        if (moveAction.action == null) return;

        // Read joystick value
        Vector2 input = moveAction.action.ReadValue<Vector2>();

        // DEBUG: Uncomment the line below to see raw values in the console
        // if (input.magnitude > 0.01f) Debug.Log($"Joystick Input: {input}");

        // Flick UP (toward Street level)
        if (input.y > threshold && canToggle)
        {
            Debug.Log("Flicked UP: Moving toward Street level.");
            ChangeFloor(-1);
            canToggle = false;
        }
        // Flick DOWN (toward U2 level)
        else if (input.y < -threshold && canToggle)
        {
            Debug.Log("Flicked DOWN: Moving toward U2 level.");
            ChangeFloor(1);
            canToggle = false;
        }
        // Reset toggle when joystick returns to center
        else if (Mathf.Abs(input.y) < 0.2f)
        {
            canToggle = true;
        }
    }

    void ChangeFloor(int adjustment)
    {
        int oldIndex = currentFloorIndex;
        currentFloorIndex = Mathf.Clamp(currentFloorIndex + adjustment, 0, layers.Length - 1);
        
        if (oldIndex != currentFloorIndex)
        {
            Debug.Log($"Floor Changed to Index: {currentFloorIndex}");
            UpdateVisibility();
        }
    }

    void UpdateVisibility()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            if (layers[i] != null)
            {
                // Shows the current layer and everything below it
                layers[i].SetActive(i >= currentFloorIndex);
            }
        }
    }
}