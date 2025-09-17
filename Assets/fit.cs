using UnityEngine;

public class fit : MonoBehaviour
{
void Reset()
    {
        CapsuleCollider col = GetComponent<CapsuleCollider>();
        Renderer rend = GetComponentInChildren<Renderer>();

        if (rend)
        {
            Bounds b = rend.bounds;
            col.center = transform.InverseTransformPoint(b.center);
            col.height = b.size.y;
            col.radius = Mathf.Max(b.size.x, b.size.z) / 2f;
        }
    }
}
