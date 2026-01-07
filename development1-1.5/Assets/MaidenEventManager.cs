using UnityEngine;

public class MaidenEventManager : MonoBehaviour
{
    [Header("UI 引用")]
    public GameObject pressEPrompt;   
    public GameObject gemPickupUI;  

    [Header("核心组件")]
    public GameObject gemInside;     
    public Animator animator;        
    public AudioSource audioSource; 

    [Header("音效")]
    public AudioClip openDoorSFX;   
    public AudioClip pickupGemSFX;   

    private bool isPlayerInRange = false;
    private bool isOpened = false;
    private bool isGemPicked = false;

    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isPlayerInRange)
        {
            if (!isOpened && Input.GetKeyDown(KeyCode.E))
            {
                OpenMaiden();
            }
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

        if (animator != null) animator.SetTrigger("OpenMaiden");
        
        if (audioSource != null && openDoorSFX != null)
        {
            audioSource.PlayOneShot(openDoorSFX);
        }
    }

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

        if (pickupGemSFX != null)
        {
            AudioSource.PlayClipAtPoint(pickupGemSFX, transform.position);
        }

        if (gemInside != null)
        {
            Destroy(gemInside);
        }

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
            if (gemPickupUI != null) gemPickupUI.SetActive(false);
        }
    }
}