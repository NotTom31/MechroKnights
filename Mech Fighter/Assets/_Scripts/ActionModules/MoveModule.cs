using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveModule : MonoBehaviour
{
    [SerializeField][Range(0, 50)] private float moveForce = 1f;
    [SerializeField] [Range(1, 100)] private float maxVelocity = 15f;
    private Vector3 heading;
    private bool isAiControl = false;
    private float dynFriction;

    [SerializeField] private StunSystem stunSystemRef;
    [SerializeField] private JumpModule jumpModuleRef;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animatorRef;
    [SerializeField] private Collider moveColliderRef;
    [SerializeField] private GameObject playerRef;
    [SerializeField] private GameObject fakeCamera;

    private void Awake()
    {
        if (gameObject.GetComponent<PlayerInput>() == null && gameObject.GetComponentInChildren<PlayerInput>() == null)
            isAiControl = true;
        stunSystemRef = gameObject.GetComponent<StunSystem>();
        rb.freezeRotation = true;
        dynFriction = moveColliderRef.material.dynamicFriction;
    }
    private void FixedUpdate()
    {
        if (heading.sqrMagnitude == 0f)
        {
            Debug.Log($"Set friction to default");
            moveColliderRef.material.dynamicFriction = dynFriction;
            return;
        }
        Debug.Log($"Set friction to zero.");
        moveColliderRef.material.dynamicFriction = 0f;


        Vector3 forward;
        Vector3 right;
        if (!isAiControl)
        {
            forward = Camera.main.transform.forward;
            right = Camera.main.transform.right;
        }
        else
        {
            forward = fakeCamera.transform.forward;
            right = fakeCamera.transform.right;
        }

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
   
    public void OnMove(Vector3 inputHeading)
    {
        Vector3 inputHeadingIn3D = inputHeading;
        animatorRef.SetFloat("Input Magnitude", inputHeadingIn3D.magnitude);

        //inputHeadingIn3D *= moveForce;
/*        if (stunSystemRef == null)
            stunSystemRef = GameManager.serviceLocator.GetStunSystem();
        if (stunSystemRef.IsStunned)
        {
            inputHeadingIn3D *= stunSystemRef.StunScale;
        }*/
        if (!jumpModuleRef.IsGrounded)
        {
            inputHeadingIn3D *= jumpModuleRef.AirMoveScale;
        }
        heading = inputHeadingIn3D;
    }
    void OnMove(InputValue value)
    {
        /*if (menuManager.IsPaused() || menuManager.IsShop() || menuManager.IsDialogue())
        {
            movementVector = Vector3.zero;
            return;
        }*/
        Vector2 inputHeading = value.Get<Vector2>();
        Vector3 inputHeadingIn3D = new(inputHeading.x, 0, inputHeading.y);
        animatorRef.SetFloat("Input Magnitude", inputHeadingIn3D.magnitude);

        inputHeadingIn3D *= moveForce;
        if (stunSystemRef == null)
            stunSystemRef = gameObject.GetComponent<StunSystem>();
        if (stunSystemRef.IsStunned)
        {
            inputHeadingIn3D *= stunSystemRef.StunScale;
        }
        if (!jumpModuleRef.IsGrounded)
        {
            inputHeadingIn3D *= jumpModuleRef.AirMoveScale;
        }
        heading = inputHeadingIn3D;
    }

}
