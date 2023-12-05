using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Map {
    public class UnderGroundDoor:MapObjectController {
    // Start is called before the first frame update
    public GameObject door;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

        public override void Interact(int triggerId) {
            if(triggerId==0){
            StartCoroutine(Disappear());
            }
            else{
                return;
            }
        }

        IEnumerator Disappear()
        {
            int n = 8;
            for (int i = 0; i < n; i++)
            {
                door.GetComponent<Renderer>().material.color -= new Color(0, 0, 0, 1f / n);
                yield return new WaitForSeconds(0.05f);
            }

            door.SetActive(false);
        }
    }
}