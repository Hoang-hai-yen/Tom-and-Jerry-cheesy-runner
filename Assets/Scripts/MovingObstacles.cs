using UnityEngine;

public class MovingObstacles : MonoBehaviour
{
    public float speed = 20f;      
    
    [Header("Activation")]
    public float activationDistance = 50f; 
    
    [Header("Despawn")]
    public float despawnOffset = 10f; 
    
    private Transform playerTransform;
    private bool isMoving = false;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform; 
        }
    }

    private void Update()
    {
        if (!isMoving)
        {
            CheckActivation();
        }

        if (isMoving)
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);

            CleanupBehindPlayer();
        }
    }

    private void CheckActivation()
    {
        if (playerTransform == null) return;

        float distanceZ = transform.position.z - playerTransform.position.z;

        if (distanceZ <= activationDistance)
        {
            isMoving = true;
            Debug.Log(gameObject.name + " Activated!");
        }
    }

    private void CleanupBehindPlayer()
    {
        if (playerTransform == null) return;

        if (transform.position.z < playerTransform.position.z - despawnOffset)
        {
            ItemTagHolder tagHolder = GetComponent<ItemTagHolder>();
            if (tagHolder != null && !string.IsNullOrEmpty(tagHolder.itemTag))
            {
                ItemPoolManager.instance.ReturnItem(tagHolder.itemTag, gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}