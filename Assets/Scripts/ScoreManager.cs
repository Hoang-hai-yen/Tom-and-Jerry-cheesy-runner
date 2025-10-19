using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("UI")]
    public TextMeshProUGUI cheeseText;          
    public TextMeshProUGUI finalCheeseText;     

    private int cheeseCount = 0;

    private int scoreMultiplier = 1;
    private float multiplierTimer;
    private bool isMultiplierActive = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateCheeseUI();
    }

    void Update()
    {
        if (isMultiplierActive)
        {
            multiplierTimer -= Time.deltaTime;
            
            if (multiplierTimer <= 0)
            {
                DeactivateScoreMultiplier();
            }
        }
    }

    public void AddCheese(int amount)
    {
        cheeseCount += (amount * scoreMultiplier);
        
        UpdateCheeseUI();
    }

    private void UpdateCheeseUI()
    {
        if (cheeseText != null)
        {
            string multiplierText = (isMultiplierActive) ? $" (x{scoreMultiplier})" : "";
            cheeseText.text = "Cheese: " + cheeseCount + multiplierText;
        }
    }

    public void ShowFinalScore()
    {
        if (finalCheeseText != null)
            finalCheeseText.text = "Total Cheese: " + cheeseCount;
    }

    public void ResetScore()
    {
        cheeseCount = 0;
        DeactivateScoreMultiplier(); 
        UpdateCheeseUI();
    }

    public int GetScore()
    {
        return cheeseCount;
    }

    public void ActivateScoreMultiplier(int multiplier, float duration)
    {
        scoreMultiplier = multiplier;
        multiplierTimer = duration;
        isMultiplierActive = true;
        Debug.Log($"Score Multiplier x{multiplier} activated for {duration} seconds!");
        UpdateCheeseUI(); 
    }

    private void DeactivateScoreMultiplier()
    {
        scoreMultiplier = 1;
        isMultiplierActive = false;
        Debug.Log("Score Multiplier deactivated.");
        UpdateCheeseUI(); 
    }
}