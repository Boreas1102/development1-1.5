using UnityEngine;

public class SimpleAutoAnim : MonoBehaviour
{
    [Header("设置")]
    public Animator animator; 
    public string MaidenOpen; 

    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasPlayed)
        {
            if (animator != null)
            {
                animator.Play(MaidenOpen);
                hasPlayed = true; 
                Debug.Log("触发成功，播放动画：" + MaidenOpen);
            }
        }
    }
}