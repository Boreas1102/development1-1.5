using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ManualMenuController : MonoBehaviour
{
    public GameObject pauseMenuUI;

    // 该方法由你场景中的“暂停按钮”或特定逻辑调用
    public void OpenMenu()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // 冻结游戏时间

        // 释放鼠标
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 核心修复：打开菜单瞬间清除所有 UI 焦点，确保空格键无效
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    // 关联到 Continue 按钮的 OnClick 事件
    public void ContinueGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // 恢复游戏时间

        // 重新锁定鼠标
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 再次清除焦点
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