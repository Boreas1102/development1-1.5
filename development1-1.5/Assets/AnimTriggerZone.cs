using UnityEngine;

public class TriggerOtherAnim : MonoBehaviour
{
    [Header("目标物体设置")]
    public Animator targetAnimator;   // 拖入你想播放动画的那个物体（比如铁处女）
    public string animationName;      // 填入 Animator 里的方块名字

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // 1. 检查是不是玩家进来了
        // 2. 检查是不是还没触发过
        if (other.CompareTag("Player") && !hasTriggered)
        {
            if (targetAnimator != null)
            {
                // 让“旁边”的物体播放动画
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