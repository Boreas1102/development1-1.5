using UnityEngine;

public class TriggerOtherAnim : MonoBehaviour
{
    [Header("目标物体设置")]
    public Animator targetAnimator;   
    public string animationName;      

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            if (targetAnimator != null)
            {
                targetAnimator.Play(animationName);
                hasTriggered = true; 
                Debug.Log("玩家踩到触发器，目标动画开始播放：" + animationName);
            }
            else
            {
                Debug.LogError("触发器上没有关联 targetAnimator！");
            }
        }
    }
}