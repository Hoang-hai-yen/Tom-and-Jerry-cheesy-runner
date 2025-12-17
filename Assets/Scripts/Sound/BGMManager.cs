//using UnityEngine;
//using UnityEngine.SceneManagement;
//using System.Collections;

//public class BGMManager : MonoBehaviour
//{
//    public static BGMManager Instance;

//    [Header("List nhạc của từng scene")]
//    public AudioClip lobbyBGM;
//    public AudioClip mainSceneBGM;

//    private AudioSource audioSource;
//    private Coroutine fadeCoroutine;

//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);     // sống xuyên scene
//            audioSource = gameObject.AddComponent<AudioSource>();
//            audioSource.loop = true;

//            SceneManager.sceneLoaded += OnSceneLoaded;
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//    {
//        // Gọi hàm đổi nhạc mỗi khi load scene mới
//        ChangeMusicByScene(scene.name);
//    }

//    public void ChangeMusicByScene(string sceneName)
//    {
//        AudioClip targetClip = null;

//        if (sceneName == "Lobby") targetClip = lobbyBGM;
//        if (sceneName == "MainScene") targetClip = mainSceneBGM;

//        if (targetClip == null) return;
//        if (audioSource.clip == targetClip) return; // tránh restart nếu đang phát đúng nhạc

//        PlayMusic(targetClip);
//    }

//    public void PlayMusic(AudioClip clip, float fadeTime = 0.5f)
//    {
//        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
//        fadeCoroutine = StartCoroutine(FadeToNewMusic(clip, fadeTime));
//    }

//    private IEnumerator FadeToNewMusic(AudioClip newClip, float fadeTime)
//    {
//        float startVolume = audioSource.volume;

//        // fade out
//        for (float t = 0; t < fadeTime; t += Time.deltaTime)
//        {
//            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeTime);
//            yield return null;
//        }

//        audioSource.volume = 0;
//        audioSource.clip = newClip;
//        audioSource.Play();

//        // fade in
//        for (float t = 0; t < fadeTime; t += Time.deltaTime)
//        {
//            audioSource.volume = Mathf.Lerp(0, startVolume, t / fadeTime);
//            yield return null;
//        }

//        audioSource.volume = startVolume;
//    }

//    public void SetMusic(bool on)
//    {
//        audioSource.mute = !on;
//    }
//}


using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    [Header("BGM theo Scene")]
    public AudioClip lobbyBGM;
    public AudioClip mainSceneBGM;

    [Header("Fade")]
    public float defaultFadeTime = 0.5f;

    private AudioSource audioSource;
    private Coroutine fadeCoroutine;
    private bool musicOn = true;
    private float baseVolume = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.playOnAwake = false;
            baseVolume = audioSource.volume;

            // Load trạng thái Music
            musicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
            audioSource.mute = !musicOn;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ChangeMusicByScene(scene.name);
    }

    // =========================
    // CHANGE MUSIC
    // =========================

    public void ChangeMusicByScene(string sceneName)
    {
        AudioClip targetClip = null;

        if (sceneName == "Lobby")
            targetClip = lobbyBGM;
        else if (sceneName == "MainScene")
            targetClip = mainSceneBGM;

        if (targetClip == null) return;
        if (audioSource.clip == targetClip) return;

        PlayMusic(targetClip, defaultFadeTime);
    }

    public void PlayMusic(AudioClip clip, float fadeTime)
    {
        if (clip == null) return;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeToNewMusic(clip, fadeTime));
    }

    private IEnumerator FadeToNewMusic(AudioClip newClip, float fadeTime)
    {
        float startVolume = baseVolume;

        // Fade out
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeTime);
            yield return null;
        }

        audioSource.volume = 0;
        audioSource.clip = newClip;
        audioSource.Play();

        // Fade in (chỉ khi Music ON)
        if (musicOn)
        {
            for (float t = 0; t < fadeTime; t += Time.deltaTime)
            {
                audioSource.volume = Mathf.Lerp(0, startVolume, t / fadeTime);
                yield return null;
            }
        }

        audioSource.volume = musicOn ? startVolume : 0;
    }

    // =========================
    // TOGGLE MUSIC
    // =========================

    public void SetMusic(bool on)
    {
        musicOn = on;
        audioSource.mute = !on;

        PlayerPrefs.SetInt("MusicOn", on ? 1 : 0);
        PlayerPrefs.Save();
    }

    public bool IsMusicOn()
    {
        return musicOn;
    }
}
