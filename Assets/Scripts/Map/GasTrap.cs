using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasTrap : MonoBehaviour
{
    public GameObject gasTemplate;

    public int trapNum;

    public float space;

    public float timeInterval;

    public float startTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ReleaseGasCoroutine());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ReleaseGasCoroutine()
    {
        yield return new WaitForSeconds(startTime);
        
        while (true)
        {
            for (int i = 0; i < trapNum; i++)
            {
                Instantiate(gasTemplate, transform.position + space * i * Vector3.right, transform.rotation);
            }
            yield return new WaitForSeconds(timeInterval);
        }
    }
}
