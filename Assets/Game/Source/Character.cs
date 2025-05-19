using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	private const string HorizontalAxisName = "Horizontal";
	private const KeyCode JumpKey = KeyCode.Space;

	[SerializeField] private Rigidbody2D _rigidbody;
	[SerializeField] private ObstacleChecker _groundChecker;
	[SerializeField] private ObstacleChecker _cailChecker;

	[SerializeField] private float _yVelocityForJump;
	[SerializeField] private float _gravity;

	[SerializeField] private float _speed;

	private Vector2 _velocity;

	private bool _jumpPressed;

	public Vector2 Velocity => _velocity;

	private Quaternion TurnRight => Quaternion.identity;
	private Quaternion TurnLeft => Quaternion.Euler(0, 180, 0);

	private void Update()
	{
		_jumpPressed = Input.GetKeyDown(JumpKey);

		HandleHorizontalInput();

		HandleGravity();

		HandleJump();

		HandleCail();

		_rigidbody.velocity = _velocity;

		transform.rotation = GetRotationFrom(_velocity);
	}

	public bool IsGrounded() => _groundChecker.IsTouches();

	private void HandleCail()
	{
		if (_cailChecker.IsTouches())
			_velocity.y = Mathf.Min(0, _velocity.y);
	}

	private void HandleHorizontalInput()
	{
		float xInput = Input.GetAxis(HorizontalAxisName);
		_velocity.x = _speed * xInput;
	}

	private void HandleJump()
	{
		if (_jumpPressed && _groundChecker.IsTouches())
			_velocity.y = _yVelocityForJump;
	}

	private void HandleGravity()
	{
		if (_groundChecker.IsTouches() && _velocity.y <= 0)
			_velocity.y = 0;
		else
			_velocity.y -= _gravity * Time.deltaTime;
	}

	private Quaternion GetRotationFrom(Vector2 velocity)
	{
		if (velocity.x > 0)
			return TurnRight;

		if (velocity.x < 0)
			return TurnLeft;

		return transform.rotation;
	}
}
