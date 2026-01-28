using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectVisibilityToggle : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The object to show/hide (e.g., Hoverstate).")]
    public GameObject targetObject;

    [Header("Input")]
    [Tooltip("Map this to the A button (Primary Button). Recommendation: Use 'UI/Submit' or 'Player/Interact' if supported, or create a 'PrimaryButton' action.")]
    public InputActionProperty toggleAction;

    [Header("Settings")]
    [Tooltip("Should the object be visible when the game starts?")]
    public bool startVisible = false;

    private void OnEnable()
    {
        if (toggleAction.action != null)
            toggleAction.action.Enable();
    }

    private void OnDisable()
    {
        if (toggleAction.action != null)
            toggleAction.action.Disable();
    }

    void Start()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(startVisible);
        }
        else
        {
            Debug.LogWarning("ObjectVisibilityToggle: Target Object is not assigned.");
        }
    }

    void Update()
    {
        if (toggleAction.action != null && toggleAction.action.WasPressedThisFrame())
        {
            if (targetObject != null)
            {
                bool newState = !targetObject.activeSelf;
                targetObject.SetActive(newState);
                Debug.Log($"ObjectVisibilityToggle: Toggled {targetObject.name} to {newState}");
            }
        }
    }
}
