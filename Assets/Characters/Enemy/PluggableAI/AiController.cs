﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour, IController
{
    [SerializeField] State currentState;
    [SerializeField] State remainState;
    [SerializeField] float actionTime;
    float timer;

    [SerializeField] EnemyMovement movement;
    public EnemyMovement Movement => movement;
    [SerializeField] TankShoot shooting;
    public TankShoot Shooting => shooting;
    bool trackPlayer;
    PlayerController playerController;
    [SerializeField] Transform[] patrolRoute;
    [SerializeField] int patrolIndex;
    Vector3 lastPlayerPosition;
    public int PatrolIndex
    {
        get
        {
            return patrolIndex;
        }
        set
        {
            if(value >= 0 && value < patrolRoute.Length)
            {
                patrolIndex = value;
            }
        }
    }

    public ControlType TypeOfControl => throw new System.NotImplementedException();

    IMoveable IController.Movement => throw new System.NotImplementedException();

    IShooter IController.Shooting => throw new System.NotImplementedException();

    public Vector3 GetLastSeenPosition()
    {
        return lastPlayerPosition;
    }

    public void SavePlayerPosition()
    {
        lastPlayerPosition = playerController.transform.position;
    }

    public void SetActionTime(float time)
    {
        actionTime = time;
    }

    public void TrackPlayer(bool state)
    {
        trackPlayer = state;
    }

    public int GetRouteLength()
    {
        return patrolRoute.Length;
    }

    public Vector3 GetNextSpot()
    {
        return patrolRoute[patrolIndex].position;
    }

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (trackPlayer)
        {
            Shooting.Aim(playerController.transform.position);
        }
        currentState.UpdateState(this);
    }

    public Vector3 GetTargetPosition()
    {
        if (playerController != null)
        {
            return playerController.transform.position;
        }
        return Vector3.zero;
    }

    public bool IsTargetActive()
    {
        return playerController.isActiveAndEnabled;
    }

    public void TransitionToState(State newState)
    {
        if (newState != remainState)
        {
            currentState = newState;
            OnStateChanged();
        }
    }

    public bool CheckActionTimer(float countDown)
    {
        actionTime = countDown;
        timer += Time.deltaTime;
        return (timer >= actionTime);
    }

    void OnStateChanged()
    {
        timer = 0;
    }
}