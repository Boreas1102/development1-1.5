using UnityEngine;
using System.Collections;

public class MushroomPickup : MonoBehaviour
{
    [Header("UI 引用")]
    public GameObject uiPrompt;       // 之前关联的UI
    
    [Header("反馈设置")]
    public GameObject pickupEffect;   // 拖入你的粒子特效Prefab
    public AudioClip pickupSound;     // 拖入你的音效文件
    public float fadeDuration = 1.0f; // 消逝持续时间

    [Header("描边设置")]
    public Color outlineColor = Color.green; // 蘑菇建议用绿色或白色
    [Range(0f, 10f)]
    public float outlineWidth = 5f;

    private bool isPlayerInRange = false;
    private bool isPickedUp = false;
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

    void Update()
    {
        // 只有在范围内且没被采摘时才检测按键
        if (isPlayerInRange && !isPickedUp && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(PickUpSequence());
        }
    }

    IEnumerator PickUpSequence()
    {
        isPickedUp = true;
        
        // 1. 彻底关闭 UI 和 描边
        if (uiPrompt != null) uiPrompt.SetActive(false);
        if (outline != null) outline.enabled = false;

        // 2. 播放声音
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }

        // 3. 生成粒子特效
        if (pickupEffect != null)
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
        }

        // 4. 逐渐消逝（变透明或变小）
        float elapsed = 0;
        Vector3 initialScale = transform.localScale;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / fadeDuration;

            // 效果：逐渐变小并消失
            transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, percent);
            
            yield return null;
        }

        // 5. 彻底销毁物体
        Destroy(gameObject);
        Debug.Log("蘑菇采集完成并销毁");
    }

    private void OnTriggerEnter(Collider other)
    {
        // 只有没被采摘时才触发
        if (other.CompareTag("Player") && !isPickedUp)
        {
            isPlayerInRange = true;
            if (uiPrompt != null) uiPrompt.SetActive(true);
            
            // --- 新增：开启描边 ---
            if (outline != null) outline.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (uiPrompt != null) uiPrompt.SetActive(false);
            
            // --- 新增：关闭描边 ---
            if (outline != null) outline.enabled = false;
        }
    }
}