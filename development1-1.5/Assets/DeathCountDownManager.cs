using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class FinalDeathManager : MonoBehaviour
{
    [Header("时间设置 (秒)")]
    public float remainingTime = 300f; 

    [Header("视觉效果引用")]
    public Image blackOverlay;   // 黑色全屏遮罩
    public Image bloodFlash;     // 红色闪烁遮罩
    public GameObject deathDialog; // 包含按钮和文字的 UI 面板
    public float fadeDuration = 5.0f; // 变黑动画的时长

    [Header("音效设置")]
    public AudioSource heartBeatSource; // 挂载心跳音频的组件

    private bool _isGameOver = false;
    // 标记点：确保每个时间点只触发一次
    private bool _f60 = false, _f30 = false, _f10 = false, _f5 = false, _f1 = false;

    void Start()
    {
        // 确保游戏开始时 UI 是透明/隐藏的
        if (blackOverlay != null) blackOverlay.color = new Color(0, 0, 0, 0);
        if (bloodFlash != null) bloodFlash.color = new Color(1, 0, 0, 0);
        if (deathDialog != null) deathDialog.SetActive(false);
    }

    void Update()
    {
        if (_isGameOver) return;

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            CheckTimeEvents();
        }
        else
        {
            StartCoroutine(SequenceDeath());
        }
    }

    private void CheckTimeEvents()
    {
        // 早期预警：红光闪烁 + 心跳声
        if (remainingTime <= 60f && !_f60) { _f60 = true; StartCoroutine(FlashEffect(0.3f, true)); }
        if (remainingTime <= 30f && !_f30) { _f30 = true; StartCoroutine(FlashEffect(0.5f, true)); }
        if (remainingTime <= 10f && !_f10) { _f10 = true; StartCoroutine(FlashEffect(0.6f, true)); }

        // 濒死阶段：最后5秒和1秒只有红光闪烁，取消心跳声，模拟停跳感
        if (remainingTime <= 5f && !_f5)   { _f5 = true;  StartCoroutine(FlashEffect(0.7f, false)); }
        if (remainingTime <= 1f && !_f1)   { _f1 = true;  StartCoroutine(FlashEffect(0.8f, false)); }
    }

    // 核心闪烁逻辑：playSound 控制是否在此次闪烁中播放音频
    IEnumerator FlashEffect(float maxAlpha, bool playSound)
    {
        if (playSound && heartBeatSource != null) 
        {
            heartBeatSource.Play();
        }

        float t = 0;
        // 快速变红
        while (t < 0.15f)
        {
            t += Time.deltaTime;
            bloodFlash.color = new Color(1, 0, 0, Mathf.Lerp(0, maxAlpha, t / 0.15f));
            yield return null;
        }
        // 缓慢消退
        t = 0;
        while (t < 0.4f)
        {
            t += Time.deltaTime;
            bloodFlash.color = new Color(1, 0, 0, Mathf.Lerp(maxAlpha, 0, t / 0.4f));
            yield return null;
        }
        bloodFlash.color = new Color(1, 0, 0, 0);
    }

    IEnumerator SequenceDeath()
    {
        _isGameOver = true;
        
        // 1. 视野渐隐变黑（使用 unscaledDeltaTime 保证不受 timeScale 干扰）
        float elapsed = 0;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            blackOverlay.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        blackOverlay.color = Color.black;

        // 2. 激活对话框（此时它应该在 Hierarchy 最下方以确保显示在黑屏之上）
        if (deathDialog != null) 
        {
            deathDialog.SetActive(true);
        }

        // 3. 冻结物理世界
        Time.timeScale = 0f;

        // 4. 释放鼠标控制权
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // 用于 UI 按钮点击事件
    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); 
    }
}