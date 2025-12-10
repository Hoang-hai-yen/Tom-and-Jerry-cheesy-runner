using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource sfxSource;
    public AudioClip buttonClick;
    public AudioClip pickCheese;
    public AudioClip pickBuff;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayButton() => PlaySFX(buttonClick);
    public void PlayCheese() => PlaySFX(pickCheese);
    public void PlayBuff() => PlaySFX(pickBuff);
}
