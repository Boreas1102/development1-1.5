using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LetterInteract1 : MonoBehaviour
{
    [Header("UI 引用")]
    public GameObject uiPrompt;           
    public CanvasGroup letterCanvasGroup;  
    public RectTransform letterRect;      

    [Header("动画设置")]
    public float animationDuration = 0.5f;
    public float fadeDistance = 50f;

    private Outline outline; 

    private bool isPlayerInRange = false;
    private bool isReading = false;
    private Vector2 originalPosition;

    void Start()
    {
        outline = GetComponent<Outline>();
        if (outline != null) 
        {
            outline.enabled = false; 
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
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isReading) OpenLetter();
            else CloseLetter();
        }

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
            
            if (outline != null) outline.enabled = true; 
            
            if (uiPrompt != null && !isReading) uiPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            
            if (outline != null) outline.enabled = false; 
            
            if (uiPrompt != null) uiPrompt.SetActive(false);
            if (isReading) CloseLetter();
        }
    }
}