//using UnityEngine;
//using UnityEngine.UI;

//public class MagnetPowerup : MonoBehaviour
//{
//    public float duration = 10f;
//    public RawImage magnetUI;
//    public float fadedAlpha = 0.6f;
//    public float fullAlpha = 1f;

//    void Start()
//    {
//        if (magnetUI == null)
//            magnetUI = GameObject.Find("MagnetIcon").GetComponent<RawImage>();
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (!other.CompareTag("Player")) return;

//        PlayerMovement player = other.GetComponent<PlayerMovement>();
//        if (player != null)
//            player.ActivateMagnet(duration);

//        // Gọi UI xử lý (UI không bị destroyed)
//        MagnetUIEffect.Instance.PlayEffect(magnetUI, fullAlpha, fadedAlpha, duration);

//        // Trả item về pool hoặc destroy
//        ItemTagHolder tag = GetComponent<ItemTagHolder>();
//        if (tag != null && !string.IsNullOrEmpty(tag.itemTag))
//            ItemPoolManager.instance.ReturnItem(tag.itemTag, gameObject);
//        else
//            Destroy(gameObject);
//    }
//}

using UnityEngine;

public class MagnetPowerup : MonoBehaviour
{
    public float duration = 10f;
    public Texture magnetIcon;   // ICON của buff (RawImage dùng Texture)

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        AudioManager.Instance.PlayBuff();
        // Apply hiệu ứng gameplay
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
            player.ActivateMagnet(duration);

        // Báo UI hiển thị buff lên khung buff
        Debug.Log("Gọi AddBuff");
        BuffUIManager.Instance.AddBuff(magnetIcon, duration);

        // Trả item về pool hoặc destroy
        ItemTagHolder tag = GetComponent<ItemTagHolder>();
        if (tag != null && !string.IsNullOrEmpty(tag.itemTag))
            ItemPoolManager.instance.ReturnItem(tag.itemTag, gameObject);
        else
            Destroy(gameObject);
    }
}

