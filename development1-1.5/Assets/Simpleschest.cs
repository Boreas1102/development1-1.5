using UnityEngine;

public class SimpleChest : MonoBehaviour
{
    [Header("设置")]
    public Animator animator;      // 拖入宝箱的 Animator
    public string triggerName = "OpenChest"; // 必须和 Animator 里的参数名一致
    public AudioSource audioSource; // 宝箱自带的音效组件
    public AudioClip openSFX;      // 你下载的二次元音效

    private bool isOpened = false; // 确保只能开一次

    private void OnTriggerEnter(Collider other)
    {
        // 检查进来的是不是玩家，且宝箱还没开
        if (other.CompareTag("Player") && !isOpened)
        {
            OpenTheChest();
        }
    }

    void OpenTheChest()
    {
        isOpened = true;

        // 1. 触发动画
        if (animator != null)
        {
            animator.SetTrigger(triggerName);
        }

        // 2. 播放二次元音效
        if (audioSource != null && openSFX != null)
        {
            audioSource.PlayOneShot(openSFX);
        }

        Debug.Log("宝箱开启成功！");
    }
}