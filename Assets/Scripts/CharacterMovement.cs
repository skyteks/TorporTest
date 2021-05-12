using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class CharacterMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private NavMeshObstacle obstacle;
    private new CharacterAnimation animation;

    public UnityEvent onArrived;

    private Coroutine rotating;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();
        animation = GetComponent<CharacterAnimation>();
    }

    void Update()
    {
        if (agent.enabled && agent.isOnNavMesh)
        {
            float steeringAngle = Vector3.SignedAngle(transform.forward, (agent.steeringTarget - transform.position).normalized, Vector3.up);
            animation.steeringAngle = steeringAngle;
        }
        else if(rotating == null)
        {
            animation.steeringAngle = 0f;
        }
    }

    void LateUpdate()
    {
        bool stopped = Vector3.Distance(transform.position, agent.destination) <= agent.stoppingDistance;
        if (stopped)
        {
            bool first = false;
            if (agent.isOnNavMesh)
            {
                agent.isStopped = true;
                first = true;
            }
            agent.enabled = false;

            if (obstacle != null)
            {
                obstacle.enabled = true;
            }

            if (first)
            {
                onArrived?.Invoke();
            }
        }
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

    public void Stop()
    {
        if (agent.isOnNavMesh)
        {
            agent.destination = transform.position;
            agent.isStopped = true;
        }
        agent.enabled = false;

        if (obstacle != null)
        {
            obstacle.enabled = true;
        }

        onArrived?.Invoke();
    }

    public void RotateTowards(Vector3 point)
    {
        if (rotating != null)
        {
            StopCoroutine(rotating);
        }
        rotating = StartCoroutine(Rotate(point));
    }

    private IEnumerator Rotate(Vector3 point)
    {
        Vector3 vector = (point - transform.position);
        vector.y = 0f;
        vector.Normalize();
        Quaternion angleRot = Quaternion.LookRotation(vector, Vector3.up);
        while (Vector3.Angle(transform.forward, vector) > 1f)
        {
            Vector3 forwardBefore = transform.forward;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, angleRot, Time.deltaTime * 100f);
            animation.steeringAngle = Vector3.SignedAngle(transform.forward, vector, Vector3.up);
            yield return null;
        }
        rotating = null;
    }
}
