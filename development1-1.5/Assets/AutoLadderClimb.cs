using UnityEngine;
using StarterAssets;
using System.Collections;
using TMPro; // 如果你使用的是 TextMeshPro，需要这一行

public class AutoLadderClimberWithUI : MonoBehaviour
{
    private Animator _animator;
    private ThirdPersonController _controller;
    private bool _isNearLadder = false;
    private bool _isClimbing = false;
    private Transform _targetTopPoint;

    [Header("UI 设置")]
    [Tooltip("将场景中的交互提示 UI (GameObject) 拖到这里")]
    public GameObject interactionUI; 

    [Header("爬行设置")]
    public float climbSpeed = 2.0f;
    public string ladderTag = "Ladder";

    void Start()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<ThirdPersonController>();
        
        // 游戏开始时确保 UI 是隐藏的
        if (interactionUI != null) interactionUI.SetActive(false);
    }

    void Update()
    {
        // 只有靠近梯子且没在爬的时候，按下 E 开始
        if (_isNearLadder && Input.GetKeyDown(KeyCode.E) && !_isClimbing)
        {
            StartCoroutine(AutoClimbRoutine());
        }
    }

    private IEnumerator AutoClimbRoutine()
    {
        if (_targetTopPoint == null)
        {
            Debug.LogError("未找到 TopPoint！请检查梯子子物体。");
            yield break;
        }

        _isClimbing = true;
        
        // 开始爬行时隐藏 UI 提示
        if (interactionUI != null) interactionUI.SetActive(false);

        // 1. 准备状态
        if (_controller != null) _controller.enabled = false;
        _animator.SetBool("IsClimbing", true);
        _animator.SetFloat("Speed", 1f);

        // 2. 自动位移
        Vector3 endPos = _targetTopPoint.position;
        while (Vector3.Distance(transform.position, endPos) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, climbSpeed * Time.deltaTime);
            yield return null; 
        }

        // 3. 完成
        StopClimbing();
    }

    public void StopClimbing()
    {
        _isClimbing = false;
        _animator.SetBool("IsClimbing", false);
        _animator.SetFloat("Speed", 0f);

        if (_controller != null) _controller.enabled = true;
        
        // 爬完后如果还在触发器内，重新显示 UI
        if (_isNearLadder && interactionUI != null) interactionUI.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ladderTag))
        {
            _isNearLadder = true;
            _targetTopPoint = other.transform.Find("TopPoint");

            // 显示交互 UI
            if (interactionUI != null && !_isClimbing) 
            {
                interactionUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(ladderTag))
        {
            _isNearLadder = false;
            
            // 隐藏交互 UI
            if (interactionUI != null) 
            {
                interactionUI.SetActive(false);
            }

            if (!_isClimbing) _targetTopPoint = null;
        }
    }
}