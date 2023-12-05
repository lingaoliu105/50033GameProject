using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DronesCircle : MonoBehaviour
{
    public BeamController[] drones;
    public GameObject DronePrefab;
    public WaitForSeconds wait2 = new WaitForSeconds(0.2f);
    public WaitForSeconds wait5 = new WaitForSeconds(0.5f);
    public WaitForSeconds wait10 = new WaitForSeconds(1f);
    private bool launched = false;


    // Start is called before the first frame update
    void Start()
    {
        Generate();
        drones = new BeamController[transform.childCount];

        for (int i = 0; i <= transform.childCount - 1; i++) {
            drones[i] = transform.GetChild(i).GetComponent<BeamController>();
        }
        
    }
    [ContextMenu("Generate")]
    public void Generate() {
// Generating 18 objects on a circle
        for (int i = 0; i <= 17; i++) {
            GameObject drone = Instantiate(DronePrefab, transform.position, Quaternion.identity);
            drone.transform.parent = transform;
            drone.transform.localPosition = new Vector3(Mathf.Cos(i * 20 * Mathf.Deg2Rad), Mathf.Sin(i * 20 * Mathf.Deg2Rad), 0) * (DronePrefab.GetComponent<BeamController>().length/2f+1f);
            drone.transform.localRotation = Quaternion.Euler(0, 0, i * 20+180);
        }

    }

    [ContextMenu("Launch")]
    public void LaunchOneByOne() {
        launched = true;
        StartCoroutine(LaunchCoroutine());
    }

    public IEnumerator LaunchCoroutine() {
        while (launched) {
            int rd = Random.Range(0, 2);
            if (rd == 0) {
                StartCoroutine(LaunchOneByOneCoroutine(true));
            } else {
                StartCoroutine(LaunchOneByOneCoroutine(false));
            }
            yield return wait10;
            yield return wait10;
            yield return wait10;
            yield return wait10;
        }

    }

    public IEnumerator LaunchOneByOneCoroutine(bool Clockwise) {
        for (int i = 0; i <= transform.childCount - 1; i++) {
            if (Clockwise) {
                drones[i].Launch();
            } else {
                drones[transform.childCount - 1 - i].Launch();
            }
            yield return wait2;
        }
    }

    public IEnumerator StopOneByOneCoroutine() {
        for (int i = 0; i <= transform.childCount - 1; i++) {
            drones[i].Stop();
            yield return wait2;
        }
    }
    [ContextMenu("Stop")]
    public void Stop() {
        launched = false;
        StopAllCoroutines();
        for (int i = 0; i <= transform.childCount - 1; i++) {
            drones[i].Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
