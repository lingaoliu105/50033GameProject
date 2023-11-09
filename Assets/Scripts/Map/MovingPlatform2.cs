using UnityEngine;

public class MovingPlatform2 : MonoBehaviour
{
    public float speed = 2.0f; // 平台的移动速度
    public float maxDistance = 5.0f; // 平台移动的最大高度

    private Vector3 startPosition;
    private bool movingUp = true; // 是否正在向上移动

    void Start()
    {
        startPosition = transform.position; // 记录初始位置
    }

    void Update()
    {
        // 根据平台的移动方向和最大移动距离，计算新位置
        if (movingUp)
        {
            if (transform.position.y < startPosition.y + maxDistance)
            {
                // 向上移动
                transform.position += Vector3.up * speed * Time.deltaTime;
            }
            else
            {
                movingUp = false;
            }
        }
        else
        {
            if (transform.position.y > startPosition.y)
            {
                // 向下移动
                transform.position -= Vector3.up * speed * Time.deltaTime;
            }
            else
            {
                movingUp = true;
            }
        }
    }
}
