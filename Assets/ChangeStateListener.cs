using System;
using System.Collections;
using System.Collections.Generic;
using DWGames.CoordinatedMovement;
using UnityEngine;
[RequireComponent(typeof(DWTargetFollowSteering))]
public class ChangeStateListener : MonoBehaviour
{
    public int changeCount;
    public TargetFollowState currentState;
    
    void Start()
    {
        GetComponent<DWTargetFollowSteering>().followStateListeners.AddListener((go, state) =>
        {
            currentState = state;
            changeCount++;
        });
    }

}
