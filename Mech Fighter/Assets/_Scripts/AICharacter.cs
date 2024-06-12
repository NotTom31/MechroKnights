using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Pathfinding;

public enum AIState
{
    Idle,
    Chasing,
    Melee,
    Shooting,
    Jumping,
    Blocking,
    Stunned
}

public enum AIMovement
{
    Forward,
    Backward,
    Left,
    Right,
    Idle
}

public class AICharacter : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] private GameObject leftNav;
    [SerializeField] private GameObject rightNav;
    [SerializeField] private GameObject backNav;
    //private NavMeshAgent navMeshAgent;
    [SerializeField] AIDestinationSetter aIDestinationSetter;

    public float chooseChaseDist = 400f;
    public float chooseJumpDist = 300f;
    public float chooseBlockDist = 500f;
    public float chooseMeleeDist = 500f;
    public float chooseShootDist = 505f;
    public float chooseRetreatDist = 50f;
    public float meleeRange = 20f;

    private JumpModule jumpModuleRef;
    private MoveModule moveModuleRef;
    private LookModule lookModuleRef;
    private MeleeModule meleeModuleRef;
    private BlockModule blockModuleRef;
    private FireModule fireModuleRef;
    private MechState mechStateRef;
    private Animator animatorRef;
    private Rigidbody rb;
    private StunSystem stunSysRef;

    private float distToPlayer;
    private Vector2 movement;
    private bool isMove;
    float speed;
    private bool isJumping;
    private bool isBlocking;
    private bool isMelee;
    private bool isShooting;
    private bool isTakeDamage;
    //private bool isStunned;
    private float blockDuration;

    private Stack<AIState> stateStack = new Stack<AIState>();
    private List<AIState> excludedStates = new List<AIState>();

    public float decisionCooldownHigh = 1.2f;
    public float decisionCooldownLow = 0.5f;
    private float decisionTimer = 0f;

    //public float chaseContinuationProbability = 0.2f; // The probability of continuing to chase when within range.

    private void Start()
    {
        //navMeshAgent = GetComponent<NavMeshAgent>();
        jumpModuleRef = GetComponent<JumpModule>();
        moveModuleRef = GetComponent<MoveModule>();
        lookModuleRef = GetComponent<LookModule>();
        meleeModuleRef = GetComponent<MeleeModule>();
        blockModuleRef = GetComponent<BlockModule>();
        fireModuleRef = GetComponent<FireModule>();
        mechStateRef = GetComponent<MechState>();
        animatorRef = GetComponentInChildren<Animator>();
        stunSysRef = GetComponent<StunSystem>();
        rb = GetComponent<Rigidbody>();

        stateStack.Push(AIState.Chasing);
    }

    private void Update()
    {
        LookAt();
        distToPlayer = Vector3.Distance(transform.position, player.transform.position);

        isJumping = stateStack.Contains(AIState.Jumping);
        isBlocking = stateStack.Contains(AIState.Blocking);
        isMelee = stateStack.Contains(AIState.Melee);

        //speed = Vector3.Magnitude(rb.velocity.normalized);
        //moveModuleRef.OnMove(rb.velocity.normalized); //I think this might be wrong
        //Debug.Log(speed + "Speed");

        if (isMove)
            animatorRef.SetFloat("Input Magnitude", 1); //this is a really dumb hotfix
        else
            animatorRef.SetFloat("Input Magnitude", 0);

        // Add a cooldown before making the next decision.
        if (decisionTimer > 0f)
        {
            decisionTimer -= Time.deltaTime;
            return;
        }
        
        Brain();
        float randomValue = Random.value;
        if (randomValue < 0.4f)
            Move(AIMovement.Forward);
        else if (randomValue < 0.5f && !isBlocking && !isMelee)
            Move(AIMovement.Left);
        else if (randomValue < 0.6f && !isBlocking && !isMelee)
            Move(AIMovement.Right);
        else if (randomValue < 0.8f && !isBlocking && !isMelee)
            Move(AIMovement.Backward);
        else
            Move(AIMovement.Idle);

        // Reset the decision timer after making a decision.
        decisionTimer = Random.Range(decisionCooldownLow, decisionCooldownHigh);
        //Debug.Log(decisionTimer);

        // After executing the action, remove the state from the stack.
        if (stateStack.Count > 0)
        {
            AIState currentState = stateStack.Peek();
            if (currentState == AIState.Jumping && !isJumping)
            {
                stateStack.Pop();
            }
            else if (currentState == AIState.Blocking && !isBlocking)
            {
                stateStack.Pop();
            }
            else if (currentState == AIState.Melee && !isMelee)
            {
                stateStack.Pop();
            }
            else if (currentState == AIState.Shooting && !isShooting)
            {
                stateStack.Pop();
            }
        }
    }

    private void Brain()
    {
        //Debug.Log(stateStack);
        AIState currentState = stateStack.Peek();
        //Debug.Log(currentState);

/*        // The AI will always chase if it is outside the chase distance threshold or based on a random chance.
        if ((ShouldChase() && Random.value < chaseContinuationProbability) && !IsStateExcluded(AIState.Chasing, AIState.Blocking, AIState.Melee, AIState.Shooting))
        {
            
            ClearStateStack();
            stateStack.Push(AIState.Chasing);
            return;
        }*/

        // If mech should be stunned, clear the state stack and push the stunned state
        // Then peek the state into currentstate and execute it. Early Return.
        if (ShouldBeStunned())
        {
            ClearStateStack();
            stateStack.Push(AIState.Stunned);
            currentState = stateStack.Peek();
            ExecuteStateAction(currentState);
            return;
        }

        // Randomly decide which action to take based on probabilities.
        float randomValue = Random.value;
        //Debug.Log(randomValue);

        if (randomValue < 0.2f && !isJumping && ShouldJump())
        {
            stateStack.Push(AIState.Jumping);
        }
        else if (randomValue < 0.4f && !isBlocking && ShouldBlock()) //add a hp and battery check, maybe block more when low health
        {
            stateStack.Push(AIState.Blocking);
        }
        else if (randomValue < 0.7f && ShouldMelee())
        {
            stateStack.Push(AIState.Melee);
        }
        else if (ShouldShoot())
        {
            stateStack.Push(AIState.Shooting);
        }

        currentState = stateStack.Peek();
        ExecuteStateAction(currentState);
    }

    private void ExecuteStateAction(AIState state)
    {
        switch (state)
        {
            case AIState.Chasing:
                Move(AIMovement.Forward);
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
                Block();
                break;
            case AIState.Idle:
                break;
            case AIState.Stunned:
                // If anything needs to be done for stun to work properly, put it here.
                break;
        }
    }

    private void Move(AIMovement direction) //edit to vary the movement, sometimes it should retreat, strafe, or some combination
    {
        
        switch (direction)
        {
            case AIMovement.Forward:
                //navMeshAgent.SetDestination(player.transform.position);
                /*movement = transform.position - player.transform.position;
                movement.Normalize();
                moveModuleRef.OnMove(movement);*/
                isMove = true;
                aIDestinationSetter.target = player.transform;
                //moveModuleRef.OnMove(rb.velocity.normalized);
                break;
            case AIMovement.Left:
                isMove = true;
                //navMeshAgent.SetDestination(leftNav.transform.position);
                aIDestinationSetter.target = leftNav.transform;
                //moveModuleRef.OnMove(rb.velocity.normalized);
                break;
            case AIMovement.Right:
                isMove = true;
                //navMeshAgent.SetDestination(rightNav.transform.position);
                aIDestinationSetter.target = rightNav.transform;
                //moveModuleRef.OnMove(rb.velocity.normalized);
                break;
            case AIMovement.Backward:
                isMove = true;
                //navMeshAgent.SetDestination(backNav.transform.position);
                aIDestinationSetter.target = backNav.transform;
                //moveModuleRef.OnMove(rb.velocity.normalized);
                break;
            case AIMovement.Idle:
                isMove = true;
                //navMeshAgent.SetDestination(transform.position); //might be bad
                aIDestinationSetter.target = null;
                rb.velocity.Equals(Vector3.zero);
                //moveModuleRef.OnMove(rb.velocity.normalized);
                break;
        }

    }

    private void Jump()
    {
        //Debug.Log("Jumping");
        jumpModuleRef.OnJump();
        Debug.Log("here i am");
        decisionTimer = 0;
        //isJumping = true;
    }

    private void Melee()
    {
        Debug.Log("Do Melee attack");
        Move(AIMovement.Idle);
        StartCoroutine(Swing());
            
        //do attack, maybe have a chance of 2nd attack
        

        //isMelee = false;
    }

    private IEnumerator Swing()
    {
        int attackType = Random.Range(0, 2);
        int attackAmount = Random.Range(4, 9);

        for (int i = 0; i < attackAmount; i++)
        {
            if (attackType == 0)
                meleeModuleRef.OnMeleeLight();
            else
                meleeModuleRef.OnMeleeHeavy();
            attackType = Random.Range(0, 2);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void Shoot()
    {
        Debug.Log("Shooting");
        StartCoroutine(Fire());
        fireModuleRef.OnFire();
        //isShooting = false;
    }

    private IEnumerator Fire()
    {
        int attackAmount = Random.Range(7, 11);
        for (int i = 0; i < attackAmount; i++)
        {
            fireModuleRef.OnFire();
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void Block()
    {
        float blockDuration = Random.Range(1, 4); Debug.Log("Blocking");
        blockModuleRef.OnBlock(blockDuration);
        //isBlocking = true;
    }

    private void LookAt()
    {
        lookModuleRef.OnLook(player);
    }

    private bool ShouldChase()
    {
        return distToPlayer > chooseChaseDist;
    }

    private bool ShouldJump()
    {
        if (isJumping)
            return false;
        return distToPlayer < chooseJumpDist;
    }

    private bool ShouldBlock()
    {
        return distToPlayer < chooseBlockDist && !isBlocking && !isMelee && !isJumping;
    }

    private bool ShouldMelee()
    {
        return distToPlayer < chooseMeleeDist;
    }

    private bool ShouldShoot()
    {
        return distToPlayer < chooseShootDist;
    }

    private bool ShouldRetreat()
    {
        return distToPlayer < chooseRetreatDist;
    }

    private bool ShouldBeStunned()
    {
        return stunSysRef.IsStunned;
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