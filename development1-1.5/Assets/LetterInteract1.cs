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

    [Header("描边设置")]
    public Color outlineColor = Color.yellow;
    [Range(0f, 10f)]
    public float outlineWidth = 5f;

    private bool isPlayerInRange = false;
    private bool isReading = false;
    private Vector2 originalPosition;
    private Outline outline; // 内部引用描边组件

    void Awake()
    {
        // 自动初始化描边组件
        outline = GetComponent<Outline>();
        if (outline == null)
        {
            outline = gameObject.AddComponent<Outline>();
        }
        
        // 设置描边初始参数
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = outlineColor;
        outline.OutlineWidth = outlineWidth;
        outline.enabled = false; // 初始状态关闭
    }

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
        
        // 打开信纸时关闭描边，避免UI挡住时物体还在发光
        if (outline != null) outline.enabled = false;

        letterCanvasGroup.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }

    public void CloseLetter()
    {
        Debug.Log("【系统】关闭信纸界面");
        isReading = false;
        
        // 关闭信纸时，如果玩家还在范围内，重新开启描边
        if (isPlayerInRange && outline != null) outline.enabled = true;

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

    // --- 触发检测 ---
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(">>> [检测成功] 玩家进入了信纸的检测范围！");
            isPlayerInRange = true;
            
            // 显示提示和开启描边
            if (uiPrompt != null && !isReading) uiPrompt.SetActive(true);
            if (outline != null && !isReading) outline.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("<<< [检测成功] 玩家离开了信纸范围。");
            isPlayerInRange = false;
            
            // 隐藏提示和关闭描边
            if (uiPrompt != null) uiPrompt.SetActive(false);
            if (outline != null) outline.enabled = false;
            
            if (isReading) CloseLetter();
        }
    }
}