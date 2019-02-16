using UnityEngine;
using Valve.VR;

public class Shoot : MonoBehaviour
{
    public SteamVR_Behaviour_Pose pose;
    public SteamVR_Input_Sources hand;
    public SteamVR_Action_Single shoot;
    public SteamVR_Action_Vibration haptic;

    public Transform muzzle;
    public Transform head;
    public ObjectPooler bulletSpawner;
    public Animator animator;
    public LayerMask layerMask;

    private bool canShoot = true;
    private Vector3 destinationHit;

    // Update is called once per frame
    void Update()
    {
        if(shoot.GetAxis(hand) < 0.2)
        {
            canShoot = true;
        }

        if(shoot.GetAxis(hand) >= 0.2 && canShoot)
        {
            SpawnBullet();
            animator.SetTrigger("Shoot");
            haptic.Execute(0f, .1f, 100f, 1f, hand);
            canShoot = false;
        }
    }

    private void SpawnBullet()
    {
        GameObject bullet = bulletSpawner.GetObject();
        bullet.transform.position = muzzle.position;
        bullet.transform.rotation = muzzle.rotation;
        bullet.transform.parent = bulletSpawner.activeParent;
        bullet.SetActive(true);
        SetBulletVelocity(bullet);
    }

    private void SetBulletVelocity(GameObject bullet)
    {
        if(pose.GetVelocity().magnitude <= .5f)
        {
            bullet.GetComponent<Bullet>().SetVelocity(muzzle.forward);
        }
        else
        {
            
            foreach(Transform t in head.gameObject.GetComponentsInChildren<Transform>())
            {
                if (Physics.Raycast(t.position, t.forward, out RaycastHit targetHit, 500f, layerMask))
                {
                    // hit target collider
                    bullet.GetComponent<Bullet>().SetVelocity(muzzle.forward, pose.GetVelocity(), pose.GetAngularVelocity(), targetHit.point);
                    destinationHit = targetHit.point;
                    print("hitting target from " + t.name);
                    return;
                }
            }
            
            // no target hit
            bullet.GetComponent<Bullet>().SetVelocity(muzzle.forward, pose.GetVelocity(), pose.GetAngularVelocity(), head.forward * 100);
            destinationHit = head.forward * 100;
            print("hitting nothing");
        }
    }
}
