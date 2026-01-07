using UnityEngine;
using StarterAssets;
using System.Collections;
using TMPro; 

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
        
        if (interactionUI != null) interactionUI.SetActive(false);
    }

    void Update()
    {
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
        
        if (interactionUI != null) interactionUI.SetActive(false);

        if (_controller != null) _controller.enabled = false;
        _animator.SetBool("IsClimbing", true);
        _animator.SetFloat("Speed", 1f);

        Vector3 endPos = _targetTopPoint.position;
        while (Vector3.Distance(transform.position, endPos) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, climbSpeed * Time.deltaTime);
            yield return null; 
        }

        StopClimbing();
    }

    public void StopClimbing()
    {
        _isClimbing = false;
        _animator.SetBool("IsClimbing", false);
        _animator.SetFloat("Speed", 0f);

        if (_controller != null) _controller.enabled = true;
        
        if (_isNearLadder && interactionUI != null) interactionUI.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ladderTag))
        {
            _isNearLadder = true;
            _targetTopPoint = other.transform.Find("TopPoint");

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
            
            if (interactionUI != null) 
            {
                interactionUI.SetActive(false);
            }

            if (!_isClimbing) _targetTopPoint = null;
        }
    }
}