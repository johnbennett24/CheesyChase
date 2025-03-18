using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mouse : MonoBehaviour
{
    public enum MouseDirection
    {
        DiagonalRight,
        DiagonalLeft
    }

    [SerializeField] private float _minSpeed = 2f;
    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private float _accelerationTime = 30f;
    [SerializeField] private float _angle = 45f;
    [SerializeField] private float _absXBounds = 2f;

    private MouseDirection _direction;
    private Vector3 _startPosition;
    private float _speed;

    public float Angle => _angle;

    public float Speed => _speed;

    public void OnTap(InputValue value)
    {
        if(Context.Instance.IsPlaying)
            SwitchDirection();
    }

    private void Awake()
    {
        _startPosition = transform.position;

        ResetPositionAndRotation();
        ResetSpeed();
    }

    private void Update()
    {
        if(Context.Instance.IsPlaying)
        {
            UpdateAcceleration();

            float x = _speed * Mathf.Cos(_angle);

            Vector3 xDirection = (_direction == MouseDirection.DiagonalRight) ? Vector3.right : Vector3.left;
            Vector3 velocity = xDirection * x;

            if (!IsPositionOutOfBounds(transform.position + velocity))
                transform.Translate(velocity * Time.deltaTime, Space.World);
            else
                SwitchDirection();
        }
    }

    private void OnEnable()
    {
        Context.Instance.StateChanged += OnStateChanged;
    }

    private void OnDisable()
    {
        Context.Instance.StateChanged -= OnStateChanged;
    }

    private void OnStateChanged(GameState state)
    {
        if(state == GameState.Title)
            ResetPositionAndRotation();

        if(state == GameState.Playing)
        {
            SetDirection(MouseDirection.DiagonalRight);
            ResetSpeed();
        }
    }

    private void SetDirection(MouseDirection direction)
    {
        _direction = direction;

        float angle = (_direction == MouseDirection.DiagonalRight) ? _angle : -_angle;
        transform.localRotation = Quaternion.Euler(0, angle, 0);
    }

    private void ResetPositionAndRotation()
    {
        transform.position = _startPosition;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private void SwitchDirection()
    {
        SetDirection((_direction == MouseDirection.DiagonalRight) ?
            MouseDirection.DiagonalLeft : MouseDirection.DiagonalRight);
    }

    private bool IsPositionOutOfBounds(Vector3 position)
    {
        return Mathf.Abs(position.x) > _absXBounds;
    }

    private void UpdateAcceleration()
    {
        if (_speed >= _maxSpeed) return;

        _speed += Time.deltaTime / _accelerationTime;
    }

    private void ResetSpeed()
    {
        _speed = _minSpeed;
    }
}
