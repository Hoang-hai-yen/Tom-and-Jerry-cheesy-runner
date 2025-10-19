using UnityEngine;

public class StarScoreUp : MonoBehaviour
{
    public int multiplier = 2;
    public float duration = 10f; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Gọi hàm kích hoạt nhân điểm trên ScoreManager
            if (ScoreManager.instance != null)
            {
                ScoreManager.instance.ActivateScoreMultiplier(multiplier, duration);
            }
            
            // Phá hủy item sau khi nhặt
            Destroy(gameObject);
        }
    }
}
