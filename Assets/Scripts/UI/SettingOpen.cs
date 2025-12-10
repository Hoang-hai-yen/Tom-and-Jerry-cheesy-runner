using UnityEngine;
using UnityEngine.EventSystems;

public class SettingButton : MonoBehaviour, IPointerClickHandler
{
    public GameObject settingPanel;

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlayButton();
        settingPanel.SetActive(!settingPanel.activeSelf);
    }
}