using UnityEngine;

public class SimpleChest : MonoBehaviour
{
    [Header("设置")]
    public Animator animator;     
    public string triggerName = "OpenChest"; 
    public AudioSource audioSource; 
    public AudioClip openSFX;      

    private bool isOpened = false; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpened)
        {
            OpenTheChest();
        }
    }

    void OpenTheChest()
    {
        isOpened = true;

        if (animator != null)
        {
            animator.SetTrigger(triggerName);
        }

        if (audioSource != null && openSFX != null)
        {
            audioSource.PlayOneShot(openSFX);
        }

        Debug.Log("宝箱开启成功！");
    }
}