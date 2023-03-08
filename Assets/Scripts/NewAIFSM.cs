using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewAIFSM : MonoBehaviour
{
    // https://github.com/bzgeb/MinimalFSM
    // Using the GitHub Repo by bzgeb

    // Setup FSM
    Fsm _fsm;
    Fsm.State _idleState;
    Fsm.State _doPatrolState;
    Fsm.State _chaseState;
    Fsm.State _attackState;

    public enum AIState { Idle, DoPatrol, ChasePlayer, AttackPlayer };
    public AIState currentState = AIState.Idle;
    public Sight sightSensor;
    public float playerAttackDistance;
    public FriendOrFoe friendOrFoe;
    public float stalkingSpeed = 1f;
    public float runningSpeed = 3.5f;

    [Tooltip("Time in Seconds to Wait after losing player")]
    [Range(0, 60)]
    public float waitingTime = 30f;
    private float waitingTimer = 0f;
    private NavMeshAgent agent;
    private bool isWaiting = false;
    public bool preferStalking = true;
    private Fsm.State cState;
    public bool doPatrol = true;
    public bool randomPatrol = true;
    public GameObject patrolPoints;
    private List<Vector3> patrolPointList;
    private List<Vector3> randomPointList;
    private int patrolIndex = 0;

    void Awake()
    {
        agent = friendOrFoe.GetComponent<NavMeshAgent>();
        patrolPointList = new List<Vector3>();
        randomPointList = new List<Vector3>();

    }

    // Start is called before the first frame update
    void Start()
    {
        // Find all Patrol Point Game Objects in Parent and add to list

       String gName = patrolPoints.name;

        foreach (Transform child in patrolPoints.GetComponentsInChildren<Transform>())
        {
            if(child.gameObject.name != gName)
            {
                patrolPointList.Add(child.gameObject.transform.position);
            }
   
        }

        _idleState = Fsm_IdleState;
        _doPatrolState = Fsm_DoPatrol;
        _chaseState = Fsm_ChaseState;
        _attackState = Fsm_AttackState;

        _fsm = new Fsm();
        _fsm.Start(_doPatrolState);

    }

    // Update is called once per frame
    void Update()
    {
        _fsm.OnUpdate();

        waitingTimer += Time.deltaTime;

        if (waitingTimer > waitingTime && isWaiting && cState == _idleState)
        {
            isWaiting = false;
            if (doPatrol)
            {
                //   currentState = AIState.DoPatrol;
                _fsm.TransitionTo(_doPatrolState);
            }
            else
            {
                //   currentState = AIState.Idle;
                _fsm.TransitionTo(_idleState);
            }
        }
    }

    void Fsm_IdleState(Fsm fsm, Fsm.Step step, Fsm.State state)
    {
        if (step == Fsm.Step.Enter)
        {
            //On Enter
            agent.isStopped = true;
            friendOrFoe.PlayNewAnimation(friendOrFoe.animationIdle);
        }
        else if (step == Fsm.Step.Update)
        {
            //On Update
            if (sightSensor.detectedObject != null)
            {
                // 
                //    currentState = AIState.ChasePlayer;
                cState = _chaseState;
                fsm.TransitionTo(_chaseState);
            }

        }
        else if (step == Fsm.Step.Exit)
        {
            //On Exit

        }
    }
    void Fsm_DoPatrol(Fsm fsm, Fsm.Step step, Fsm.State state)
    {
        if (step == Fsm.Step.Enter)
        {
            //On Enter
            agent.isStopped = false;

      //      Debug.Log("Starting for " + patrolIndex + " and I am " + friendOrFoe.gameObject.name);

            agent.SetDestination(GetPatrolDestination());
            friendOrFoe.PlayNewAnimation(friendOrFoe.animationChase);
            friendOrFoe.PlayerLost();

        }
        else if (step == Fsm.Step.Update)
        {
            //On Update
            if (sightSensor.detectedObject != null)
            {
            //    currentState = AIState.ChasePlayer;
                cState = _chaseState;
                fsm.TransitionTo(_chaseState);
            }
            else
            {
                if (!agent.pathPending)
                {
                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                        {
                            agent.SetDestination(GetPatrolDestination());
                        }
                    }
                }
            }

        }
        else if (step == Fsm.Step.Exit)
        {
            //On Exit
            friendOrFoe.PlayNewAnimation(friendOrFoe.animationIdle);
        }
    }
    void Fsm_ChaseState(Fsm fsm, Fsm.Step step, Fsm.State state)
    {
        if (step == Fsm.Step.Enter)
        {
            //On Enter
            // Check if myStatus if Friendly (0) or Neutral (1)
            int status = (int)friendOrFoe.myStatus;
            friendOrFoe.ChasePlayer();
            agent.isStopped = false;

            if (preferStalking && status < 2) 
            {
                friendOrFoe.PlayNewAnimation(friendOrFoe.animationStalk);
                agent.speed = stalkingSpeed;
            }
            else
            {
                Debug.Log("Chasing Player");
                friendOrFoe.PlayNewAnimation(friendOrFoe.animationChase);
                agent.speed = runningSpeed;
            }
            
        }
        else if (step == Fsm.Step.Update)
        {
            //On Update
            if (sightSensor.detectedObject == null)
            {
            //    Debug.Log("Lost sight of player... Waiting.");
                isWaiting = true;
                //  currentState = AIState.Idle;
                cState = _idleState;
                waitingTimer = 0f;
                fsm.TransitionTo(_idleState);
                return;
            }

            // LookTo(sightSensor.detectedObject.transform.position);
            transform.LookAt(sightSensor.detectedObject.transform.position);

            agent.SetDestination(sightSensor.detectedObject.transform.position);

            float distanceToPlayer = Vector3.Distance(transform.position, sightSensor.detectedObject.transform.position);
         //   Debug.Log("distance to player " + distanceToPlayer);
            if (distanceToPlayer <= playerAttackDistance && friendOrFoe.myStatus != GameMaster.Disposition.Friendly)
            {
                if (preferStalking)
                {
                    agent.speed = runningSpeed;
                }
               // currentState = AIState.AttackPlayer;
                cState = _attackState;
                fsm.TransitionTo(_attackState);
            }

        }
        else if (step == Fsm.Step.Exit)
        {
            //On Exit
            friendOrFoe.PlayNewAnimation(friendOrFoe.animationIdle);
        }
    }
    void Fsm_AttackState(Fsm fsm, Fsm.Step step, Fsm.State state)
    {
        if (step == Fsm.Step.Enter)
        {
            //On Enter
            agent.isStopped = true;

        }
        else if (step == Fsm.Step.Update)
        {
            //On Update
            if (sightSensor.detectedObject == null)
            {
            //    Debug.Log("Lost sight of player... Waiting.");
                isWaiting = true;
                // currentState = AIState.Idle;
                cState = _idleState;
                waitingTimer = 0f;
                fsm.TransitionTo(_idleState);
                return;
            }
            //   LookTo(sightSensor.detectedObject.transform.position);
            transform.LookAt(sightSensor.detectedObject.transform.position);

            int index = UnityEngine.Random.Range(0, friendOrFoe.animationAttack.Count);
         //   Debug.Log("count " + friendOrFoe.animationAttack.Count);
        //    Debug.Log("index " + index);
        //    Debug.Log("attack with " + friendOrFoe.animationAttack[index]);

            friendOrFoe.PlayAttackAnimation(friendOrFoe.animationAttack[index]);

            float distanceToPlayer = Vector3.Distance(transform.position, sightSensor.detectedObject.transform.position);

            if (distanceToPlayer > playerAttackDistance * 1.1f)
            {
             //   currentState = AIState.ChasePlayer;
                cState = _chaseState;
                fsm.TransitionTo(_chaseState);
            }

        }
        else if (step == Fsm.Step.Exit)
        {
            //On Exit
            friendOrFoe.PlayNewAnimation(friendOrFoe.animationIdle);

        }
    }

    private Vector3 GetPatrolDestination()
    {
        Vector3 vic;
        if (randomPatrol)
        {
          //  Debug.Log("Random patrol");
            if (randomPointList.Count == 0)
            {
                randomPointList = new List<Vector3>(patrolPointList);
            }
            int randomIndex = UnityEngine.Random.Range(0, randomPointList.Count);
            vic = randomPointList[randomIndex];
        //    Debug.Log("randominzing..." + randomIndex);
        //    Debug.Log("random list is " + randomPointList[randomIndex].ToString());

            randomPointList.Remove(randomPointList[randomIndex]);

        }
        else
        {
            vic = patrolPointList[patrolIndex];
            patrolIndex++;

            if(patrolIndex == patrolPointList.Count)
            {
                patrolIndex = 0;
            }

         /*   if (patrolIndex == patrolPointList.Count - 1)
            { // if it's a last point
                patrolIndex = 0;
            }
            else
            {
                patrolIndex++;
            }*/
            
        }

        return vic;
    }

}
