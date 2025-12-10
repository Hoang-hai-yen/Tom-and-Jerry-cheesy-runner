using UnityEngine;
using UnityEngine.UI;

public class BuffUIManager : MonoBehaviour
{
    public static BuffUIManager Instance;

    public Transform buffParent;          // Vùng chứa các buff
    public GameObject buffUIPrefab;       // Prefab UI Buff đã tạo trước

    private void Awake()
    {
        Instance = this;
        Debug.Log("BuffUIManager Awake"); // KIỂM TRA
    }

    public BuffUIItem AddBuff(Texture icon, float duration)
    {
        Debug.Log("AddBuff CALLED");

        GameObject obj = Instantiate(buffUIPrefab, buffParent);
        Debug.Log("New BuffUI Spawned: " + obj.name + " at " + buffParent.name);

        BuffUIItem item = obj.GetComponent<BuffUIItem>();
        item.Setup(icon, duration);

        return item;   // ⬅️ Trả về để Player giữ lại
    }

    public void RemoveBuff(BuffUIItem item)
    {
        if (item != null)
            Destroy(item.gameObject);  // Xóa đúng buff slot
    }
}
