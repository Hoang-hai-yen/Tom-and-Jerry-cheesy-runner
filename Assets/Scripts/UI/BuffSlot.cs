using UnityEngine;
using UnityEngine.UI;

public class BuffSlot : MonoBehaviour
{
    public RawImage icon;        // icon của buff
    public Image timerBar;    // thanh thời gian

    float duration;   // thời gian tồn tại
    float timer;      // đếm ngược

    public void Setup(Sprite buffIcon, float buffDuration)
    {
        icon.texture = buffIcon.texture;

        duration = buffDuration;
        timer = duration;
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            // cập nhật thanh thời gian
            timerBar.fillAmount = timer / duration;
        }
        else
        {
            Destroy(gameObject);   // tự xoá slot khi hết thời gian
        }
    }
}
