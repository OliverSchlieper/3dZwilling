using UnityEngine;

public class StayUpright : MonoBehaviour
{
    private void LateUpdate()
    {
        // Get current rotation
        Vector3 currentEuler = transform.rotation.eulerAngles;

        // Force X and Z to 0, keep Y
        transform.rotation = Quaternion.Euler(0, currentEuler.y, 0);
    }
}
