using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
public class PlayerNearTrigger : MonoBehaviour
{
    public float triggerRange = 4f;

    private SphereCollider sphereCollider;
    private Rigidbody rigid;

#if UNITY_EDITOR
    [Header("Editor")]
    public bool drawTriggerRange = true;
#endif

    void Awake()
    {
        Setup();
    }

    void Reset()
    {
        Setup();
    }

    void OnTriggerEnter(Collider other)
    {
        NPCController npc = other.GetComponent<NPCController>();
        if (npc != null)
        {
            npc.OnNearPlayer(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        NPCController npc = other.GetComponent<NPCController>();
        if (npc != null)
        {
            npc.OnNearPlayer(false);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.color = Color.grey;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, triggerRange);
    }
#endif

    private void Setup()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = triggerRange;

        rigid = GetComponent<Rigidbody>();
        rigid.isKinematic = true;
    }
}
