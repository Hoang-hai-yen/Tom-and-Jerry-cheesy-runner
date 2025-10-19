using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public Vector3 rotationAxis = Vector3.up;

    void Update()
    {
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }
}
