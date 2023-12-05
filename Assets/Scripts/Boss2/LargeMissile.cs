using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeMissile : MonoBehaviour
{
    // Start is called before the first frame update
    public float lifetime;
    public Transform targetPlayer;
    private float timer;
    private float explosiontimer;
    public float speed;
    public float explosiontime;
    public float MaxAngularVelocity;
    public Quaternion rotationOffset;
    private Vector2 target;
    private Vector2 movingDirection;
    private Vector2 targetingDirection;
    private Vector2 position;
    private float startingGoingUpTime = 0.5f;

    public GameObject explosionOnWay;
    public GameObject explosionOnHit;

    public void Start() {
        timer = 0;
        explosiontimer = 0;
        position = transform.position;
    }

    public void Update() {
        if (targetPlayer == null) {
            target = position + Vector2.down;
        } else {
            target = targetPlayer.position;
        }

        
        targetingDirection = (target - position).normalized;
        if (startingGoingUpTime >= 0) { 
            startingGoingUpTime -= Time.deltaTime;
            targetingDirection = (4f * startingGoingUpTime * Vector2.up + targetingDirection).normalized;
        }
        // correct moving direction to target but limit the angular velocity
        if (Vector2.Dot(movingDirection, targetingDirection) < 0.99f) {
            movingDirection = Vector2.Lerp(movingDirection, targetingDirection, MaxAngularVelocity).normalized;
        }
        position += movingDirection * speed * Time.deltaTime;
        transform.position = position;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, movingDirection) * rotationOffset;
        timer += Time.deltaTime;
        explosiontimer += Time.deltaTime;
        if (timer > lifetime) {
            Explode();
            
        }
        if (explosiontimer > explosiontime) {
            ExplodeOnWay();
        }
    }
    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Ground") {
            Explode();
        }
    }
    public void Explode() {
          Instantiate(explosionOnHit, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    public void ExplodeOnWay() {
        Instantiate(explosionOnWay, transform.position, Quaternion.identity);
        explosiontimer = 0;
    }

}
