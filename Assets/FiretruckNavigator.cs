using UnityEngine;
using UnityEngine.InputSystem;

public class FiretruckNavigator : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Drag the Firetruck object here (e.g., Firetruck1).")]
    public Transform firetruck;

    [Tooltip("Drag the Target object here (e.g., TooltipWorldspace inside Hoverstate).")]
    public Transform targetLocation;

    [Header("Input")]
    [Tooltip("Map this to the B button. Recommendation: Use 'Player/Jump' or create a specific action for 'Secondary Button'.")]
    public InputActionProperty moveTriggerAction;

    [Header("Movement Settings")]
    [Tooltip("Speed of the firetruck in meters per second.")]
    public float speed = 5.0f;
    
    [Tooltip("How close the truck needs to be to stop.")]
    public float stoppingDistance = 0.05f;

    private bool isMoving = false;
    private Vector3 currentDestination;
    private Vector3 startPosition;
    private Quaternion startRotation;
    
    // State tracking: true = at target/moving to target, false = at start/moving to start
    private bool isAtTarget = false; 

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
            // Remember where we started so we can go back
            startPosition = firetruck.position;
            startRotation = firetruck.rotation;
        }
    }

    void Update()
    {
        // 1. Check Input
        if (moveTriggerAction.action != null && moveTriggerAction.action.WasPressedThisFrame())
        {
            if (firetruck != null && targetLocation != null)
            {
                ToggleDestination();
            }
            else
            {
                Debug.LogError("FiretruckNavigator: Missing Firetruck or Target references!");
            }
        }

        // 2. Handle Movement
        if (isMoving && firetruck != null)
        {
            MoveToDestination();
        }
    }

    void ToggleDestination()
    {
        if (!isAtTarget)
        {
            // Go to Target
            currentDestination = targetLocation.position;
            isAtTarget = true;
            isMoving = true;
            Debug.Log("FiretruckNavigator: Moving to Target.");
        }
        else
        {
            // Reset to Start DESTINATION instantly
            // We don't "move" there, we teleport.
            if (firetruck != null)
            {
                firetruck.position = startPosition;
                firetruck.rotation = startRotation;
            }
            
            isAtTarget = false;
            isMoving = false; // Stop moving logic since we are already there
            Debug.Log("FiretruckNavigator: Instantly reset to Start.");
        }
    }

    void MoveToDestination()
    {
        // Calculate target position with the SAME height (Y) as the firetruck's current height
        Vector3 flatDestination = new Vector3(currentDestination.x, firetruck.position.y, currentDestination.z);

        // Move towards that flat destination
        float step = speed * Time.deltaTime;
        firetruck.position = Vector3.MoveTowards(firetruck.position, flatDestination, step);

        // Calculate direction for rotation (ignoring Y)
        Vector3 direction = (flatDestination - firetruck.position).normalized;

        if (direction.magnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            firetruck.rotation = Quaternion.RotateTowards(firetruck.rotation, targetRotation, speed * 100 * Time.deltaTime);
        }

        // Check availability
        if (Vector3.Distance(firetruck.position, flatDestination) < stoppingDistance)
        {
            // Snap to exact position
            firetruck.position = flatDestination; 
            isMoving = false;
            
            Debug.Log("FiretruckNavigator: Reached destination.");
        }
    }
}
