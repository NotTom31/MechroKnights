using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum AIState
{
    Idle,
    Chasing,
    Melee,
    Shooting,
    Jumping,
    Blocking
}

public class AICharacter : MonoBehaviour
{
    [SerializeField] GameObject player;
    private NavMeshAgent navMeshAgent;

    public float chaseDistanceThreshold = 20f; //if ai is outside this area, ai should move towards player 100% of the time 
    public float jumpDistanceThreshold = 5f;
    public float blockDistanceThreshold = 1.5f;
    public float meleeDistanceThreshold = 1f;
    public float shootDistanceThreshold = 10f;

    private JumpModule jumpModuleRef;
    private MoveModule moveModuleRef;
    private LookModule lookModuleRef;
    private MeleeModule meleeModuleRef;
    private BlockModule blockModuleRef;
    //[SerializeField] private MechState mechStateRef;
    private Animator animatorRef;

    private float distToPlayer;
    private Vector2 movement;
    private bool isJumping;
    private bool isBlocking;
    private bool isMelee;
    private bool isTakeDamage;
    //private bool isStunned;
    private float blockDuration;

    private Stack<AIState> stateStack = new Stack<AIState>();
    private List<AIState> excludedStates = new List<AIState>();

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        jumpModuleRef = GetComponent<JumpModule>();
        moveModuleRef = GetComponent<MoveModule>();
        lookModuleRef = GetComponent<LookModule>();
        meleeModuleRef = GetComponent<MeleeModule>();
        blockModuleRef = GetComponent<BlockModule>();
        animatorRef = GetComponent<Animator>();

        stateStack.Push(AIState.Chasing);
    }

    private float timeSinceLastStateChange;
    public float stateChangeInterval = 3f; 

    private void Update()
    {
        Brain();
    }

    private void Brain()
    {
        AIState currentState = stateStack.Peek();
        Debug.Log(currentState);

        if (!isJumping && ShouldChase() && !IsStateExcluded(AIState.Chasing, AIState.Blocking, AIState.Melee, AIState.Shooting))
        {
            ClearStateStack();
            stateStack.Push(AIState.Chasing);
            return;
        }

        if (!isJumping)
        {
            if (ShouldBlock() && !IsStateExcluded(AIState.Blocking, AIState.Melee, AIState.Shooting))
            {
                stateStack.Push(AIState.Blocking);
            }

            if (ShouldMelee() && !IsStateExcluded(AIState.Melee, AIState.Blocking, AIState.Shooting))
            {
                stateStack.Push(AIState.Melee);
            }

            if (ShouldShoot() && !IsStateExcluded(AIState.Shooting, AIState.Blocking, AIState.Melee))
            {
                stateStack.Push(AIState.Shooting);
            }
        }

        // Execute the action for the current state.
        ExecuteStateAction(currentState);
    }

    private void ExecuteStateAction(AIState state)
    {
        switch (state)
        {
            case AIState.Chasing:
                Move();
                break;
            case AIState.Jumping:
                Jump();
                break;
            case AIState.Melee:
                Melee();
                break;
            case AIState.Shooting:
                Shoot();
                break;
            case AIState.Blocking:
                Block(blockDuration);
                break;
            case AIState.Idle:
                break;
        }
    }

    private void Move()
    {
        movement = player.transform.position - transform.position; //not sure about this
        moveModuleRef.OnMove(movement);
    }

    private void Jump()
    {
        jumpModuleRef.OnJump();
        isJumping = true;
    }

    private void Melee()
    {
        //shwing
    }

    private void Shoot()
    {
        //pew
    }

    private void Block(float blockDuration)
    {
        blockModuleRef.OnBlock(blockDuration);
        isBlocking = true;
    }

    private bool ShouldChase()
    {
        return distToPlayer > chaseDistanceThreshold;
    }

    private bool ShouldJump()
    {
        return distToPlayer < jumpDistanceThreshold;
    }

    private bool ShouldBlock()
    {
        return distToPlayer < blockDistanceThreshold && !isBlocking && !isMelee && !isJumping;
    }

    private bool ShouldMelee()
    {
        return distToPlayer < meleeDistanceThreshold;
    }

    private bool ShouldShoot()
    {
        return distToPlayer < shootDistanceThreshold;
    }

    private bool IsStateExcluded(params AIState[] states)
    {
        foreach (AIState state in states)
        {
            if (excludedStates.Contains(state))
            {
                return true;
            }
        }
        return false;
    }

    private void ExcludeState(AIState state)
    {
        if (!excludedStates.Contains(state))
        {
            excludedStates.Add(state);
        }
    }

    private void ClearStateStack()
    {
        stateStack.Clear();
    }

    private void ClearExcludedStates()
    {
        excludedStates.Clear();
    }
}