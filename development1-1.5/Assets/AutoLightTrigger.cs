using UnityEngine;

public class AutoLightTrigger : MonoBehaviour
{
    [Header("设置要启动的灯光物体")]
    public GameObject[] targetLights; 

    [Header("玩家的标签（确保主角Tag也是Player）")]
    public string playerTag = "Player";

    private bool hasTriggered = false; // 防止重复触发

    private void OnTriggerEnter(Collider other)
    {
        // 1. 检查是否是玩家进入，且还没被触发过
        if (!hasTriggered && other.CompareTag(playerTag))
        {
            Debug.Log("玩家进入触发区，正在启动灯光...");
            
            // 2. 遍历数组中的每一个物体
            foreach (GameObject lightObj in targetLights)
            {
                if (lightObj != null)
                {
                    // 尝试获取该物体上的渐变脚本
                    LightFader fader = lightObj.GetComponent<LightFader>();

                    if (fader != null)
                    {
                        // 如果有渐变脚本，调用它的渐变方法
                        fader.StartFadeIn();
                    }
                    else
                    {
                        // 如果没挂渐变脚本，则直接强制开启（作为备份方案）
                        lightObj.SetActive(true);
                        Debug.LogWarning(lightObj.name + " 上没找到 LightFader 脚本，已直接开启。");
                    }
                }
            }

            // 3. 标记为已触发，避免玩家反复走动导致灯光重复重置
            hasTriggered = true; 
        }
    }
}