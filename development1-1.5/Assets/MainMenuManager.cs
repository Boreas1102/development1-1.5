using UnityEngine;
using UnityEngine.SceneManagement; // 用于场景跳转
using UnityEngine.Audio;         // 用于控制 Audio Mixer
using UnityEngine.UI;            // 用于 UI 组件操作

public class MainMenuManager : MonoBehaviour
{
    [Header("场景与面板配置")]
    public string gameSceneName = "LabScene"; // 确保这里填入你实验室场景的准确名称
    public GameObject settingsPanel;         // 拖入你的声音设置 Panel

    [Header("音频配置")]
    public AudioMixer mainMixer;             // 拖入你创建的 Audio Mixer 资源
    public string exposedParamName = "MyExposedVolume"; // 必须与 Mixer 中暴露的参数名一致

    void Awake()
    {
        // 1. 强制重置时间流速，防止从死亡结算页面回来时时间依然冻结
        Time.timeScale = 1f;

        // 2. 彻底释放鼠标，确保在 2D 菜单中可以正常点击
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        // 初始状态隐藏设置面板
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    // --- 核心功能函数 ---

    // 1. 开始游戏
    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // 2. 切换设置面板（isOpen 为 true 打开，false 关闭）
    public void ToggleSettings(bool isOpen)
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(isOpen);
        }
    }

    // 3. 全局音量调节 (由 Slider 的 OnValueChanged 调用)
    public void SetGlobalVolume(float sliderValue)
    {
        // 将 Slider 的 0.0001 - 1 线性值转换为分贝对数 (-80dB 到 0dB)
        // 这样调节音量的听觉感受会非常线性平滑
        float volume = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;
        
        if (mainMixer != null)
        {
            mainMixer.SetFloat(exposedParamName, volume);
        }
    }

    // 4. 退出游戏
    public void QuitGame()
    {
        Debug.Log("Game Exiting...");
        Application.Quit(); // 打包后的程序会关闭

        // 在 Unity 编辑器模式下点击也会停止运行
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}