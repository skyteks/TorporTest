﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : BaseController
{
    public LayerMask walkRaycastMask;
    public float interactingDistance = 1f;

    private new Camera camera;

    private Coroutine goToNPCRoutine;

#if UNITY_EDITOR
    [Header("Editor")]
    public bool drawInteractingDistance = true;
#endif

    protected override void Awake()
    {
        base.Awake();
        camera = Camera.main;
    }

    void Update()
    {
        if (!GameManager.Instance.uiManager.gameObject.activeSelf && EventSystem.current.currentSelectedGameObject == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                CastRaycast();
            }
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (drawInteractingDistance)
        {
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, interactingDistance);
        }
    }
#endif

    private void CastRaycast()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        bool success = Physics.Raycast(ray, out hit, 100f, walkRaycastMask);
        if (success)
        {
            if (goToNPCRoutine != null)
            {
                StopCoroutine(goToNPCRoutine);
                goToNPCRoutine = null;
            }

            NPCController npc = hit.transform.GetComponent<NPCController>();
            if (npc != null)
            {
                goToNPCRoutine = StartCoroutine(GoToNPC(npc));
            }
            else
            {
                movement.GoTo(hit.point);
                Debug.DrawLine(ray.origin, hit.point, Color.cyan, 0.1f);
            }
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 0.1f);
        }
    }

    private IEnumerator GoToNPC(NPCController npc)
    {
        for (; ; )
        {
            if (Vector3.Distance(transform.position, npc.transform.position) > interactingDistance)
            {
                movement.GoTo(npc.transform.position);
            }
            else
            {
                goToNPCRoutine = null;
                InteractWithNPC(npc);
                yield break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void InteractWithNPC(NPCController npc)
    {
        npc.SwitchState(NPCController.NPCStates.Stopped);

        movement.Stop();
        npc.movement.Stop();

        movement.RotateTowards(npc.transform.position);
        npc.movement.RotateTowards(transform.position);
    }
}
