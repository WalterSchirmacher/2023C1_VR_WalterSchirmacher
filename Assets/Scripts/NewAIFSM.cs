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
    public GameObject patrolPoints;
    private Vector3[] patrolPointList = new Vector3[1];
   // private List<Vector3> patrolPointList;
    private int patrolIndex = 0;

    void Awake()
    {
        agent = friendOrFoe.GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _idleState = Fsm_IdleState;
        _doPatrolState = Fsm_DoPatrol;
        _chaseState = Fsm_ChaseState;
        _attackState = Fsm_AttackState;

        _fsm = new Fsm();
        _fsm.Start(_idleState);


        Array.Resize(ref patrolPointList, patrolPoints.transform.childCount-1);

        // Find all Patrol Point Game Objects in Parent and add to list
        for(int i = 0; i < patrolPoints.transform.childCount-1; i++)
        {
            patrolPointList[i] = patrolPoints.transform.GetChild(i).gameObject.transform.position;
        }

    //    foreach (Transform child in patrolPoints.transform)
  //      {
         //   patrolPointList.(child.gameObject.transform.position);
     //   }
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
            agent.SetDestination(patrolPointList[patrolIndex]);
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
                if(agent.remainingDistance <= agent.stoppingDistance)
                {
                    patrolIndex += 1;
                    if(patrolIndex == patrolPointList.Length)
                    {
                        patrolIndex = 0;
                    }
                    agent.SetDestination(patrolPointList[patrolIndex]);
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
                Debug.Log("Lost sight of player... Waiting.");
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
                Debug.Log("Lost sight of player... Waiting.");
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
            Debug.Log("count " + friendOrFoe.animationAttack.Count);
            Debug.Log("index " + index);
            Debug.Log("attack with " + friendOrFoe.animationAttack[index]);

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

}
