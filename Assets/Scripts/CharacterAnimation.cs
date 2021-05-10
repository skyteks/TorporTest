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
    public float steeringLerpMultiplyer = 5f;

    public float steeringAngle { private get; set; }
    private float lastAngle;

#if UNITY_EDITOR
    [Header("Editor")]
    public bool drawSteeringAngle = true;
    public bool drawLerpedAngle = true;
#endif

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    void LateUpdate()
    {
        anim.SetFloat(hashForward, agent.velocity.magnitude * velocityToWalkSpeedScale);
        steeringAngle = Mathf.Lerp(lastAngle, steeringAngle, Time.deltaTime * steeringLerpMultiplyer);
        anim.SetFloat(hashTurn, steeringAngle * 0.01f);
        lastAngle = steeringAngle;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            float angle = steeringAngle;
            if (drawSteeringAngle)
            {
                Color color2 = Color.red;
                color2.a = 0.1f;
                UnityEditor.Handles.color = color2;
                UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, angle, 3f);
            }
            if (drawLerpedAngle)
            {
                angle = Mathf.Lerp(lastAngle, angle, Time.deltaTime * steeringLerpMultiplyer);

                Color color1 = Color.green;
                color1.a = 0.2f;
                UnityEditor.Handles.color = color1;
                UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, angle, 2f);
            }
        }
    }
#endif
}
