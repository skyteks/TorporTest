using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CharacterAnimation : MonoBehaviour
{
    private static int hashForward = Animator.StringToHash("Forward");
    private static int hashTurn = Animator.StringToHash("Turn");
    private static int hashCrouch = Animator.StringToHash("Crouch");
    private static int hashOnGround = Animator.StringToHash("OnGround");
    private static int hashJump = Animator.StringToHash("Jump");
    private static int hashJumpLeg = Animator.StringToHash("JumpLeg");
    private static int hashWin = Animator.StringToHash("Win");
    private static int hashDeathTrigger = Animator.StringToHash("DeathTrigger");

    private NavMeshAgent agent;
    private Animator anim;

    public float velocityToWalkSpeedScale = 0.1f;

#if UNITY_EDITOR
    [Header("Editor")]
    public bool drawSteeringAngle;
#endif

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    void LateUpdate()
    {
        anim.SetFloat(hashForward, agent.velocity.magnitude * velocityToWalkSpeedScale);
        float angle = Vector3.SignedAngle(transform.forward, (agent.steeringTarget - transform.position).normalized, Vector3.up);
        anim.SetFloat(hashTurn, angle * 0.01f);
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (Application.isPlaying && drawSteeringAngle)
        {
            Gizmos.color = Color.green;
            Color color = Color.green;
            color.a = 0.2f;
            float angle = Vector3.SignedAngle(transform.forward, (agent.steeringTarget - transform.position).normalized, Vector3.up);
            UnityEditor.Handles.color = color;
            UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, angle, 2f);
            Gizmos.DrawSphere(agent.steeringTarget, 0.1f);
        }
    }
#endif
}
