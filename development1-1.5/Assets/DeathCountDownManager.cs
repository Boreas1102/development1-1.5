using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class DeathCountdownManager : MonoBehaviour
{
    [Header("时间设置")]
    public float remainingTime = 300f; // 5分钟默认倒计时

    [Header("视觉效果引用")]
    public Image blackOverlay; // 最终变黑的遮罩
    public Image bloodFlash;   // 刚才创建的红色遮罩
    public GameObject deathDialog;
    public float fadeDuration = 5.0f;

    private bool _isGameOver = false;
    // 用于记录哪些时间点已经闪烁过，防止重复触发
    private bool _flash1min = false, _flash30s = false, _flash10s = false, _flash5s = false, _flash1s = false;

    void Start()
    {
        if (blackOverlay != null) blackOverlay.color = new Color(0, 0, 0, 0);
        if (bloodFlash != null) bloodFlash.color = new Color(1, 0, 0, 0); // 初始透明红
        if (deathDialog != null) deathDialog.SetActive(false);
    }

    void Update()
    {
        if (_isGameOver) return;

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            CheckFlashPoints();
        }
        else
        {
            StartCoroutine(SequenceDeath());
        }
    }

    void CheckFlashPoints()
    {
        // 1分钟闪烁
        if (remainingTime <= 60f && !_flash1min) { _flash1min = true; StartCoroutine(FlashRed(0.4f)); }
        // 30秒
        if (remainingTime <= 30f && !_flash30s) { _flash30s = true; StartCoroutine(FlashRed(0.5f)); }
        // 10秒
        if (remainingTime <= 10f && !_flash10s) { _flash10s = true; StartCoroutine(FlashRed(0.6f)); }
        // 5秒
        if (remainingTime <= 5f && !_flash5s) { _flash5s = true; StartCoroutine(FlashRed(0.7f)); }
        // 1秒
        if (remainingTime <= 1f && !_flash1s) { _flash1s = true; StartCoroutine(FlashRed(0.8f)); }
    }

    // 红光闪烁协程
    IEnumerator FlashRed(float maxAlpha)
    {
        float t = 0;
        // 快速变红
        while (t < 0.2f)
        {
            t += Time.deltaTime;
            bloodFlash.color = new Color(1, 0, 0, Mathf.Lerp(0, maxAlpha, t / 0.2f));
            yield return null;
        }
        // 缓慢消退
        t = 0;
        while (t < 0.5f)
        {
            t += Time.deltaTime;
            bloodFlash.color = new Color(1, 0, 0, Mathf.Lerp(maxAlpha, 0, t / 0.5f));
            yield return null;
        }
        bloodFlash.color = new Color(1, 0, 0, 0);
    }

    IEnumerator SequenceDeath()
    {
        _isGameOver = true;
        Time.timeScale = 0f;

        float elapsed = 0;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            blackOverlay.color = new Color(0, 0, 0, Mathf.Clamp01(elapsed / fadeDuration));
            yield return null;
        }

        if (deathDialog != null) deathDialog.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void BackToMainMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}