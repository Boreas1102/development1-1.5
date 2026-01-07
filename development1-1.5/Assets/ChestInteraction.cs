using UnityEngine;

public class ChestInteraction : MonoBehaviour
{
    [Header("UI 引用")]
    public GameObject pressEPrompt;   
    public GameObject itemPickupUI;  

    [Header("交互物体")]
    public GameObject gemModel;      

    [Header("组件")]
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip openSFX;
    public AudioClip pickupSFX;      

    private bool isPlayerInRange = false;
    private bool isOpened = false;
    private bool isItemShown = false;

    void Update()
    {
        if (isPlayerInRange)
        {
            if (!isOpened && Input.GetKeyDown(KeyCode.E))
            {
                OpenChest();
            }
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

        Invoke("ShowItemUI", 0.5f); 
    }

    void ShowItemUI()
    {
        itemPickupUI.SetActive(true);
        isItemShown = true;
    }

    void PickupAndDestroy()
    {
        if (audioSource != null && pickupSFX != null)
        {
            AudioSource.PlayClipAtPoint(pickupSFX, transform.position);
        }

        itemPickupUI.SetActive(false);

        if (gemModel != null)
        {
            Destroy(gemModel); 
        }

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