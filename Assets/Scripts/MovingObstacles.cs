using UnityEngine;

public class MovingObstacles : MonoBehaviour
{
    public float speed = 20f;      
    public float lifeTime = 10f;  

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
