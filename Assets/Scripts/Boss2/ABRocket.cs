using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABRocket : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector2 p0;
    private Vector2 p2;
    private Vector2 p1;
    public float t;
    public Vector2 target;
    private float timer;
    private float percentage;
    public Quaternion rotationOffset;

    public GameObject explosion;
    void Start()
    {
        timer = 0;
        this.GetComponent<SpriteRenderer>().enabled = false;
        //this.transform.GetChild(0).gameObject.SetActive(false);
        StartCoroutine(Init());
    }
    public IEnumerator Init() {
        yield return null;
        p0 = transform.position;
        p2 = target;
        p1 = SetP1();
        transform.position = p0;
        transform.rotation = Bezier.BezierRotation(0, p0, p1, p2) * rotationOffset;
        this.GetComponent<SpriteRenderer>().enabled = true;
        //this.transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(LaunchCoroutine());
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
        while (timer < t) {
            percentage = timer / t;
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
        if (other.gameObject.CompareTag("Player")) {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update(){}
}
