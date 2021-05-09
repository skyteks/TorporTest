using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CharacterMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    private NavMeshObstacle obstacle;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        obstacle = GetComponent<NavMeshObstacle>();
    }

    public void GoTo(Vector3 position)
    {
        if (obstacle != null)
        {
            obstacle.enabled = false;
        }
        agent.enabled = true;
        agent.SetDestination(position);
        agent.isStopped = false;
    }

    private void LateUpdate()
    {
        bool stopped = Vector3.Distance(transform.position, agent.destination) <= agent.stoppingDistance;
        if (stopped)
        {
            if (agent.enabled && agent.isOnNavMesh)
            {
                agent.isStopped = true;
            }
            agent.enabled = false;

            if (obstacle != null)
            {
                obstacle.enabled = true;
            }
        }
    }
}
