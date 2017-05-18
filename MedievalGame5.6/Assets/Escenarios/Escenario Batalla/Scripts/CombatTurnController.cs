using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTurnController : MonoBehaviour
{
    
    public BattlefieldController battlefield;

    public void SetStateSelectAttack()
    {
        battlefield.state = BattlefieldController.States.SelectTargetAttack;
    }

    public void SetStateAttacking()
    {
        battlefield.state = BattlefieldController.States.Attacking;
    }
    public void SetStateSelectMovement()
    {
        if (battlefield == null)
        {
            Debug.Log("battlefield is null");
        }
        battlefield.state = BattlefieldController.States.SelectMovement;
    }

    public void SetStateMoving()
    {
        battlefield.state = BattlefieldController.States.Moving;
    }


    public void SetStateIdle()
    {
        battlefield.state = BattlefieldController.States.Idle;
    }

    public void SetStateDie()
    {
        battlefield.state = BattlefieldController.States.Die;
    }

    public void SetStateTakeDamage()
    {
        battlefield.state = BattlefieldController.States.TakeDamage;
    }

}
