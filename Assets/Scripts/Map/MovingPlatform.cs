using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2.0f; // �ƶ��ٶ�
    public float maxDistance = 5.0f; // ����ƶ�����

    private Vector3 startPosition;
    private bool movingRight = true; // ��ʼ����

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // ����ƽ̨���ƶ�
        if (movingRight)
        {
            if (transform.position.x < startPosition.x + maxDistance)
            {
                // �����ƶ�
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
                // �����ƶ�
                transform.position += Vector3.left * speed * Time.deltaTime;
            }
            else
            {
                movingRight = true;
            }
        }
    }
}
