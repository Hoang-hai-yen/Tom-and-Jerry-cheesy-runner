using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;      
    public Vector3 offset;        
    public float smoothSpeed = 2f; 

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = new Vector3(0, target.position.y + offset.y, target.position.z + offset.z);

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;
    }
}
