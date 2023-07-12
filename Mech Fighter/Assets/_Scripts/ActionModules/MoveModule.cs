using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveModule : MonoBehaviour
{
    [SerializeField][Range(0, 100)] private float MoveSpeed;
    private Vector3 heading;

    // Stun system isStunned reference
    StunSystem stunSystemRef;
    // jump module isGrounded reference
    [SerializeField] JumpModule jumpModuleRef;
    [SerializeField] private CharacterController charControlRef;
    [SerializeField] private Rigidbody rbRef;
    [SerializeField] private GameObject playerRef;
    private void Awake()
    {
        // get StunSystem reference from GameManager
        stunSystemRef = GameManager.serviceLocator.GetStunSystem();
        // get jump module 

    }
    private void FixedUpdate()
    {
        // rb.AddForce(heading);
        charControlRef.Move(heading);
    }
    private void Update()
    {
       
    }
    private void LateUpdate()
    {
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputHeading = context.ReadValue<Vector2>();
        Vector3 inputHeadingIn3D = new Vector3(inputHeading.x, 0, inputHeading.y);

        inputHeadingIn3D *= MoveSpeed;
        // multiply heading by stun value if stunned(likely zero). get ref if bool ref is null. if there's none, throw an error in log and continue without multiplying
        if (stunSystemRef.IsStunned)
        {
            if (stunSystemRef == null)
                stunSystemRef = GameManager.serviceLocator.GetStunSystem();
            inputHeadingIn3D *= stunSystemRef.stunScale;
        }
        // multiply by air move value if in the air. get ref if bool ref is null. if there's none, throw an error in log and continue without multiplying.
        if (jumpModuleRef.IsGrounded)
        {
            // if (stunSystem == null)
                // stunSystem = ; // FIX THIS!!
            inputHeadingIn3D *= jumpModuleRef.AirMoveScale;
        }
        
        //playerRef.transform; // Need to get the player model facing towards the camera direction (in look modules)
        //inputHeadingIn3D

        heading = inputHeadingIn3D;
    }
    
}
