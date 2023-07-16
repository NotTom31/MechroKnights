using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveModule : MonoBehaviour
{
    [SerializeField][Range(0, 5)] private float MoveSpeed = 1f;
    private Vector3 heading;

    // Stun system isStunned reference
    StunSystem stunSystemRef;
    // jump module isGrounded reference
    [SerializeField] JumpModule jumpModuleRef;
    [SerializeField] private CharacterController charControlRef;
    // [SerializeField] private Rigidbody rbRef;
    [SerializeField] private GameObject playerRef;

    private void Awake()
    {
            stunSystemRef = GameManager.serviceLocator.GetStunSystem();
    }
    private void FixedUpdate()
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;

        Vector3 forwardRelativeInput = heading.z * forward;
        Vector3 rightRelativeInput = heading.x * right;
        Vector3 cameraRelativeMovement = forwardRelativeInput + rightRelativeInput;

        charControlRef.Move(cameraRelativeMovement);
    }
    private void Update()
    {
        

    }
    private void LateUpdate()
    {
        
    }

    void OnMove(InputValue value)
    {
        Vector2 inputHeading = value.Get<Vector2>();
        Vector3 inputHeadingIn3D = new(inputHeading.x, 0, inputHeading.y);
        

        Debug.Log("inputheadingin3d: " + inputHeadingIn3D);

        inputHeadingIn3D *= MoveSpeed;
        if (stunSystemRef == null)
            stunSystemRef = GameManager.serviceLocator.GetStunSystem();
        // multiply heading by stun value if stunned(likely zero). get ref if bool ref is null. if there's none, throw an error in log and continue without multiplying
        if (stunSystemRef.IsStunned)
        {
            inputHeadingIn3D *= stunSystemRef.StunScale;
        }
        // multiply by air move value if in the air. get ref if bool ref is null. if there's none, throw an error in log and continue without multiplying.
        if (jumpModuleRef.IsGrounded)
        {
            // if (stunSystem == null)
                // stunSystem = ; // FIX THIS!!
            inputHeadingIn3D *= jumpModuleRef.AirMoveScale;
        }

        


        heading = inputHeadingIn3D;
    }

}
