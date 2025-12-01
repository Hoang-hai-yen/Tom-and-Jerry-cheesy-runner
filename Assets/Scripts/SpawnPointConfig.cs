using UnityEngine;

[ExecuteAlways]
public class SpawnPointConfig : MonoBehaviour
{
    public string itemTag;
    [Range(0f, 100f)]
    public float spawnChance = 100f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.2f);
#if UNITY_EDITOR
        UnityEditor.Handles.Label(transform.position + Vector3.up * 0.3f, itemTag);
#endif
    }
}
