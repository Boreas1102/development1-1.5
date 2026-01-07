using UnityEngine;
using StarterAssets; // 必须引用官方命名空间

public class LadderController : MonoBehaviour
{
    private Animator _animator;
    private ThirdPersonController _tpsController;
    private bool _isClimbing = false;

    [Header("爬行设置")]
    public float climbSpeed = 3f;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _tpsController = GetComponent<ThirdPersonController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            StartClimbing();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            StopClimbing();
        }
    }

    void StartClimbing()
    {
        _isClimbing = true;
        _animator.SetBool("IsClimbing", true);
        
        // 暂时禁用重力影响，防止爬的时候往下滑
        // 注意：官方脚本中 Gravity 是私有的，你可能需要微调官方脚本或在此强制接管位移
    }

    void StopClimbing()
    {
        _isClimbing = false;
        _animator.SetBool("IsClimbing", false);
    }

    void Update()
    {
        if (_isClimbing)
        {
            // 获取玩家的前后输入 (W/S)
            float verticalInput = Input.GetAxis("Vertical");
            
            if (Mathf.Abs(verticalInput) > 0.1f)
            {
                // 手动控制 Y 轴移动
                Vector3 climbMove = new Vector3(0, verticalInput * climbSpeed * Time.deltaTime, 0);
                transform.position += climbMove;
            }
        }
    }
}