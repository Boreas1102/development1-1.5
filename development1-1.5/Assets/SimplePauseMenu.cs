using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ManualMenuController : MonoBehaviour
{
    public GameObject pauseMenuUI;

    public void OpenMenu()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; 

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void ContinueGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; 

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    // 关联到 Exit 按钮的 OnClick 事件
    public void ExitToMain()
    {
        Time.timeScale = 1f; // 记得恢复时间，否则主界面会卡死
        SceneManager.LoadScene("MainMenu"); // 替换为你的主界面场景名
    }
}