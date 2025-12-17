//using UnityEngine;

//public class AudioManager : MonoBehaviour
//{
//    public static AudioManager Instance;

//    public AudioSource sfxSource;
//    public AudioClip buttonClick;
//    public AudioClip pickCheese;
//    public AudioClip pickBuff;

//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    public void PlaySFX(AudioClip clip)
//    {
//        sfxSource.PlayOneShot(clip);
//    }

//    public void PlayButton() => PlaySFX(buttonClick);
//    public void PlayCheese() => PlaySFX(pickCheese);
//    public void PlayBuff() => PlaySFX(pickBuff);
//}

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("SFX Source")]
    [SerializeField] private AudioSource sfxSource;

    [Header("SFX Clips")]
    public AudioClip buttonClick;
    public AudioClip pickCheese;
    public AudioClip pickBuff;

    private bool sfxOn = true;
    private float baseVolume = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Auto tạo AudioSource nếu quên gán
            if (sfxSource == null)
                sfxSource = gameObject.AddComponent<AudioSource>();

            sfxSource.playOnAwake = false;
            baseVolume = sfxSource.volume;

            sfxOn = PlayerPrefs.GetInt("EffectOn", 1) == 1;
            ApplySFXState();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // =========================
    // TOGGLE SFX
    // =========================

    public void SetSFX(bool on)
    {
        sfxOn = on;
        ApplySFXState();

        PlayerPrefs.SetInt("EffectOn", on ? 1 : 0);
        PlayerPrefs.Save();
    }

    public bool IsSFXOn() => sfxOn;

    // =========================
    // PLAY SFX
    // =========================

    public void PlaySFX(AudioClip clip)
    {
        if (!sfxOn || clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    public void PlayButton() => PlaySFX(buttonClick);
    public void PlayCheese() => PlaySFX(pickCheese);
    public void PlayBuff() => PlaySFX(pickBuff);

    // =========================
    // INTERNAL
    // =========================

    private void ApplySFXState()
    {
        sfxSource.mute = !sfxOn;
        sfxSource.volume = baseVolume;
    }
}
