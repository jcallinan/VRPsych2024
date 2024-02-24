using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRHandle : MonoBehaviour
{
    public SteamVR_Action_Boolean triggerAction = SteamVR_Actions.default_InteractUI; // Adjust this based on your actual action setup
    public SteamVR_Action_Vibration hapticAction = SteamVR_Actions.default_Haptic; // Haptic feedback action
    public Transform HandlePosition;
    public Transform HandleJointPrefab;

    private List<SteamVR_Behaviour_Pose> ActiveControllers = new List<SteamVR_Behaviour_Pose>();
    private Transform JointObject;
    private SteamVR_Behaviour_Pose AttachedController;
    private bool bAttached = false;

    void OnTriggerEnter(Collider collider)
    {
        SteamVR_Behaviour_Pose controllerPose = collider.GetComponentInParent<SteamVR_Behaviour_Pose>();
        if (controllerPose != null && !ActiveControllers.Contains(controllerPose))
        {
            ActiveControllers.Add(controllerPose);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        SteamVR_Behaviour_Pose controllerPose = collider.GetComponentInParent<SteamVR_Behaviour_Pose>();
        if (controllerPose != null)
        {
            ActiveControllers.Remove(controllerPose);
        }
    }

    void Update()
    {
        foreach (SteamVR_Behaviour_Pose controllerPose in ActiveControllers)
        {
            if (triggerAction.GetStateDown(controllerPose.inputSource) && !bAttached)
            {
                AttachTo(controllerPose);
            }
            else if (triggerAction.GetStateUp(controllerPose.inputSource) && bAttached)
            {
                Disconnect();
            }
        }
    }

    public void AttachTo(SteamVR_Behaviour_Pose controllerPose)
    {
        hapticAction.Execute(0, 0.1f, 150, 0.75f, controllerPose.inputSource); // Trigger haptic feedback

        JointObject = Instantiate(HandleJointPrefab, HandlePosition.position, Quaternion.identity, transform);
        ConfigurableJoint cj = JointObject.GetComponent<ConfigurableJoint>();
        cj.connectedBody = controllerPose.GetComponent<Rigidbody>();

        FixedJoint fj = JointObject.GetComponent<FixedJoint>();
        fj.connectedBody = GetComponent<Rigidbody>();

        AttachedController = controllerPose;
        bAttached = true;
    }

    public void Disconnect()
    {
        if (JointObject != null)
        {
            Destroy(JointObject.gameObject);
        }

        AttachedController = null;
        bAttached = false;
    }
}
