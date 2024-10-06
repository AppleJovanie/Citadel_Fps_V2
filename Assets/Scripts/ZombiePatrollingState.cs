using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombiePatrollingState : StateMachineBehaviour
{
    float timer;
    public float patrollingTime = 10f;
    NavMeshAgent agent;

    Transform player;
    public float detectionArea = 18f;
    public float patrolSpeed = 2f;

    List<Transform> wayPointsList = new List<Transform>();  

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

        agent.speed = patrolSpeed;
        timer = 0f;

        GameObject wayPointCluster = GameObject.FindGameObjectWithTag("Waypoints");
        foreach(Transform t in wayPointCluster.transform)
        {
            wayPointsList.Add(t);
        }

        Vector3 nextPosition = wayPointsList[Random.Range(0, wayPointsList.Count)].position;
        agent.SetDestination(nextPosition);
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if(SoundManager.Instance.zombieChannel.isPlaying == false)
        {
            SoundManager.Instance.zombieChannel.clip = SoundManager.Instance.zombieWalking;
            SoundManager.Instance.zombieChannel.PlayDelayed(1f);
        }

         if(agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(wayPointsList[Random.Range(0, wayPointsList.Count)].position);
        }
        //Transition to idle state
        timer += Time.deltaTime;
        
        if(timer > patrollingTime)
        {
            animator.SetBool("isPatrolling", false);
        }

        // transition to chase state
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < detectionArea)
        {
            animator.SetBool("isChasing", true);
        }
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
        SoundManager.Instance.zombieChannel.Stop();
    }
}
