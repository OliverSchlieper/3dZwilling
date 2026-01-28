using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;

public class FiretruckNavigator : MonoBehaviour
{
    [Header("Core References")]
    [Tooltip("Drag the Firetruck object here (e.g., Firetruck1).")]
    public Transform firetruck;

    [Tooltip("Drag the Target object here (e.g., TooltipWorldspace inside Hoverstate).")]
    public Transform targetLocation;

    [Header("Demo Sequence Objects")]
    [Tooltip("The Hoverstate object that should appear first.")]
    public GameObject hoverStateObject;

    [Tooltip("The object inside the firetruck that changes color (e.g., FT1Tooltip). Can be SpriteRenderer, MeshRenderer, or UI Image.")]
    public GameObject tooltipObject;

    [Header("Input")]
    [Tooltip("Map this to the B button. Recommendation: 'Player/Jump'.")]
    public InputActionProperty moveTriggerAction;

    [Header("Sequence Settings")]
    public float delayBeforeColor = 1.0f;
    public float delayBeforeMove = 1.0f;
    public Color signalReceivedColor = Color.green;

    [Header("Movement Settings")]
    public float speed = 5.0f;
    public float stoppingDistance = 0.05f;

    // Internal State
    private bool isSequenceActive = false;
    private bool isMoving = false;
    
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Color originalTooltipColor;
    private Coroutine sequenceCoroutine;

    // Helpers to handle different renderer types
    private Renderer targetRenderer;
    private Image targetImage;

    private void OnEnable()
    {
        if (moveTriggerAction.action != null)
            moveTriggerAction.action.Enable();
    }

    private void OnDisable()
    {
        if (moveTriggerAction.action != null)
            moveTriggerAction.action.Disable();
    }

    void Start()
    {
        if (firetruck != null)
        {
            startPosition = firetruck.position;
            startRotation = firetruck.rotation;
        }

        // Cache Tooltip Renderer/Image and original color
        if (tooltipObject != null)
        {
            targetRenderer = tooltipObject.GetComponent<Renderer>();
            targetImage = tooltipObject.GetComponent<Image>();

            if (targetRenderer != null) originalTooltipColor = targetRenderer.material.color;
            else if (targetImage != null) originalTooltipColor = targetImage.color;
        }

        // Ensure Hoverstate starts hidden
        if (hoverStateObject != null)
        {
            hoverStateObject.SetActive(false);
        }
    }

    void Update()
    {
        // Check Toggle Input
        if (moveTriggerAction.action != null && moveTriggerAction.action.WasPressedThisFrame())
        {
            if (isSequenceActive)
            {
                ResetSequence();
            }
            else
            {
                StartCoroutine(PlayDemoSequence());
            }
        }

        // Handle Movement per frame
        if (isMoving && firetruck != null && targetLocation != null)
        {
            MoveToDestination();
        }
    }

    IEnumerator PlayDemoSequence()
    {
        isSequenceActive = true;
        Debug.Log("FiretruckNavigator: Sequence Started.");

        // 1. Show HoverState
        if (hoverStateObject != null)
        {
            hoverStateObject.SetActive(true);
            Debug.Log("FiretruckNavigator: HoverState Visible.");
        }

        yield return new WaitForSeconds(delayBeforeColor);

        // 2. Change Tooltip Color
        if (tooltipObject != null)
        {
            SetTooltipColor(signalReceivedColor);
            Debug.Log("FiretruckNavigator: Signal Received (Green).");
        }

        yield return new WaitForSeconds(delayBeforeMove);

        // 3. Start Moving
        isMoving = true;
        Debug.Log("FiretruckNavigator: Truck Moving.");
    }

    void ResetSequence()
    {
        StopAllCoroutines();
        isSequenceActive = false;
        isMoving = false;

        // 1. Instant Reset Position
        if (firetruck != null)
        {
            firetruck.position = startPosition;
            firetruck.rotation = startRotation;
        }

        // 2. Hide HoverState
        if (hoverStateObject != null)
        {
            hoverStateObject.SetActive(false);
        }

        // 3. Revert Color
        if (tooltipObject != null)
        {
            SetTooltipColor(originalTooltipColor);
        }

        Debug.Log("FiretruckNavigator: Reset Complete.");
    }

    void SetTooltipColor(Color color)
    {
        if (targetRenderer != null) targetRenderer.material.color = color;
        else if (targetImage != null) targetImage.color = color;
    }

    void MoveToDestination()
    {
        Vector3 flatDestination = new Vector3(targetLocation.position.x, firetruck.position.y, targetLocation.position.z);
        float step = speed * Time.deltaTime;
        
        firetruck.position = Vector3.MoveTowards(firetruck.position, flatDestination, step);

        Vector3 direction = (flatDestination - firetruck.position).normalized;
        if (direction.magnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            firetruck.rotation = Quaternion.RotateTowards(firetruck.rotation, targetRotation, speed * 100 * Time.deltaTime);
        }

        if (Vector3.Distance(firetruck.position, flatDestination) < stoppingDistance)
        {
            firetruck.position = flatDestination; 
            isMoving = false; // Stay there
        }
    }
}
