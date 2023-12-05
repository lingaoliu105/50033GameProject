using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class BeamController : MonoBehaviour {
    // Start is called before the first frame update
    public Transform beam;
    public Transform beamEnd;
    public Transform beamStart;
    public BoxCollider2D beamCollider;
    public SpriteRenderer WarningArea;
    public int length;
    private bool LockLength;
    // size 1,1,1
    // pos 1.8, 0.8, 0
    // posend 2.8, 0.8, 0
    // offset 1.8, 0.8
    // scale 3, 0.6

    // size 1,2,1
    // pos 2.3, 0.8, 0
    // posend 3.8, 0.8, 0
    // offset 2.3, 0.8
    // scale 4, 0.6

    // size 1,x,1
    // pos 1.3+0.5x, 0.8, 0
    // posend 1.8+x, 0.8, 0
    // offset 1.3+0.5x, 0.8
    // scale 2+x, 0.6

    
    void Start() {
        OnValidate();
        WarningArea.color = new Color(1, 0, 0, 0);
        beamStart.GetComponent<SpriteRenderer>().enabled = false;
        beamEnd.GetComponent<SpriteRenderer>().enabled = false;
        beam.GetComponent<SpriteRenderer>().enabled = false;
        beamCollider.enabled = false;
        LockLength = false;
    }
    public IEnumerator ShowWarningAreaCoroutine() {
        float timer = 0;
        while (timer < 0.2f) {
            timer += Time.deltaTime;
            WarningArea.color = new Color(1, 0, 0, timer * 2.5f);
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        WarningArea.color = new Color(1, 0, 0, 0);
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(LaunchBeam());
    }

    [ContextMenu("Launch")]
    public void Launch() {
        StartCoroutine(ShowWarningAreaCoroutine());
    }

    public IEnumerator LaunchBeam() {
        float timer = 0;
        if (LockLength) {
            yield break;
        }
        LockLength = true;
        yield return null;
        beam.transform.localScale = new Vector3(beam.transform.localScale.x, 0, 1);
        beamStart.transform.localScale = new Vector3(beamStart.transform.localScale.x, 0, 1);
        beamEnd.transform.localScale = new Vector3(beamEnd.transform.localScale.x, 0, 1);
        beamStart.GetComponent<SpriteRenderer>().enabled = true;
        beamEnd.GetComponent<SpriteRenderer>().enabled = true;
        beam.GetComponent<SpriteRenderer>().enabled = true;
        while (timer < 0.2f) {
            timer += Time.deltaTime;
            beam.transform.localScale = new Vector3(beam.transform.localScale.x, timer * 5, 1);
            beamStart.transform.localScale = new Vector3(beamStart.transform.localScale.x, timer * 5, 1);
            beamEnd.transform.localScale = new Vector3(beamEnd.transform.localScale.x, timer * 5, 1);
            yield return null;
        }
        beamCollider.enabled = true;
        LockLength = false;
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(StopBeam());
    }
    [ContextMenu("Stop")]
    public void Stop() {
        StartCoroutine(StopBeam());
    }
    public IEnumerator StopBeam() {
        float timer = 0;
        if (LockLength) {
            yield break;
        }
        LockLength = true;
        yield return null;
        beamCollider.enabled = false;
        while (timer < 0.2f) {
            timer += Time.deltaTime;
            beam.transform.localScale = new Vector3(beam.transform.localScale.x, 1f - timer * 5, 1);
            beamStart.transform.localScale = new Vector3(beamStart.transform.localScale.x, 1f - timer * 5, 1);
            beamEnd.transform.localScale = new Vector3(beamEnd.transform.localScale.x, 1f - timer * 5, 1);
            yield return null;
        }
        beamStart.GetComponent<SpriteRenderer>().enabled = false;
        beamEnd.GetComponent<SpriteRenderer>().enabled = false;
        beam.GetComponent<SpriteRenderer>().enabled = false;
        
        LockLength = false;
    }

    [ExecuteAlways]
    private void OnValidate() {
        if (LockLength) {
            return;
        }
        WarningArea.color = new Color(1, 0, 0, 0);
        beamStart.GetComponent<SpriteRenderer>().enabled = false;
        beamEnd.GetComponent<SpriteRenderer>().enabled = false;
        beam.GetComponent<SpriteRenderer>().enabled = false;
        beamCollider.enabled = false;
        WarningArea.transform.localScale = new Vector3(length+2, 1, 1);
        WarningArea.transform.localPosition = new Vector3(2f + length / 2f, 0.8f, 0);
        beam.transform.localScale = new Vector3(length, 1, 1);
        beam.transform.localPosition = new Vector3(1.3f + length / 2f, 0.8f, 0);
        beamEnd.transform.localPosition = new Vector3(1.8f + length, 0.8f, 0);
        beamCollider.size = new Vector2(length + 2f, 0.6f);
        beamCollider.offset = new Vector2(1.3f + length / 2f, 0.8f);
    }

    // Update is called once per frame
    void Update() {

    }
}
