using UnityEngine;
using UnityEngine.EventSystems;
using StarterAssets; // 必须添加这个命名空间来引用 StarterAssets 的脚本

public class UltimateGameController : MonoBehaviour
{
    public GameObject pauseMenuUI;

    [Header("Starter Assets 引用")]
    // 拖入 Player 物体上的 ThirdPersonController 脚本
    public MonoBehaviour characterController; 
    // 拖入 Player 物体上的 StarterAssetsInputs 脚本
    public MonoBehaviour starterAssetsInputs; 

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }

        // 防空格干扰：强制踢掉 UI 焦点
        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        // 恢复控制
        SetPlayerControl(true);

        // 锁定鼠标
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        // 禁用控制
        SetPlayerControl(false);

        // 释放鼠标
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // 统一管理开关
    private void SetPlayerControl(bool state)
    {
        if (characterController != null) characterController.enabled = state;
        if (starterAssetsInputs != null) starterAssetsInputs.enabled = state;
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}