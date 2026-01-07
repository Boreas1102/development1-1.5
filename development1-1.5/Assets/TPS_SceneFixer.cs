using UnityEngine;
using System.Collections;
// 如果你使用的是新版 Input System，请取消下面这行的注释
// using UnityEngine.InputSystem; 

public class TPS_SceneFixer : MonoBehaviour
{
    void Awake()
    {
        // 1. 解决物理卡死：强制恢复时间
        Time.timeScale = 1f;
        
        // 2. 解决输入卡死：强制锁定鼠标
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    IEnumerator Start()
    {
        // 等待一小会儿，确保 Starter Assets 的控制器自己先初始化完
        yield return new WaitForSecondsRealtime(0.2f);

        // 3. 强力唤醒逻辑：
        // 寻找场景中的输入脚本并手动激活它
        var inputs = FindFirstObjectByType<StarterAssets.StarterAssetsInputs>();
        if (inputs != null)
        {
            // 强行赋予一个微小的位移，迫使输入系统刷新状态
            inputs.MoveInput(Vector2.zero);
            inputs.LookInput(Vector2.zero);
            Debug.Log("已成功强行唤醒 Starter Assets 输入系统");
        }

        // 4. 如果你的相机还是不动，尝试刷新 Cinemachine
        var vcam = FindFirstObjectByType<Unity.Cinemachine.CinemachineVirtualCamera>();
        if (vcam != null)
        {
            vcam.enabled = false;
            vcam.enabled = true; // 闪断一下相机组件，强制重置观察目标
        }
    }
}