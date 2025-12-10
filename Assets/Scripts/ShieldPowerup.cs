//using UnityEngine;

//public class ShieldPowerup : MonoBehaviour
//{
//    public float duration = 8f;
//    public Texture shieldIcon;
//    void OnEnable()
//    {
//        ParticleSystem ps = GetComponent<ParticleSystem>();
//        if (ps != null) ps.Play();
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (!other.CompareTag("Player")) return;

//        PlayerMovement player = other.GetComponent<PlayerMovement>();
//        if (player != null)
//            player.ActivateShield();

//        BuffUIManager.Instance.AddBuff(shieldIcon, duration);



//        ItemTagHolder tagHolder = GetComponent<ItemTagHolder>();
//        if (tagHolder != null && !string.IsNullOrEmpty(tagHolder.itemTag))
//        {
//            ItemPoolManager.instance.ReturnItem(tagHolder.itemTag, gameObject);
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }
//}


using UnityEngine;

public class ShieldPowerup : MonoBehaviour
{
    public float duration = 8f;
    public Texture shieldIcon;

    void OnEnable()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        if (ps != null) ps.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        AudioManager.Instance.PlayBuff();
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.ActivateShield();
            StartCoroutine(DisableShieldAfter(player, duration));  // ⬅️ Tắt khiên sau 8s
        }

        //BuffUIManager.Instance.AddBuff(shieldIcon, duration);

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

    private System.Collections.IEnumerator DisableShieldAfter(PlayerMovement player, float time)
    {
        yield return new WaitForSeconds(time);
        player.DeactivateShield();   // ⬅️ Bạn phải có hàm này trong PlayerMovement
    }
}
