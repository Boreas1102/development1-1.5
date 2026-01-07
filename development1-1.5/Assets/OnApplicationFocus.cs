using UnityEngine;
using StarterAssets; // 必须引用这个

public class UltimateTPSUnlocker : MonoBehaviour
{
    private StarterAssetsInputs _inputs;
    private ThirdPersonController _controller;

    void Start()
    {
        _inputs = GetComponent<StarterAssetsInputs>();
        _controller = GetComponent<ThirdPersonController>();
        
        // 强制重置
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 如果你按下 W 键还是没反应，强制激活输入状态
        if (Input.anyKey && _inputs != null)
        {
            _inputs.cursorLocked = true;
            _inputs.cursorInputForLook = true;
        }

        // 预防万一：如果按下键盘，确保时间还在跑
        if (Time.timeScale < 1f) Time.timeScale = 1f;
    }
}