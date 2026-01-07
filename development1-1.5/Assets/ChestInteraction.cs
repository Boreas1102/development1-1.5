using UnityEngine;

public class ChestInteraction : MonoBehaviour
{
    [Header("UI 引用")]
    public GameObject pressEPrompt;   // “按 E 开启”提示
    public GameObject itemPickupUI;  // 宝石拾取界面

    [Header("交互物体")]
    public GameObject gemModel;      // 宝箱里那个看得见的宝石模型

    [Header("组件")]
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip openSFX;
    public AudioClip pickupSFX;      // 拾取音效

    private bool isPlayerInRange = false;
    private bool isOpened = false;
    private bool isItemShown = false;

    void Update()
    {
        if (isPlayerInRange)
        {
            // 阶段 1：开箱
            if (!isOpened && Input.GetKeyDown(KeyCode.E))
            {
                OpenChest();
            }
            // 阶段 2：拾取并让宝石消失
            else if (isOpened && isItemShown && Input.GetKeyDown(KeyCode.E))
            {
                PickupAndDestroy();
            }
        }
    }

    void OpenChest()
    {
        isOpened = true;
        pressEPrompt.SetActive(false);

        if (animator != null) animator.SetTrigger("OpenChest");
        if (audioSource != null && openSFX != null) audioSource.PlayOneShot(openSFX);

        // 延迟显示拾取 UI（等箱子盖子打开一点）
        Invoke("ShowItemUI", 0.5f); 
    }

    void ShowItemUI()
    {
        itemPickupUI.SetActive(true);
        isItemShown = true;
    }

    void PickupAndDestroy()
    {
        // 1. 播放拾取音效
        if (audioSource != null && pickupSFX != null)
        {
            // 注意：如果直接销毁物体，声音会立刻中断
            // 所以我们用 PlayClipAtPoint 在位置播放，或者只销毁模型
            AudioSource.PlayClipAtPoint(pickupSFX, transform.position);
        }

        // 2. 隐藏 UI
        itemPickupUI.SetActive(false);

        // 3. 核心：销毁宝石模型
        if (gemModel != null)
        {
            Destroy(gemModel); 
        }

        // 4. 彻底禁用此交互脚本，防止再次按 E 触发
        this.enabled = false;
        
        Debug.Log("宝石已拾取并消失");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isItemShown)
        {
            isPlayerInRange = true;
            if (!isOpened) pressEPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            pressEPrompt.SetActive(false);
            itemPickupUI.SetActive(false);
        }
    }
}