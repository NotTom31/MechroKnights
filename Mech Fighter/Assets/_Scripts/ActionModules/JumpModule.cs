using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpModule : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CharacterController charControlRef;
    [SerializeField] public float AirMoveScale { get; private set; } = 0.5f;
    [SerializeField] private float spherecastOffset;
    [SerializeField] private float jumpHeight;
    public bool IsGrounded { get; private set; } = false;
    private float groundAngle;

    private Vector3 groundNormal;

    private void Update()
    {
        GroundCheck();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        /*if (menuManager.IsPaused() || menuManager.IsShop() || menuManager.IsDialogue())
            return;*/
        if (IsGrounded)
        { 
            rb.AddForce(transform.up * jumpHeight, ForceMode.VelocityChange);
            IsGrounded = false;
        }
        // Anim.SetBool("Is Jumping", true);
    }
    void GroundCheck()
    {
        if (Physics.SphereCast(transform.position, charControlRef.radius, Vector3.down, out RaycastHit hit, charControlRef.height / 2 - charControlRef.radius + spherecastOffset))
        {
            IsGrounded = true;
            groundAngle = Vector3.Angle(Vector3.up, hit.normal);
            groundNormal = hit.normal;

            if (Physics.BoxCast(transform.position, new Vector3(charControlRef.radius / 2.5f, charControlRef.radius / 3f, charControlRef.radius / 2.5f), Vector3.down, out RaycastHit helpHit, transform.rotation, charControlRef.height / 2 - charControlRef.radius / 2))
            {
                groundAngle = Vector3.Angle(Vector3.up, helpHit.normal);
            }
            // Anim.SetBool("Is Jumping", false);
        }
        else
        {
            IsGrounded = false;
            groundAngle = 0;
            groundNormal = Vector3.up;
        }
        // Anim.SetBool("Is Grounded", isGrounded);
    }
}
