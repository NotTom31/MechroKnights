using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICharacter : MonoBehaviour
{
    [SerializeField] GameObject player;
    private NavMeshAgent navMeshAgent;

    [SerializeField] private JumpModule jumpModuleRef;
    [SerializeField] private MoveModule moveModuleRef;
    [SerializeField] private LookModule LookModuleRef;
    [SerializeField] private MeleeModule meleeModuleRef;
    [SerializeField] private BlockModule blockModuleRef;
    //[SerializeField] private MechState mechStateRef;
    [SerializeField] private Animator animatorRef;

    private float distToPlayer;
    private Vector2 movement;
    private bool isJumping;
    private bool isBlocking;
    private bool isMelee;
    private bool isTakeDamage;
    private bool isStunned;

    private void Update()
    {
        Brain();
    }

    private void Start()
    {
        // Initialize the NavMeshAgent component.
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Brain()
    {
        //decide on the action
    }

    private void Grab()
    {
        //do grab
    }

    private void Jump()
    {
        //jumpModuleRef.DoJump();
    }

    private void Move()
    {
        //moveModuleRef.
    }

    private void Melee()
    {
        //check chance of doing melee 2
    }

    private void Block(float time)
    {
        //blockModuleRef.SetBlock(time);
    }

}
