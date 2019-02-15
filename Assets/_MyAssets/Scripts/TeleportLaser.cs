using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TeleportLaser : MonoBehaviour
{

    public SteamVR_Input_Sources hand;
    public SteamVR_Behaviour_Pose pose;
    public SteamVR_Action_Boolean teleportAction;
    public LayerMask teleportLayer;

    public GameObject laserPrefab;
    public GameObject reticlePrefab;
    public Transform cameraRigTransform;
    public Transform headTransform;
    public float reticleOffset = 2f;

    private GameObject reticleReference;
    private GameObject laserReference;
    private Transform laserTransfrom;
    private Transform reticleTransform;
    private RaycastHit hitInfo;

    private void Start()
    {
        reticleReference = Instantiate(reticlePrefab);
        laserReference = Instantiate(laserPrefab);
        reticleTransform = reticleReference.transform;
        laserTransfrom = laserReference.transform;
        laserReference.SetActive(false);
        reticleReference.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (teleportAction.GetState(hand))
        {
            ActivateLaser();
        }

        if (teleportAction.GetStateUp(hand))
        {
            Teleport();
        }
    }

    void ActivateLaser()
    {
        if(Physics.Raycast(transform.position,transform.forward, out hitInfo, 100f, teleportLayer)){
            ShowLaser();
        }
    }

    void Teleport()
    {
        laserReference.SetActive(false);
        reticleReference.SetActive(false);

        Vector3 offset = cameraRigTransform.position - headTransform.position;
        offset.y = 0;

        if (hitInfo.collider)
        {
            cameraRigTransform.position = hitInfo.point + offset;
        }
    }

    void ShowLaser()
    {
        laserTransfrom.position = Vector3.Lerp(pose.transform.position, hitInfo.point, .5f);
        laserTransfrom.localScale = new Vector3(laserTransfrom.localScale.x, laserTransfrom.localScale.y, hitInfo.distance);
        laserTransfrom.LookAt(hitInfo.point);
        laserReference.SetActive(true);

        reticleTransform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + reticleOffset, hitInfo.point.z);
        reticleReference.SetActive(true);

    }
}
