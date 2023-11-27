using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlashingOn : MonoBehaviour
{
    public GameObject lamp;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FlashingOn());
    }

    [SerializeField]
    public AnimationCurve LightCurve;

    public float LightIntensity = 1;

    public IEnumerator FlashingOn() {
        float time = 0;
        while (time < 1) {
            time += Time.deltaTime;
            lamp.GetComponent<Light2D>().intensity = LightIntensity * LightCurve.Evaluate(time);
            yield return null;
        }
        lamp.GetComponent<Light2D>().intensity = LightIntensity;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
