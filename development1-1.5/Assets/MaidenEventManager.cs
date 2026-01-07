using UnityEngine;

public class MaidenEventManager : MonoBehaviour
{
    [Header("UI 引用")]
    public GameObject pressEPrompt;   // “按 E 开启”提示 (PressEChest)
    public GameObject gemPickupUI;   // 宝石拾取界面 (Gemstone)

    [Header("核心组件")]
    public GameObject gemInside;     // 内部的宝石模型 (SM_Prop_Gem_01)
    public Animator animator;        // 铁处女的 Animator
    public AudioSource audioSource;  // 铁处女的 AudioSource

    [Header("音效")]
    public AudioClip openDoorSFX;    // 开门声 (iron_door1_O)
    public AudioClip pickupGemSFX;   // 拾取声 (poka02)

    private bool isPlayerInRange = false;
    private bool isOpened = false;
    private bool isGemPicked = false;

    void Start()
    {
        // 自动纠错：如果面板没挂 Animator，尝试从自己身上找
        if (animator == null) animator = GetComponent<Animator>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isPlayerInRange)
        {
            // 第一阶段：按下 E 键开启铁处女
            if (!isOpened && Input.GetKeyDown(KeyCode.E))
            {
                OpenMaiden();
            }
            // 第二阶段：如果 UI 已经弹出，按下 E 键拾取宝石
            else if (isOpened && !isGemPicked && gemPickupUI.activeSelf && Input.GetKeyDown(KeyCode.E))
            {
                PickupGem();
            }
        }
    }

    void OpenMaiden()
    {
        isOpened = true;
        if (pressEPrompt != null) pressEPrompt.SetActive(false); 

        // 1. 触发开门动画
        if (animator != null) animator.SetTrigger("OpenMaiden");
        
        // 2. 播放开门音效
        if (audioSource != null && openDoorSFX != null)
        {
            audioSource.PlayOneShot(openDoorSFX);
        }
    }

    // --- 核心修复：对应你报错信息的动画事件函数 ---
    // 必须为 public，名称必须完全一致
    public void OnMaidenOpenAnimationFinished()
    {
        Debug.Log("动画暗号对接成功：弹出拾取 UI");
        if (gemPickupUI != null)
        {
            gemPickupUI.SetActive(true);
        }
    }

    void PickupGem()
    {
        isGemPicked = true;
        if (gemPickupUI != null) gemPickupUI.SetActive(false);

        // 播放拾取音效
        if (pickupGemSFX != null)
        {
            // 使用这个方法可以确保即使脚本禁用，声音也能播完
            AudioSource.PlayClipAtPoint(pickupGemSFX, transform.position);
        }

        // 销毁宝石模型
        if (gemInside != null)
        {
            Destroy(gemInside);
        }

        // 交互彻底结束，禁用脚本防止重复触发
        this.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isGemPicked)
        {
            isPlayerInRange = true;
            if (!isOpened && pressEPrompt != null) pressEPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (pressEPrompt != null) pressEPrompt.SetActive(false);
            // 如果玩家走远，自动关闭拾取界面
            if (gemPickupUI != null) gemPickupUI.SetActive(false);
        }
    }
}