using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager instance;

    [Header("UI Elements")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalCheeseText;

    private bool isGameOver = false;

    void Awake()
    {
        instance = this;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        Time.timeScale = 0f; 
        gameOverPanel.SetActive(true);

        if (finalCheeseText != null)
        {
            int finalScore = ScoreManager.instance.GetScore();
            finalCheeseText.text = "" + finalScore;
        }
    }

    public void RestartGame()
    {
        AudioManager.Instance.PlayBuff();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
