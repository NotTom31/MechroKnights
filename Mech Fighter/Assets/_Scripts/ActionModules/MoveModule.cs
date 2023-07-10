using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveModule : MonoBehaviour
{
    [SerializeField][Range(0, 100)] private float MoveSpeed;
    private Vector3 heading;

    // Stun system isStunned reference
    // jump module isGrounded reference
    [SerializeField] private CharacterController CharControl;
    [SerializeField] private Rigidbody rb;

    private void Start()
    {
        
    }
    private void Awake()
    {
        // get StunSystem reference from GameManager
        // get jump module 
    }
    private void FixedUpdate()
    {
        rb.AddForce(heading);
    }
    private void Update()
    {
       
    }
    private void LateUpdate()
    {
        
    }
    private void OnDestroy()
    {
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputHeading = context.ReadValue<Vector2>();
        Vector3 inputHeadingIn3D = new Vector3(inputHeading.x, 0, inputHeading.y);

        inputHeadingIn3D *= MoveSpeed;
        // multiply heading by stun value if stunned(likely zero). get ref if bool ref is null. if there's none, throw an error in log and continue without multiplying
        // multiply by air move value if in the air. get ref if bool ref is null. if there's none, throw an error in log and continue without multiplying.
        heading = inputHeadingIn3D;
    }
    
}
