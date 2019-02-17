using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{

    public float speed = 2f;
    public AudioClip impactHit;

    private Rigidbody rb;
    private ObjectPooler bulletSpawner;
    private AudioSource audioSource;
    

    private bool collided = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        bulletSpawner = GetComponentInParent<ObjectPooler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!collided)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity, Vector3.up);
        }
    }

    public void SetVelocity(Vector3 forward, Vector3 armVelocity, Vector3 armRotation, Vector3 destination)
    {
        rb.velocity = armVelocity;
        StartCoroutine(CurveBullet(destination));
        audioSource.PlayDelayed(.2f);
    }

    IEnumerator CurveBullet(Vector3 destination)
    {
        float initialDistance = Vector3.Distance(transform.position, destination);
        Vector3 initialVelocity = rb.velocity;

        float lerpValue = 0;
        while(lerpValue < 1f && lerpValue >= 0f)
        {
            Vector3 directionToChange = (destination - transform.position) * speed;
            rb.velocity = Vector3.Lerp(initialVelocity, directionToChange, lerpValue);
            if (rb.velocity.sqrMagnitude != speed * speed)
                rb.velocity = rb.velocity.normalized * speed;
            lerpValue = 1 - Vector3.Distance(transform.position, destination) / initialDistance;
            yield return null;
        }
    }

    public void SetVelocity(Vector3 forward)
    {
        rb.velocity = forward * speed;
        audioSource.PlayDelayed(.2f);
    }

    void SetInactive()
    {
        rb.velocity = Vector3.zero;
        collided = false;
        rb.useGravity = false;
        transform.parent = bulletSpawner.inactiveParent;
        this.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collided) {
            audioSource.Stop();
            AudioSource.PlayClipAtPoint(impactHit, transform.position);
        }
        collided = true;
        rb.useGravity = true;
        StopAllCoroutines();
        Invoke("SetInactive", 5f);
    }
}
