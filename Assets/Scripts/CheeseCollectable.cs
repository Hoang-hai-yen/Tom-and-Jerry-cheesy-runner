using UnityEngine;

public class CheeseCollect : MonoBehaviour
{
    public float attractionSpeed = 15;
    private bool isAttracted = false;
    private Transform playerTarget;
    void Update()
    {
        if (isAttracted && playerTarget != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTarget.position, attractionSpeed * Time.deltaTime);
        }
    }
    public void Attract(Transform player)
    {
        isAttracted = true;
        playerTarget = player;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (ScoreManager.instance != null)
            {
                ScoreManager.instance.AddCheese(1);
            }
            Destroy(gameObject);
        }
    }
}
