using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{

    public float speed = 2f;
    public float curveSpeed = 2f;
    

    private Rigidbody rb;
    private ObjectPooler bulletSpawner;

    private bool shot = false;
    private bool collided = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bulletSpawner = GetComponentInParent<ObjectPooler>();
    }

    private void OnEnable()
    {
        if (!shot) { return; }

        Invoke("SetInactive", 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if(!collided)
            transform.rotation = Quaternion.LookRotation(rb.velocity, Vector3.up);
    }

    public void SetVelocity(Vector3 forward, Vector3 armVelocity, Vector3 armRotation, Vector3 destination)
    {

        rb.velocity = armVelocity;
        rb.angularVelocity = armRotation;
        StartCoroutine(CurveBullet(destination));
        shot = true;
    }

    IEnumerator CurveBullet(Vector3 destination)
    {
        float initialDistance = Vector3.Distance(transform.position, destination);
        Vector3 initialVelocity = rb.velocity;

        float lerpValue = 0;
        while(lerpValue < .9f && lerpValue >= 0f)
        {
            rb.velocity = Vector3.Lerp(initialVelocity, (destination - transform.position) * speed, lerpValue);
            lerpValue = 1 - Vector3.Distance(transform.position, destination) / initialDistance;
            yield return null;
        }
    }

    public void SetVelocity(Vector3 forward)
    {
        rb.velocity = forward * speed;
        shot = true;
    }

    void SetInactive()
    {
        shot = false;
        rb.velocity = Vector3.zero;
        collided = false;
        rb.useGravity = false;
        transform.parent = bulletSpawner.inactiveParent;
        this.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        collided = true;
        rb.useGravity = true;
        StopAllCoroutines();
    }
}
