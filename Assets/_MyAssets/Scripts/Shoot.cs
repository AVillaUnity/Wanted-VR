using UnityEngine;
using Valve.VR;

public class Shoot : MonoBehaviour
{
    public SteamVR_Behaviour_Pose pose;
    public SteamVR_Input_Sources hand;
    public SteamVR_Action_Single shoot;
    

    public Transform muzzle;
    public Transform head;
    public ObjectPooler bulletSpawner;

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
            if (Physics.Raycast(head.position, head.forward, out RaycastHit hit, 100))
            {
                bullet.GetComponent<Bullet>().SetVelocity(muzzle.forward, pose.GetVelocity(), pose.GetAngularVelocity(), hit.point);
                destinationHit = hit.point;
            }
            else
            {
                bullet.GetComponent<Bullet>().SetVelocity(muzzle.forward, pose.GetVelocity(), pose.GetAngularVelocity(), head.forward * 100);
                destinationHit = head.forward * 100;
            }
        }
    }
}
