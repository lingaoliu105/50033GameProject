using Assets.Scripts.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector2 p0;
    private Vector2 p2;
    private Vector2 p1;
    public float BeforeLaunchingSpeed;
    public float afterLaunchingSpeed;
    public float Lifetime;
    public Transform targetPlayer;
    private float timer;
    private float percentage;
    public Quaternion rotationOffset;

    private Vector2 StartDirection;

    public int currentPhase;

    public GameObject explosion;
    void Start()
    {
        timer = 0;
        currentPhase = 0;
        this.GetComponent<SpriteRenderer>().enabled = false;
        //this.transform.GetChild(0).gameObject.SetActive(false);
        StartCoroutine(Init());
    }
    public IEnumerator Init() {
        yield return null;
        StartDirection = BeforeLaunchingSpeed * Vector2.right;
        // randomize a very small rotation
        this.GetComponent<SpriteRenderer>().enabled = true;
        while (currentPhase == 0 && timer < Lifetime) { 
            transform.position += (Vector3)StartDirection * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, StartDirection))*rotationOffset;
            timer += Time.deltaTime;
            yield return null;
        }
        if (currentPhase == 0 && timer >= Lifetime) {
            Destroy(gameObject);
        }
    }
    public Vector2 SetP1() {
        float rd0 = Random.Range(0.3f, 0.7f);
        Vector2 m = Vector2.Lerp(p0, p2, rd0);
        //Debug.DrawLine(p0, m, Color.red, 1f);
        Vector2 normal = Vector2.Perpendicular(p0 - p2).normalized;
        //Debug.DrawLine(m, m + normal, Color.red, 1f);
        float rd1 = Random.Range(-1f, 1f);
        float cur = rd1 * 0.6f;
        //Debug.DrawLine(m, m + normal * cur * (p2-p0).magnitude, Color.green, 1f);
        return  m + normal * cur * (p2-p0).magnitude ;
    }


    IEnumerator LaunchCoroutine() {
        yield return null;
        GetComponent<CircleCollider2D>().enabled = true;
        p0 = transform.position;
        p2 = targetPlayer.transform.position;
        p1 = SetP1();
        Lifetime = (p2 - p0).magnitude / afterLaunchingSpeed;
        timer = 0;
        while (timer < Lifetime) {
            percentage = timer / Lifetime;
            Vector2 p = Bezier.BezierPoint(percentage, p0, p1, p2);
            transform.position = p;
            transform.rotation = Bezier.BezierRotation(percentage, p0, p1, p2) * rotationOffset;
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

    public void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(p0, 0.1f);
        Gizmos.DrawSphere(p1, 0.1f);
        Gizmos.DrawSphere(p2, 0.1f);
        Gizmos.DrawLine(p0, p1);
        Gizmos.DrawLine(p1, p2);
    }

    private void OnDestroy() {
        Instantiate(explosion, transform.position, Quaternion.identity);
    }
    
    public void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log(other.gameObject);
        if (other.gameObject.CompareTag("Player")) {
            if (currentPhase == 1) {
                Destroy(gameObject);
            }
        }
    }


    // Update is called once per frame
    void Update(){
        if (currentPhase == 0) {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 4f);
            foreach (Collider2D collider in colliders) {
                if (collider.gameObject.CompareTag("Player")) {
                    GetComponent<Collider2D>().enabled = false;
                    currentPhase = 1;
                    targetPlayer = collider.transform;
                    StartCoroutine(LaunchCoroutine());
                    return;
                }
            }
        }
    }
}
