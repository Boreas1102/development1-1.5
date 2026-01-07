using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LetterInteract1 : MonoBehaviour
{
    [Header("UI 引用")]
    public GameObject uiPrompt;           // 提示按E的UI
    public CanvasGroup letterCanvasGroup;  // 控制面板透明度
    public RectTransform letterRect;      // 控制面板位置

    [Header("动画设置")]
    public float animationDuration = 0.5f;
    public float fadeDistance = 50f;

    private bool isPlayerInRange = false;
    private bool isReading = false;
    private Vector2 originalPosition;

    void Start()
    {
        if (letterCanvasGroup != null)
        {
            letterCanvasGroup.alpha = 0; 
            letterCanvasGroup.gameObject.SetActive(false);
            originalPosition = letterRect.anchoredPosition;
        }
        else
        {
            Debug.LogError("【错误】LetterCanvasGroup 未关联！请检查 Inspector 面板。");
        }
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isReading) OpenLetter();
            else CloseLetter();
        }

        if (isReading && Input.GetKeyDown(KeyCode.Escape)) CloseLetter();
    }

    public void OpenLetter()
    {
        Debug.Log("【系统】打开信纸界面");
        isReading = true;
        if(uiPrompt != null) uiPrompt.SetActive(false);
        letterCanvasGroup.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }

    public void CloseLetter()
    {
        Debug.Log("【系统】关闭信纸界面");
        isReading = false;
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        float elapsed = 0;
        Vector2 startPos = originalPosition - new Vector2(0, fadeDistance);
        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / animationDuration;
            letterCanvasGroup.alpha = Mathf.Lerp(0, 1, t);
            letterRect.anchoredPosition = Vector2.Lerp(startPos, originalPosition, t);
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        float elapsed = 0;
        Vector2 endPos = originalPosition - new Vector2(0, fadeDistance);
        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / animationDuration;
            letterCanvasGroup.alpha = Mathf.Lerp(1, 0, t);
            letterRect.anchoredPosition = Vector2.Lerp(originalPosition, endPos, t);
            yield return null;
        }
        letterCanvasGroup.gameObject.SetActive(false);
    }

    // --- 调试核心：触发检测 ---
    private void OnTriggerEnter(Collider other)
    {
        // 如果控制台没出这行字，说明 Collider 没碰到玩家
        if (other.CompareTag("Player"))
        {
            Debug.Log(">>> [检测成功] 玩家进入了信纸的检测范围！");
            isPlayerInRange = true;
            if (uiPrompt != null && !isReading) uiPrompt.SetActive(true);
        }
        else
        {
            Debug.Log(">>> [检测到碰撞] 但碰撞物体的 Tag 是: " + other.tag + "，不是 Player");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("<<< [检测成功] 玩家离开了信纸范围。");
            isPlayerInRange = false;
            if (uiPrompt != null) uiPrompt.SetActive(false);
            if (isReading) CloseLetter();
        }
    }
}