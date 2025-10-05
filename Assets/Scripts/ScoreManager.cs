using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("UI")]
    public TextMeshProUGUI cheeseText;           // Text hiển thị khi đang chơi
    public TextMeshProUGUI finalCheeseText;      // Text hiển thị ở GameOverPanel (tùy chọn)

    private int cheeseCount = 0;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateCheeseUI();
    }

    public void AddCheese(int amount)
    {
        cheeseCount += amount;
        UpdateCheeseUI();
    }

    private void UpdateCheeseUI()
    {
        if (cheeseText != null)
            cheeseText.text = "Cheese: " + cheeseCount;
    }

    public void ShowFinalScore()
    {
        if (finalCheeseText != null)
            finalCheeseText.text = "Total Cheese: " + cheeseCount;
    }

    public void ResetScore()
    {
        cheeseCount = 0;
        UpdateCheeseUI();
    }

    public int GetScore()
    {
        return cheeseCount;
    }
}
