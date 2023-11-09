using UnityEngine;

public class MovingPlatform2 : MonoBehaviour
{
    public float speed = 2.0f; // ƽ̨���ƶ��ٶ�
    public float maxDistance = 5.0f; // ƽ̨�ƶ������߶�

    private Vector3 startPosition;
    private bool movingUp = true; // �Ƿ����������ƶ�

    void Start()
    {
        startPosition = transform.position; // ��¼��ʼλ��
    }

    void Update()
    {
        // ����ƽ̨���ƶ����������ƶ����룬������λ��
        if (movingUp)
        {
            if (transform.position.y < startPosition.y + maxDistance)
            {
                // �����ƶ�
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
                // �����ƶ�
                transform.position -= Vector3.up * speed * Time.deltaTime;
            }
            else
            {
                movingUp = true;
            }
        }
    }
}
