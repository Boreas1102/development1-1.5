using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // 必须引用这个才能识别 TextMeshPro

public class DeathInteractionManager : MonoBehaviour
{
    [Header("引用你的死亡文字")]
    public TextMeshProUGUI deathText; // 把你在屏幕上显示的“死亡语”文字拖进来

    [Header("设置")]
    public string mainMenuSceneName = "MainMenu";

    void Update()
    {
        // 核心判断：如果死亡文字在场景中是显示的（Active），且颜色透明度大于0
        if (deathText != null && deathText.gameObject.activeInHierarchy)
        {
            // 只要文字出来了，按下 ESC 就回主菜单
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ReturnToMainMenu();
            }
        }
    }

    public void ReturnToMainMenu()
    {
        // 彻底恢复状态，防止主菜单卡死
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("检测到死亡文本显示，执行 ESC 返回主菜单");
        SceneManager.LoadScene(mainMenuSceneName);
    }
}