using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlashingOn : MonoBehaviour
{
    public AudioSource audioSource;
    public int sampleSize = 1024;  // 频谱采样点数

    private float[] spectrumData;



    public GameObject lamp;
    // Start is called before the first frame update
    void Start()
    {
        spectrumData = new float[sampleSize];
        lamp.GetComponent<Light2D>().intensity = 0;
        StartCoroutine(FlashingOn());
    }

    [SerializeField]
    public AnimationCurve LightCurve;


    public AudioClip loop;
    public float LightIntensity = 1;

    public IEnumerator FlashingOn() {
        float time = 0;
        float length = audioSource.clip.length;
        float amplitude = 0f;
        audioSource.Play();
        while (time < length) {
            time += Time.deltaTime;
            // 获取频谱数据
            audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.Blackman);

            // 计算音频振幅
            amplitude = 0f;
            for (int i = 0; i < sampleSize; i++) {
                amplitude += spectrumData[i];
            }
            amplitude /= sampleSize;

            amplitude = ((Mathf.Log10(amplitude) + 6.2f) / 3.0f - 1f) * 5f + 1f;
            if (time > 1 && amplitude <= 0) { amplitude = 1; }
            if (amplitude >= 1.7f) { break; }
            // Debug.Log("Current Amplitude: " + amplitude);//LightCurve.Evaluate(time)
            // Debug.Log("Current Amplitude: " + amplitude);//LightCurve.Evaluate(time)
            lamp.GetComponent<Light2D>().intensity = LightIntensity * amplitude;
            yield return null;
        }
        audioSource.clip = loop;
        audioSource.loop = true;
        audioSource.volume = 0.1f;
        audioSource.Play();
        for (int i = 0; i < 3; i++) {
            lamp.GetComponent<Light2D>().intensity = LightIntensity * 2.5f;
            yield return null;
        }
        lamp.GetComponent<Light2D>().intensity = LightIntensity;

    }

    // Update is called once per frame
    void Update()
    {
        

        // 在这里使用振幅进行其他操作
        

    }
}
