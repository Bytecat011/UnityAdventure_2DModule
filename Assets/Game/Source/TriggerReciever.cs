using System;
using UnityEngine;

public class TriggerReciever : MonoBehaviour
{
	public event Action<Collider2D> TriggerEntered;

	private void OnTriggerEnter2D(Collider2D other)
	{
		TriggerEntered?.Invoke(other);
	}
}