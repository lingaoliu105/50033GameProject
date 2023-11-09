using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2.0f; // 移动速度
    public float maxDistance = 5.0f; // 最大移动距离

    private Vector3 startPosition;
    private bool movingRight = true; // 初始方向

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // 计算平台的移动
        if (movingRight)
        {
            if (transform.position.x < startPosition.x + maxDistance)
            {
                // 向右移动
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
            else
            {
                movingRight = false;
            }
        }
        else
        {
            if (transform.position.x > startPosition.x - maxDistance)
            {
                // 向左移动
                transform.position += Vector3.left * speed * Time.deltaTime;
            }
            else
            {
                movingRight = true;
            }
        }
    }
}
