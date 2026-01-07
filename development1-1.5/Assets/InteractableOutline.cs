using UnityEngine;

public class InteractableOutline : MonoBehaviour
{
    private Outline outline;

    void Awake()
    {
        // 自动获取或添加 Outline 组件
        outline = GetComponent<Outline>();
        if (outline == null)
        {
            outline = gameObject.AddComponent<Outline>();
        }

        // 初始化设置：默认关闭
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.yellow; // 你可以改成喜欢的颜色
        outline.OutlineWidth = 5f;
        outline.enabled = false; 
    }

    // 提供给外部调用的开关函数
    public void ShowOutline(bool state)
    {
        if (outline != null)
        {
            outline.enabled = state;
        }
    }
}