using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundToggleButton : MonoBehaviour, IPointerClickHandler
{
    public enum SoundType { Music, Effect }
    public SoundType soundType;

    [Header("Icon")]
    public Sprite iconOn;
    public Sprite iconOff;

    private Image image;
    private bool isOn = true;

    void Start()
    {
        image = GetComponent<Image>();

        // Load trạng thái đã lưu
        if (soundType == SoundType.Music)
            isOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        else
            isOn = PlayerPrefs.GetInt("EffectOn", 1) == 1;

        ApplyState();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isOn = !isOn;
        ApplyState();
        SaveState();
    }

    void ApplyState()
    {
        image.sprite = isOn ? iconOn : iconOff;

        if (soundType == SoundType.Music)
        {
            BGMManager.Instance.SetMusic(isOn);
        }
        else
        {
            AudioManager.Instance.SetSFX(isOn);
        }
    }

    void SaveState()
    {
        if (soundType == SoundType.Music)
            PlayerPrefs.SetInt("MusicOn", isOn ? 1 : 0);
        else
            PlayerPrefs.SetInt("EffectOn", isOn ? 1 : 0);

        PlayerPrefs.Save();
    }
}
