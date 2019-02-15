using UnityEngine;
using Valve.VR;

public class PlayerMovement : MonoBehaviour
{
    public SteamVR_Input_Sources hand;
    public SteamVR_Action_Vector2 move;
    public SteamVR_Action_Boolean running;

    public Transform head;
    public Transform body;

    public float runningSpeed = .1f;
    public float walkingSpeed = .05f;

    private float speed = 0f;

    // Update is called once per frame
    void Update()
    {

        speed = (running.GetState(hand)) ? runningSpeed : walkingSpeed; 

        var horizontalInput = move.GetAxis(hand).x;
        var forwardInput = move.GetAxis(hand).y;

        Vector3 forwardMovement = head.forward * forwardInput * speed;
        forwardMovement.y = 0;

        Vector3 horizontalMovement = head.right * horizontalInput * speed;
        horizontalMovement.y = 0;


        body.Translate(forwardMovement);
        body.Translate(horizontalMovement);
    }
}
