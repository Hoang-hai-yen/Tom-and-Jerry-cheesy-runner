using UnityEngine;

public class CheeseCollect : MonoBehaviour
{
    public float attractionSpeed = 15f;
    private bool isAttracted = false;
    private Transform playerTarget;

    void OnEnable()
    {
        isAttracted = false;
        playerTarget = null;

        ParticleSystem ps = GetComponent<ParticleSystem>();
        if (ps != null) ps.Play();

    }

    void Update()
    {
        if (isAttracted && playerTarget != null)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                playerTarget.position,
                attractionSpeed * Time.deltaTime
            );
        }
    }

    public void Attract(Transform player)
    {
        isAttracted = true;
        playerTarget = player;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        AudioManager.Instance.PlayCheese();
        ScoreManager.instance?.AddCheese(1);

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
