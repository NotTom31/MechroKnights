using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpModule : MonoBehaviour
{
    [SerializeField] public float AirMoveScale { get; private set; } = 0.5f;
    public bool IsGrounded { get; private set; } = false;

    public void OnJump(InputAction.CallbackContext context)
    {

    }
}
