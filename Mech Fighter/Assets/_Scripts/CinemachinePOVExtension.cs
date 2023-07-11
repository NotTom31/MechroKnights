using UnityEngine;
using Cinemachine;

public class CinemachinePOVExtension : CinemachineExtension
{
    // private InputSystem inputSys; // maybe a ref to look module instead?
    private Vector3 startingRotation;

    protected override void Awake()
    {
        // get input sys ref
        base.Awake();
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTimeSeconds)
    {
        if (vcam.Follow)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {
                if (startingRotation == null)
                    startingRotation = transform.localRotation.eulerAngles;
                // Vector2 deltaInput = inputSys.GetMouseDelta();

            }
        }
    }
}
