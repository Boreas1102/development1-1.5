using UnityEngine;

public class GameSceneInitializer : MonoBehaviour
{
    void Start()
    {
        // 1. 锁定鼠标到屏幕中心，并隐藏图标
        // 这样你的第三人称相机脚本才能检测到鼠标的“移动量”
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 2. 确保游戏时间是运行的（防止主菜单暂停了时间）
        Time.timeScale = 1f;

        Debug.Log("第三人称场景初始化：鼠标已锁定，准备战斗！");
    }

    // 提示：如果你在游戏中按 Esc 想唤出鼠标，可以加这个小功能
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 切换鼠标状态
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}