using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public GameObject settingPanel;
    public Button settingButton;
    public Button resumeButton;

    void Start()
    {
        // Ẩn panel lúc đầu
        settingPanel.SetActive(false);

        // Gán sự kiện
        settingButton.onClick.AddListener(OpenSetting);
        resumeButton.onClick.AddListener(CloseSetting);
    }

    void OpenSetting()
    {
        settingPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    void CloseSetting()
    {
        settingPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
