using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    [Header("List nhạc của từng scene")]
    public AudioClip lobbyBGM;
    public AudioClip mainSceneBGM;

    private AudioSource audioSource;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);     // sống xuyên scene
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Gọi hàm đổi nhạc mỗi khi load scene mới
        ChangeMusicByScene(scene.name);
    }

    public void ChangeMusicByScene(string sceneName)
    {
        AudioClip targetClip = null;

        if (sceneName == "Lobby") targetClip = lobbyBGM;
        if (sceneName == "MainScene") targetClip = mainSceneBGM;

        if (targetClip == null) return;
        if (audioSource.clip == targetClip) return; // tránh restart nếu đang phát đúng nhạc

        PlayMusic(targetClip);
    }

    public void PlayMusic(AudioClip clip, float fadeTime = 0.5f)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeToNewMusic(clip, fadeTime));
    }

    private IEnumerator FadeToNewMusic(AudioClip newClip, float fadeTime)
    {
        float startVolume = audioSource.volume;

        // fade out
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeTime);
            yield return null;
        }

        audioSource.volume = 0;
        audioSource.clip = newClip;
        audioSource.Play();

        // fade in
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0, startVolume, t / fadeTime);
            yield return null;
        }

        audioSource.volume = startVolume;
    }
}
