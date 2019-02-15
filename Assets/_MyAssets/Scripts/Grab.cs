using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Grab : MonoBehaviour
{

    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose pose;
    public SteamVR_Action_Boolean grab;

    private GameObject collidingObject;
    private GameObject objectInHand;

    private void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }

    private void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!collidingObject) { return; }
        collidingObject = null;
    }

    private void SetCollidingObject(Collider other)
    {
        if(collidingObject || !other.GetComponent<Rigidbody>()) { return; }

        collidingObject = other.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (grab.GetLastStateDown(handType))
        {
            if (collidingObject)
            {
                GrabObject();
            }
        }

        if (grab.GetLastStateUp(handType))
        {
            if (objectInHand)
            {
                ReleaseObject();
            }
        }
        
    }

    void GrabObject()
    {
        objectInHand = collidingObject;
        collidingObject = null;

        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();

    }

    void ReleaseObject()
    {
        FixedJoint joint = GetComponent<FixedJoint>();
        if (joint)
        {
            joint.connectedBody = null;
            Destroy(joint);

            Rigidbody rb = objectInHand.GetComponent<Rigidbody>();
            rb.velocity = pose.GetVelocity();
            rb.angularVelocity = pose.GetAngularVelocity();
        }

        objectInHand = null;
    }

    FixedJoint AddFixedJoint()
    {
        FixedJoint fj = gameObject.AddComponent<FixedJoint>();
        fj.breakForce = 20000f;
        fj.breakTorque = 20000f;
        return fj;
    }
}
