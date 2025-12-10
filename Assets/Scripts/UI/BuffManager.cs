using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager Instance;

    public Transform buffPanel;       // panel chứa các buff
    public GameObject buffSlotPrefab; // prefab của BuffSlot

    private void Awake()
    {
        Instance = this;
    }

    public void AddBuff(Sprite icon, float duration)
    {
        GameObject newSlot = Instantiate(buffSlotPrefab, buffPanel);

        // đẩy slot mới lên đầu
        newSlot.transform.SetAsFirstSibling();

        // gọi Setup bên trong BuffSlot
        BuffSlot slot = newSlot.GetComponent<BuffSlot>();
        slot.Setup(icon, duration);
    }
}
