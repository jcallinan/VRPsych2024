using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SubmarineControl : MonoBehaviour
{
    public SteamVR_Action_Boolean grabAction;
    public SteamVR_Behaviour_Pose leftHandPose, rightHandPose;
    public Transform submarine;
    public Transform vrCamera; // Assign the VR Camera Transform here
    public float moveSpeed = 5f;
    public float turnSpeed = 30f;

    // Lever control types
    public enum LeverType { UpDown, LeftRight }
    public LeverType leftHandLeverType = LeverType.UpDown;
    public LeverType rightHandLeverType = LeverType.LeftRight;

    private Vector3 leftHandLastPosition, rightHandLastPosition;
    private bool leftHandIsGrabbing = false, rightHandIsGrabbing = false;

    void Update()
    {
        HandleLeverMovement(leftHandPose, leftHandLeverType, ref leftHandIsGrabbing, ref leftHandLastPosition);
        HandleLeverMovement(rightHandPose, rightHandLeverType, ref rightHandIsGrabbing, ref rightHandLastPosition);
        TurnSubmarine();
    }

    void HandleLeverMovement(SteamVR_Behaviour_Pose handPose, LeverType leverType, ref bool isGrabbing, ref Vector3 lastHandPosition)
    {
        if (grabAction.GetStateDown(handPose.inputSource))
        {
            lastHandPosition = handPose.transform.localPosition;
            isGrabbing = true;
        }
        else if (grabAction.GetStateUp(handPose.inputSource))
        {
            isGrabbing = false;
        }

        if (isGrabbing)
        {
            Vector3 handMovement = handPose.transform.localPosition - lastHandPosition;

            if (leverType == LeverType.UpDown)
            {
                // Move submarine up/down based on hand movement
                submarine.Translate(0, handMovement.y * moveSpeed, 0, Space.World);
            }
            else if (leverType == LeverType.LeftRight)
            {
                // Move submarine left/right based on hand movement
                submarine.Translate(handMovement.x * moveSpeed, 0, 0, Space.World);
            }

            lastHandPosition = handPose.transform.localPosition;
        }
    }

    void TurnSubmarine()
    {
        // Turn submarine based on VR camera's yaw (left and right rotation)
        Quaternion targetRotation = Quaternion.Euler(0, vrCamera.eulerAngles.y, 0);
        submarine.rotation = Quaternion.Lerp(submarine.rotation, targetRotation, Time.deltaTime * turnSpeed);
    }
}
