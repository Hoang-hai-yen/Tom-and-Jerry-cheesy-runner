using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MagnetUIEffect : MonoBehaviour
{
    public static MagnetUIEffect Instance;

    void Awake()
    {
        Instance = this;
    }

    public void PlayEffect(RawImage img, float fullAlpha, float fadedAlpha, float duration)
    {
        StartCoroutine(EffectRoutine(img, fullAlpha, fadedAlpha, duration));
    }

    IEnumerator EffectRoutine(RawImage img, float fullAlpha, float fadedAlpha, float duration)
    {
        // Hiện sáng 100%
        SetAlpha(img, fullAlpha);

        // Chờ hết hiệu lực
        yield return new WaitForSeconds(duration);

        // Nhấp nháy 5 lần
        for (int i = 0; i < 5; i++)
        {
            SetAlpha(img, 0f);
            yield return new WaitForSeconds(0.15f);
            SetAlpha(img, 1f);
            yield return new WaitForSeconds(0.15f);
        }

        // Trở lại mờ
        SetAlpha(img, fadedAlpha);
    }

    void SetAlpha(RawImage img, float a)
    {
        if (img == null) return;
        Color c = img.color;
        c.a = a;
        img.color = c;
    }
}
