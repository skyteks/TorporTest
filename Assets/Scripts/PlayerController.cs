﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : BaseController
{
    public LayerMask walkRaycastMask;

    private new Camera camera;

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
                CastWalkRaycast();
            }
        }
    }

    private void CastWalkRaycast()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        bool success = Physics.Raycast(ray, out hit, 100f, walkRaycastMask, QueryTriggerInteraction.Ignore);
        if (success)
        {
            movement.GoTo(hit.point);
            Debug.DrawLine(ray.origin, hit.point, Color.cyan, 0.1f);
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 0.1f);
        }
    }
}