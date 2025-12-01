using UnityEngine;

public class StarScoreUp : MonoBehaviour
{
    public int multiplier = 2;
    public float duration = 10f;

    void OnEnable()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        if (ps != null) ps.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        ScoreManager.instance?.ActivateScoreMultiplier(multiplier, duration);

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
