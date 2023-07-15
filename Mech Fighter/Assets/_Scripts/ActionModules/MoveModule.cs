using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveModule : MonoBehaviour
{
    [SerializeField][Range(0, 50)] private float moveForce = 1f;
    [SerializeField] [Range(1, 100)] private float maxVelocity = 15f;
    private Vector3 heading;

    // Stun system isStunned reference
    StunSystem stunSystemRef;
    // jump module isGrounded reference
    [SerializeField] JumpModule jumpModuleRef;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animatorRef;
    [SerializeField] private GameObject playerRef;

    private void Awake()
    {
        stunSystemRef = GameManager.serviceLocator.GetStunSystem();
        rb.freezeRotation = true;
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

        rb.AddForce(cameraRelativeMovement);
        if (rb.velocity.sqrMagnitude > maxVelocity * maxVelocity) // Using sqrMagnitude for efficiency
        {
            float yValue = rb.velocity.y;
            Vector3 clampedVelocity = rb.velocity.normalized * maxVelocity;
            rb.velocity = new Vector3(clampedVelocity.x, yValue, clampedVelocity.z);
        }
    }
    private void Update()
    {
        

    }
    private void LateUpdate()
    {
        
    }

    void OnMove(InputValue value)
    {
        /*if (menuManager.IsPaused() || menuManager.IsShop() || menuManager.IsDialogue())
        {
            movementVector = Vector3.zero;
            return;
        }*/
        Vector2 inputHeading = value.Get<Vector2>();
        Debug.Log("inputHeading" + inputHeading);
        Vector3 inputHeadingIn3D = new(inputHeading.x, 0, inputHeading.y);
        animatorRef.SetFloat("Input Magnitude", inputHeadingIn3D.magnitude);

        inputHeadingIn3D *= moveForce;
        if (stunSystemRef == null)
            stunSystemRef = GameManager.serviceLocator.GetStunSystem();
        if (stunSystemRef.IsStunned)
        {
            inputHeadingIn3D *= stunSystemRef.StunScale;
        }
        if (!jumpModuleRef.IsGrounded)
        {
            // if (stunSystem == null)
                // stunSystem = ; // FIX THIS!!
            inputHeadingIn3D *= jumpModuleRef.AirMoveScale;
        }
        Debug.Log("inputHeadingIn3D" + inputHeadingIn3D);
        heading = inputHeadingIn3D;
    }

}
