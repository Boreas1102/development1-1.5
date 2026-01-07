using UnityEngine;

public class LightFader : MonoBehaviour
{
    private Light _light;
    
    [Header("最终达到的亮度")]
    public float targetIntensity = 1.0f; 
    [Header("变亮的速度（建议0.1到0.5之间）")]
    public float fadeSpeed = 0.5f;      

    private bool isFading = false;

    void Start()
    {
        _light = GetComponent<Light>();
        if (_light != null)
        {
            _light.intensity = 0; // 游戏开始时自动黑灯
        }
    }

    void Update()
    {
        if (isFading && _light != null)
        {
            // 平滑数值变化
            _light.intensity = Mathf.MoveTowards(_light.intensity, targetIntensity, fadeSpeed * Time.deltaTime);
            
            // 到达目标后停止计算
            if (Mathf.Approximately(_light.intensity, targetIntensity))
            {
                isFading = false;
            }
        }
    }

    public void StartFadeIn()
    {
        isFading = true;
    }
}