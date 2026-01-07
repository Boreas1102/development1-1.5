using UnityEngine;
using System.Collections; // 必须保留，否则无法使用协程

public class CheckTrigger : MonoBehaviour
{
    [Header("移动设置")]
    public Vector3 moveOffset = new Vector3(0, 3, 0); // 向上移动3米
    public float speed = 2.0f;                       // 移动速度
    public float waitTime = 10.0f;                   // 停留时间

    private Vector3 closedPosition;                  // 初始位置
    private bool isOpening = false;                  // 防止重复触发

    void Start()
    {
        // 记录游戏开始时门的位置
        closedPosition = transform.position;
        Debug.Log("脚本启动，初始位置已记录");
    }

    void OnTriggerEnter(Collider other)
    {
        // 只有当玩家进入且门没在动时才开门
        if (other.CompareTag("Player") && !isOpening)
        {
            StartCoroutine(OpenAndCloseDoor());
        }
    }

    // 这是一个协程，负责：开门 -> 等待 -> 关门
    IEnumerator OpenAndCloseDoor()
    {
        isOpening = true;
        Vector3 openPosition = closedPosition + moveOffset;

        // 1. 开门逻辑
        Debug.Log("检测到玩家，正在开门...");
        while (Vector3.Distance(transform.position, openPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, openPosition, speed * Time.deltaTime);
            yield return null; // 等待下一帧再继续循环
        }

        // 2. 等待 10 秒
        Debug.Log("门已开启，等待 10 秒...");
        yield return new WaitForSeconds(waitTime);

        // 3. 关门逻辑
        Debug.Log("时间到，正在关门...");
        while (Vector3.Distance(transform.position, closedPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, closedPosition, speed * Time.deltaTime);
            yield return null;
        }

        isOpening = false;
        Debug.Log("门已复位");
    }
}