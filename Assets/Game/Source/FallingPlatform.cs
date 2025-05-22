using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
	private readonly int TriggeredAnimationKey = Animator.StringToHash("Triggered");
	private readonly int FallAnimationKey = Animator.StringToHash("Fall");
	private readonly int RestoreAnimationKey = Animator.StringToHash("Restore");

	[SerializeField] private float _timeToFall;
	[SerializeField] private float _restoreTime;
	[SerializeField] private TriggerReciever _triggerZone;
	[SerializeField] private LayerMask _mask;
	[SerializeField] private Animator _animator;

	private Coroutine _activeFallTask;

	private void Awake()
	{
		_triggerZone.TriggerEntered += OnTriggerEntered;
	}

	private void OnDestroy()
	{
		_triggerZone.TriggerEntered -= OnTriggerEntered;
	}

	private void OnTriggerEntered(Collider2D collider)
	{
		if (_activeFallTask != null)
		{
			return;
		}

		if (IsInLayerMask(collider.gameObject, _mask))
		{
			_activeFallTask = StartCoroutine(FallTask());
		}
	}

	private bool IsInLayerMask(GameObject obj, LayerMask mask)
	{
		return ((1 << obj.layer) & mask) != 0;
	}

	private IEnumerator FallTask()
	{
		_animator.SetTrigger(TriggeredAnimationKey);
		yield return new WaitForSeconds(_timeToFall);
		_animator.SetTrigger(FallAnimationKey);
		yield return new WaitForSeconds(_restoreTime);
		_animator.SetTrigger(RestoreAnimationKey);
		_activeFallTask = null;
	}
}
