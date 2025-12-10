using UnityEngine;
using UnityEngine.UI;

public class BuffUIItem : MonoBehaviour
{
    public RawImage iconImage;
    public Image timerBar;

    float duration;
    float timer;

    public void Setup(Texture icon, float duration)
    {
        this.duration = duration;
        timer = duration;

        iconImage.texture = icon;
        timerBar.fillAmount = 1;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        timerBar.fillAmount = timer / duration;

        if (timer <= 0)
            Destroy(gameObject);
    }
}
