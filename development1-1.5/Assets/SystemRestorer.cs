using UnityEngine;

public class SystemRestorer : MonoBehaviour
{
    void Awake()
    {
        // 1. 最重要：强制恢复引擎全局时间。如果这里是0，整个世界就是静止的照片。
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // 恢复物理步长

        // 2. 强制锁定鼠标，绕过 Starter Assets 的点击聚焦检测
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        Debug.Log("系统复位：时间流速已设为 1.0，鼠标已强制锁定。");
    }

    void Update()
    {
        // 如果进入场景后还是动不了，按一下 P 键强制再刷一次时间
        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 1f;
            Debug.Log("手动触发：时间强制恢复");
        }
    }
}