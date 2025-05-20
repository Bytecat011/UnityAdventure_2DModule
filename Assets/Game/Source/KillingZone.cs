using UnityEngine;

public class KillingZone : MonoBehaviour
{
	[SerializeField] private TriggerReciever _killingTrigger;

	private void Awake()
	{
		_killingTrigger.TriggerEntered += OnTriggerEntered;
	}

	private void OnTriggerEntered(Collider2D other)
	{
		if (other.TryGetComponent<IKillable>(out var killable))
		{
			killable.Kill();
		}
	}

	private void OnDestroy()
	{
		_killingTrigger.TriggerEntered -= OnTriggerEntered;
	}
}
