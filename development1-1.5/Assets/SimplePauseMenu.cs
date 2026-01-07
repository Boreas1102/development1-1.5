using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ManualMenuController : MonoBehaviour
{
    [Header("UI 设置")]
    public GameObject pauseMenuUI;

    void Update()
    {
        // 允许玩家通过键盘 ESC 键切换菜单状态
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuUI.activeSelf)
            {
                ContinueGame();
            }
            else
            {
                OpenMenu();
            }
        }
    }

    // 1. 打开菜单
    public void OpenMenu()
    {
        if (pauseMenuUI == null) return;

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // 停止游戏物理和时间

        // 核心修复：强制显示并解锁鼠标，确保能点到按钮
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 清除 UI 选择状态，防止按钮虚假聚焦
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
        
        Debug.Log("菜单已打开，鼠标已解锁");
    }

    // 2. 继续游戏 (Continue 按钮关联)
    public void ContinueGame()
    {
        if (pauseMenuUI == null) return;

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // 恢复游戏时间

        // 重新锁定鼠标回到 3D 操作模式
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("游戏继续，鼠标已重新锁定");
    }

    // 3. 退出到主菜单 (Exit 按钮关联)
    public void ExitToMain()
    {
        // 关键步骤：在跳转前必须重置状态，否则主菜单会处于暂停或鼠标锁定状态
        Time.timeScale = 1f; 
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("正在跳转回主菜单...");
        
        // 请确保 Build Settings 里的主菜单场景名字确实是 "MainMenu"
        SceneManager.LoadScene("MainMenu"); 
    }
}