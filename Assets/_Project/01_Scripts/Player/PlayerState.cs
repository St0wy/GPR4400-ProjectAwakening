using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public enum MovementState
    {
        Idle = 0,
        Moving = 1,
        Immobile = 2
    }

    public enum ActionState
    {
        Inactive = 0,
        Melee = 1,
        Aim = 2,
        Shield = 3,
        Carry = 4
    }

    MovementState _curMoveState;
    ActionState _curActionState;

    public MovementState CurMoveState { get => _curMoveState; set => _curMoveState = value; }
    public ActionState CurActionState { get => _curActionState; set => _curActionState = value; }
}
