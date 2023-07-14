using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpModule : MonoBehaviour
{
    // [SerializeField] private Rigidbody rb;
    [SerializeField] private CharacterController charControlRef;
    [SerializeField] public float AirMoveScale { get; private set; } = 0.5f;
    [SerializeField] private float spherecastOffset;
    [SerializeField] [Range(10, 50)] private float jumpHeight;
    public bool IsGrounded { get; private set; } = false;
    public float GroundAngle { get; private set; }
    public Vector3 GroundNormal { get; private set; }

    private void FixedUpdate()
    {
        GroundCheck();
        if (!IsGrounded)
            charControlRef.Move(Physics.gravity * Time.deltaTime);
    }

    void OnJump(InputValue value)
    {
        /*if (menuManager.IsPaused() || menuManager.IsShop() || menuManager.IsDialogue())
            return;*/
        if (IsGrounded)
        { 
            charControlRef.Move(transform.up * jumpHeight);
            Debug.Log("Jumped!");
        }
        
        // Anim.SetBool("Is Jumping", true);
    }
    void GroundCheck()
    {
        if (Physics.SphereCast(transform.position, charControlRef.radius, Vector3.down, out RaycastHit hit, charControlRef.height / 2 - charControlRef.radius + spherecastOffset))
        {
            IsGrounded = true;
            GroundAngle = Vector3.Angle(Vector3.up, hit.normal);
            GroundNormal = hit.normal;

            if (Physics.BoxCast(transform.position, new Vector3(charControlRef.radius / 2.5f, charControlRef.radius / 3f, charControlRef.radius / 2.5f), Vector3.down, out RaycastHit helpHit, transform.rotation, charControlRef.height / 2 - charControlRef.radius / 2))
            {
                GroundAngle = Vector3.Angle(Vector3.up, helpHit.normal);
            }
            // Anim.SetBool("Is Jumping", false);
        }
        else
        {
            IsGrounded = false;
            GroundAngle = 0;
            GroundNormal = Vector3.up;
        }
        // Anim.SetBool("Is Grounded", isGrounded);
    }
}
