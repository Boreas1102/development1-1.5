using UnityEngine;
using System.Collections; 

public class CheckTrigger : MonoBehaviour
{
    [Header("移动设置")]
    public Vector3 moveOffset = new Vector3(0, 3, 0); 
    public float speed = 2.0f;                       
    public float waitTime = 10.0f;                   

    private Vector3 closedPosition;                 
    private bool isOpening = false;                  

    void Start()
    {
        closedPosition = transform.position;
        Debug.Log("脚本启动，初始位置已记录");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpening)
        {
            StartCoroutine(OpenAndCloseDoor());
        }
    }

    IEnumerator OpenAndCloseDoor()
    {
        isOpening = true;
        Vector3 openPosition = closedPosition + moveOffset;

        Debug.Log("检测到玩家，正在开门...");
        while (Vector3.Distance(transform.position, openPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, openPosition, speed * Time.deltaTime);
            yield return null; 
        }

        Debug.Log("门已开启，等待 10 秒...");
        yield return new WaitForSeconds(waitTime);

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