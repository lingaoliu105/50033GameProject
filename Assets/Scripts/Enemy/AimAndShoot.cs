using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy {
    public class AimAndShoot: MonoBehaviour {
        public Vector2 StartPos;
        public Vector2 EndPos;
        public GameObject Crosshair;
        public GameObject Bullet;
        public float BulletFlyingTime;    
        public void Start() {
            StartCoroutine(LaunchBullet());
        }
        public IEnumerator LaunchBullet() {
            yield return null;
            GameObject crosshair = Instantiate(Crosshair, EndPos, Quaternion.identity);
            crosshair.GetComponent<Crosshair>().Show();
            yield return new WaitForSeconds(0.5f);

            Quaternion bulletDirection = Quaternion.FromToRotation(Vector3.up, EndPos - StartPos);
            GameObject bullet = Instantiate(Bullet, StartPos, bulletDirection);
            bullet.GetComponent<AimingBullet>().initialVelocity = (EndPos - StartPos)/ BulletFlyingTime;
            bullet.transform.parent = this.transform;
        }
    }

}
