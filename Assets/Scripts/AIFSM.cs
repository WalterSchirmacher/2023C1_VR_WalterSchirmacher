using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIFSM : MonoBehaviour
{
    public enum AIState { Idle, GoHome, ChasePlayer, AttackPlayer };
    public AIState currentState = AIState.Idle;
    public Sight sightSensor;
    public float playerAttackDistance;
    public FriendOrFoe friendOrFoe;
    public float stalkingSpeed = 1f;
    public float runningSpeed = 3.5f;

    [Tooltip("Time in Seconds to Wait after losing player")]
    [Range(0,60)]
    public float waitingTime = 30f;
    private float waitingTimer = 0f;
    private NavMeshAgent agent;
    private bool isWaiting = false;
    public bool goHome = false;
    public bool preferStalking = true;

    void Awake()
    {
        agent = friendOrFoe.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        waitingTimer += Time.deltaTime;

        if (waitingTimer > waitingTime && isWaiting && currentState == AIState.Idle)
        {
            isWaiting = false;
            if (goHome)
            {
                currentState = AIState.GoHome;
            }
            else
            {
                currentState = AIState.Idle;
            }
        }

        switch (currentState)
        {
            case AIState.Idle:
                Idle(); ;       
                break;
            case AIState.GoHome:
                GoHome(); ;
                break;
            case AIState.ChasePlayer:
                ChasePlayer(); ;
                break;
            case AIState.AttackPlayer:
                AttackPlayer(); ;
                break;
            default:
                Idle(); 
                Debug.Log("AI State Not Found");
                break;
        }
    }
    void Idle()
    {
        Debug.Log("Idle");
        agent.isStopped = true;

        friendOrFoe.animator.Play(friendOrFoe.animationIdle);

        if (sightSensor.detectedObject != null)
        {
            currentState = AIState.ChasePlayer;
        }
    }
    void GoHome()
    {
        Debug.Log("Going Home");
        agent.isStopped = false;
        agent.SetDestination(friendOrFoe.myHomeLocation);
        friendOrFoe.animator.Play(friendOrFoe.animationChase);

        if (sightSensor.detectedObject != null)
        {
            currentState = AIState.ChasePlayer;
        }
    }
    void ChasePlayer()
    {
        // Check if myStatus if Friendly (0) or Neutral (1)
        int status = (int)friendOrFoe.myStatus;
        if(preferStalking && status < 2)
        {
            Debug.Log("Stalking Player");
            friendOrFoe.animator.Play(friendOrFoe.animationStalk);
            ChaseAfterPlayer(stalkingSpeed);
        } else
        {
            Debug.Log("Chasing Player");
            friendOrFoe.animator.Play(friendOrFoe.animationChase);
            ChaseAfterPlayer(runningSpeed);
        }
        
    }
    void ChaseAfterPlayer(float speed)
    {
        agent.isStopped = false;
        agent.speed = speed;

        if (sightSensor.detectedObject == null)
        {
            Debug.Log("Lost sight of player... Waiting.");
            isWaiting = true;
            currentState = AIState.Idle;
            waitingTimer = 0f;
            return;
        }

        LookTo(sightSensor.detectedObject.transform.position);

        agent.SetDestination(sightSensor.detectedObject.transform.position);

        float distanceToPlayer = Vector3.Distance(transform.position, sightSensor.detectedObject.transform.position);
        Debug.Log("distance to player " + distanceToPlayer);
        if (distanceToPlayer <= playerAttackDistance && friendOrFoe.myStatus != GameMaster.Disposition.Friendly)
        {
            if(preferStalking)
            {
                agent.speed = runningSpeed;
            }
            currentState = AIState.AttackPlayer;
        }
    }

    void AttackPlayer()
    {
        Debug.Log("Attacking Player");
        agent.isStopped = true;
        if (sightSensor.detectedObject == null)
        {
            currentState = AIState.GoHome;
            return;
        }
        LookTo(sightSensor.detectedObject.transform.position);


        int index = Random.Range(0, friendOrFoe.animationAttack.Count);
        Debug.Log("count " + friendOrFoe.animationAttack.Count);
        Debug.Log("index " + index);
        Debug.Log("attack with " + friendOrFoe.animationAttack[index]);

        friendOrFoe.animator.Play(friendOrFoe.animationAttack[index]);

        float distanceToPlayer = Vector3.Distance(transform.position, sightSensor.detectedObject.transform.position);

        if (distanceToPlayer > playerAttackDistance * 1.1f)
        {
            currentState = AIState.ChasePlayer;
        }
    }

    void LookTo(Vector3 targetPosition)
    {
        Vector3 directionToPosition = Vector3.Normalize(targetPosition - transform.parent.position);
        directionToPosition.y = 0;
        transform.parent.forward = directionToPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, playerAttackDistance);
    }
}