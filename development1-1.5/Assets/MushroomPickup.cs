using UnityEngine;
using System.Collections;

public class MushroomPickup : MonoBehaviour
{
    public GameObject uiPrompt;       // 之前关联的UI
    public GameObject pickupEffect;   // 拖入你的粒子特效Prefab
    public AudioClip pickupSound;     // 拖入你的音效文件
    public float fadeDuration = 1.0f; // 消逝持续时间

    private bool isPlayerInRange = false;
    private bool isPickedUp = false;
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
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
        
        // 1. 隐藏 UI 提示
        if (uiPrompt != null) uiPrompt.SetActive(false);

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
        if (other.CompareTag("Player") && !isPickedUp)
        {
            isPlayerInRange = true;
            if (uiPrompt != null) uiPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (uiPrompt != null) uiPrompt.SetActive(false);
        }
    }
}