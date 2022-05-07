using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
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

    MovementState _movementState = MovementState.Moving;
    ActionState _actionState;

    [SerializeField] float _speed = 5.0f;

    Vector2 _direction;

    PlayerInput _playerInput;
    Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        //Get the directional value
        _direction = _playerInput.actions.FindAction("Move").ReadValue<Vector2>();

        //Normalize the vector if it's above 1 in length
        if (_direction.sqrMagnitude > _direction.normalized.sqrMagnitude)
            _direction.Normalize();

        SetMovement();
    }

    void SetMovement()
    {
        if (_movementState == MovementState.Moving && _direction == Vector2.zero)
            _movementState = (MovementState)0;
        else if (_movementState == MovementState.Idle && _direction != Vector2.zero)
            _movementState = (MovementState)1;

        //Check if we can move
        switch (_movementState)
        {
            case MovementState.Idle:
            case MovementState.Moving:
                //Set our velocity
                _rb.velocity = _direction * _speed;
                break;
            default:
                break;
        }
    }
}