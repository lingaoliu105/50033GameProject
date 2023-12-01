using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronesController : MonoBehaviour
{
    public BeamController[] drones;
    // Start is called before the first frame update
    void Start()
    {
        drones = new BeamController[transform.childCount];
        for (int i = 0; i <= transform.childCount - 1; i++) {
            drones[i] = transform.GetChild(i).GetComponent<BeamController>();
        }
        
    }
    [ContextMenu("Launch")]
    public void Launch() {
        foreach (BeamController drone in drones) {
            drone.Launch();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
