using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public enum MovementState
    {
        Idle = 0,
        Moving = 1,
    }

    public enum ActionState
    {
        Inactive = 0
    }
}
