using UnityEngine;

public class CheeseCollect : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.instance.AddCheese(1);
            Destroy(gameObject);
        }
    }
}
