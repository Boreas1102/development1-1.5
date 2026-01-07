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

    // 描边组件引用
    private Outline outline; 

    private bool isPlayerInRange = false;
    private bool isReading = false;
    private Vector2 originalPosition;

    void Start()
    {
        // 自动获取物体上的 Outline 组件
        outline = GetComponent<Outline>();
        if (outline != null) 
        {
            outline.enabled = false; // 初始关闭
        }

        if (letterCanvasGroup != null)
        {
            letterCanvasGroup.alpha = 0; 
            letterCanvasGroup.gameObject.SetActive(false);
            originalPosition = letterRect.anchoredPosition;
        }
    }

    void Update()
    {
        // 按 E 键开关信件
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isReading) OpenLetter();
            else CloseLetter();
        }

        // 读信时按 Esc 也可以关闭
        if (isReading && Input.GetKeyDown(KeyCode.Escape)) CloseLetter();
    }

    public void OpenLetter()
    {
        isReading = true;
        if(uiPrompt != null) uiPrompt.SetActive(false);
        
        letterCanvasGroup.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }

    public void CloseLetter()
    {
        isReading = false;
        StopAllCoroutines();
        StartCoroutine(FadeOut());
        
        // 关闭后如果人还在范围内，重新显示 E 提示
        if (isPlayerInRange && uiPrompt != null) uiPrompt.SetActive(true);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            
            // 靠近显示描边
            if (outline != null) outline.enabled = true; 
            
            if (uiPrompt != null && !isReading) uiPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            
            // 离开关闭描边
            if (outline != null) outline.enabled = false; 
            
            if (uiPrompt != null) uiPrompt.SetActive(false);
            if (isReading) CloseLetter();
        }
    }
}