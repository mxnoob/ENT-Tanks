﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Patrol")]
public class PatrolAction : AiAction
{


    public override void Act(AiController controller)
    {
        Patrol(controller);
    }

    void Patrol(AiController controller)
    {
        //controller.Movement.Agent.stoppingDistance = 0;
        controller.Movement.MoveTo(controller.GetNextSpot());
        controller.Shooting.ResetMuzzle();
        if (!controller.Movement.IsMoving())
        {
            controller.PatrolIndex = (controller.PatrolIndex + 1) % controller.GetRouteLength();
        }
    }
}
