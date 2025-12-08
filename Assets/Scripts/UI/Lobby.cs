using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackToLobby : MonoBehaviour
{
    public void GoToLobby()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Lobby"); // đổi tên nếu scene của bạn tên khác
    }
}
