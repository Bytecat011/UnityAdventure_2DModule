using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToPointMover : MonoBehaviour
{
	[SerializeField] private List<Transform> _points;
	[SerializeField] private float _moveTime;
	[SerializeField] private float _moveCooldownTime;
	[SerializeField] private AnimationCurve _progressCurve;
	[SerializeField] private bool _includeSatrtPoint;

	private Queue<Vector3> _pointsPositions;
	private Vector3 _currentTarget;

	private void Awake()
	{
		_pointsPositions = new Queue<Vector3>(_points.Count);

		foreach (var point in _points)
		{
			_pointsPositions.Enqueue(point.position);
		}

		if (_includeSatrtPoint)
		{
			_pointsPositions.Enqueue(transform.position);
		}

		StartCoroutine(ProcessMove());
	}

	private IEnumerator ProcessMove()
	{
		while (true)
		{
			SwitchPoint();

			Vector3 startPosition = transform.position;
			Vector3 endPosition = _currentTarget;

			float duration = _moveTime;
			float progress = 0f;

			while (progress < duration)
			{
				float lerpProgress = _progressCurve.Evaluate(progress / duration);
				transform.position = Vector3.LerpUnclamped(startPosition, endPosition, lerpProgress);
				progress += Time.deltaTime;
				yield return null;
			}

			yield return new WaitForSeconds(_moveCooldownTime);
		}
	}

	private void SwitchPoint()
	{
		_currentTarget = _pointsPositions.Dequeue();
		_pointsPositions.Enqueue(_currentTarget);
	}
}
