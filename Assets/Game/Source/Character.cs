using System;
using UnityEngine;

public class Character : MonoBehaviour, IKillable
{
	private const string HorizontalAxisName = "Horizontal";
	private const KeyCode JumpKey = KeyCode.Space;

	public event Action Died;

	[SerializeField] private Rigidbody2D _rigidbody;
	[SerializeField] private ObstacleChecker _groundChecker;
	[SerializeField] private ObstacleChecker _cailChecker;

	[SerializeField] private ObstacleChecker _rightWallChecker;
	[SerializeField] private ObstacleChecker _leftWallChecker;

	[SerializeField] private float _yVelocityForJump;

	[SerializeField] private float _yVelocityForWallJump;
	[SerializeField] private float _xVelocityForWallJump;

	[SerializeField] private float _wallJumpHorizontalInputLockTime;

	[SerializeField] private float _gravity;
	[SerializeField] private float _wallFrictionFactor;

	[SerializeField] private float _speed;

	private bool _isAlive = true;

	private float _horizontalInputLockTime;
	private bool _isHorizontalInputLocked;

	private Vector2 _velocity;

	private bool _jumpPressed;

	public Vector2 Velocity => _velocity;

	private Quaternion TurnRight => Quaternion.identity;
	private Quaternion TurnLeft => Quaternion.Euler(0, 180, 0);

	private void Update()
	{
		if (!_isAlive)
		{
			return;
		}

		_jumpPressed = Input.GetKeyDown(JumpKey);

		ProcessInputLock();

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
		if (_isHorizontalInputLocked)
			return;

		float xInput = Input.GetAxis(HorizontalAxisName);
		_velocity.x = _speed * xInput;
	}

	private void HandleJump()
	{
		if (!_jumpPressed)
			return;

		if (_groundChecker.IsTouches())
			_velocity.y = _yVelocityForJump;
		else if (IsTouchingWall())
			HandleWallJump();
	}

	private void HandleWallJump()
	{
		_velocity.y = _yVelocityForWallJump;
		_velocity.x = _rightWallChecker.IsTouches() ?
			-_xVelocityForWallJump : _xVelocityForWallJump;
		LockHorizontalInput(_wallJumpHorizontalInputLockTime);
	}

	private bool IsTouchingWall()
	{
		return _leftWallChecker.IsTouches() || _rightWallChecker.IsTouches();
	}

	private void LockHorizontalInput(float lockTime)
	{
		_isHorizontalInputLocked = true;
		_horizontalInputLockTime = Mathf.Max(lockTime, _horizontalInputLockTime);
	}

	private void ProcessInputLock()
	{
		if (_isHorizontalInputLocked)
		{
			_horizontalInputLockTime -= Time.deltaTime;
			if (_horizontalInputLockTime < 0)
			{
				_isHorizontalInputLocked = false;
			}
		}
	}

	private void HandleGravity()
	{
		if (_groundChecker.IsTouches() && _velocity.y <= 0)
			_velocity.y = 0;
		else
			_velocity.y -= CalcGravityForce() * Time.deltaTime;
	}

	private float CalcGravityForce()
	{
		float gravityForce = _gravity;
		if (_velocity.y < 0 && IsTouchingWall())
		{
			gravityForce *= 1f - _wallFrictionFactor;
		}
		return gravityForce;
	}

	private Quaternion GetRotationFrom(Vector2 velocity)
	{
		if (velocity.x > 0)
			return TurnRight;

		if (velocity.x < 0)
			return TurnLeft;

		return transform.rotation;
	}

	public void Kill()
	{
		if (!_isAlive) {
			return;
		}

		_isAlive = false; 
		Destroy(gameObject);
		Died?.Invoke();
	}
}
