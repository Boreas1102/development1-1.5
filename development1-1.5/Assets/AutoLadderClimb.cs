using UnityEngine;
using System.Collections;
using StarterAssets; // 必须引用官方命名空间

public class AutoLadderClimb : MonoBehaviour
{
    [Header("UI & 交互设置")]
    public GameObject promptUI; 
    public KeyCode interactKey = KeyCode.E;

    [Header("攀爬参数")]
    public float climbSpeed = 2.5f;
    // offsetFromLadder.z 用于调整角色与梯子的前后距离，防止穿模
    public float zOffset = -0.5f; 

    private Animator _animator;
    private ThirdPersonController _tpsController;
    private CharacterController _characterController;
    
    private bool _canClimb = false;
    private bool _isClimbing = false;
    private Transform _targetTopPoint;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _tpsController = GetComponent<ThirdPersonController>();
        _characterController = GetComponent<CharacterController>();
        
        if (promptUI != null) promptUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 判定标签，确保你的 Ladder_Trigger 标签为 Ladder
        if (other.CompareTag("Ladder"))
        {
            _canClimb = true;

            // 【优化：深度搜索】在触发器及其所有子物体中寻找名为 TopPoint 的点
            _targetTopPoint = FindChildRecursive(other.transform, "TopPoint");

            if (_targetTopPoint == null)
            {
                Debug.LogError($"[梯子错误] 在 {other.name} 及其子级中找不到名为 'TopPoint' 的物体！");
            }

            if (promptUI != null) promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            _canClimb = false;
            if (promptUI != null) promptUI.SetActive(false);
        }
    }

    void Update()
    {
        // 只有当可以爬、没在爬、且有目标点时才响应按键
        if (_canClimb && !_isClimbing && Input.GetKeyDown(interactKey))
        {
            if (_targetTopPoint != null)
            {
                StartCoroutine(AutoClimbRoutine());
            }
        }
    }

    IEnumerator AutoClimbRoutine()
    {
        _isClimbing = true;
        if (promptUI != null) promptUI.SetActive(false);

        // 1. 彻底禁用官方控制，防止物理系统把人弹走
        _tpsController.enabled = false;
        
        // 2. 播放动画 (确保你 Animator 里的参数名是 IsClimbing)
        _animator.SetBool("IsClimbing", true);

        // 3. 初始位置对齐：将角色水平位置对齐到梯子，并应用 Z 轴偏移
        Vector3 startPos = transform.position;
        Vector3 horizontalTarget = new Vector3(_targetTopPoint.position.x, transform.position.y, _targetTopPoint.position.z);
        // 让角色面向梯子并保持一定距离
        transform.position = horizontalTarget + _targetTopPoint.forward * zOffset;

        // 4. 自动向上平移
        // 只要当前高度小于目标点高度，就继续往上爬
        while (transform.position.y < _targetTopPoint.position.y)
        {
            transform.position += Vector3.up * climbSpeed * Time.deltaTime;
            yield return null; 
        }

        // 5. 到达顶部后的缓冲处理（防止瞬间弹出 Scene）
        _animator.SetBool("IsClimbing", false);
        
        // 强制设置最终坐标到 TopPoint 位置
        transform.position = _targetTopPoint.position;

        // 等待物理引擎和动画状态完全切换
        yield return new WaitForSeconds(0.2f);

        // 6. 重新启用控制
        _tpsController.enabled = true;
        _isClimbing = false;
        
        Debug.Log("攀爬完成，控制已归还。");
    }

    // 工具函数：递归寻找子物体
    private Transform FindChildRecursive(Transform parent, string name)
    {
        if (parent.name == name) return parent;
        foreach (Transform child in parent)
        {
            Transform result = FindChildRecursive(child, name);
            if (result != null) return result;
        }
        return null;
    }
}