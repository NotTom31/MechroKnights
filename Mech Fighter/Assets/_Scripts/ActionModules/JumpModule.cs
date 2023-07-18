using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpModule : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider coll;
    [SerializeField] private Animator animatorRef;
    [SerializeField] public float AirMoveScale { get; private set; } = 0.5f;
    [SerializeField] private float spherecastOffset;
    [SerializeField] [Range(0, 20)] private float jumpForce;
    public LayerMask groundMask;

    public bool IsGrounded { get; private set; } = false;
    public float GroundAngle { get; private set; }
    public Vector3 GroundNormal { get; private set; }
    private bool isJumping = false;


    private void FixedUpdate()
    {
        GroundCheck();
    }
    public void OnJump()
    {
        if (IsGrounded && !isJumping)
        {
            isJumping = true;
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
        animatorRef.SetBool("Is Jumping", true);
    }
    void OnJump(InputValue value)
    {
        /*if (menuManager.IsPaused() || menuManager.IsShop() || menuManager.IsDialogue())
            return;*/
        if (IsGrounded && !isJumping)
        {
            isJumping = true;
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
        animatorRef.SetBool("Is Jumping", true);
    }
    void GroundCheck()
    {
        if (Physics.SphereCast(transform.position, coll.radius, Vector3.down, out RaycastHit hit, coll.height / 2 - coll.radius + spherecastOffset))
        {
            IsGrounded = true;
            GroundAngle = Vector3.Angle(Vector3.up, hit.normal);
            GroundNormal = hit.normal;

            if (Physics.BoxCast(transform.position, new Vector3(coll.radius / 2.5f, coll.radius / 3f, coll.radius / 2.5f), Vector3.down, out RaycastHit helpHit, transform.rotation, coll.height / 2 - coll.radius / 2))
            {
                GroundAngle = Vector3.Angle(Vector3.up, helpHit.normal);
            }
            isJumping = false;
            animatorRef.SetBool("Is Jumping", false);
        }
        else
        {
            IsGrounded = false;
            GroundAngle = 0;
            GroundNormal = Vector3.up;
        }
        animatorRef.SetBool("Is Grounded", IsGrounded);
    }
}
