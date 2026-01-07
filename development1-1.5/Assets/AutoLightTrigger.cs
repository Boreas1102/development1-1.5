using UnityEngine;

public class AutoLightTrigger : MonoBehaviour
{
    [Header("设置要启动的灯光物体")]
    public GameObject[] targetLights; 

    [Header("玩家的标签（确保主角Tag也是Player）")]
    public string playerTag = "Player";

    private bool hasTriggered = false; 

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag(playerTag))
        {
            Debug.Log("玩家进入触发区，正在启动灯光...");
            
            foreach (GameObject lightObj in targetLights)
            {
                if (lightObj != null)
                {
                    LightFader fader = lightObj.GetComponent<LightFader>();

                    if (fader != null)
                    {
                        fader.StartFadeIn();
                    }
                    else
                    {
                        lightObj.SetActive(true);
                        Debug.LogWarning(lightObj.name + " 上没找到 LightFader 脚本，已直接开启。");
                    }
                }
            }

            hasTriggered = true; 
        }
    }
}