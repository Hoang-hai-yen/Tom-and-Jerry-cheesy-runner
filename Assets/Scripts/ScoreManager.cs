using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public TMP_Text cheeseText;
    private int cheeseCount = 0;

    void Awake()
    {
        instance = this;
    }

    public void AddCheese(int amount)
    {
        cheeseCount += amount;
        cheeseText.text = "Cheese: " + cheeseCount;
    }
}
