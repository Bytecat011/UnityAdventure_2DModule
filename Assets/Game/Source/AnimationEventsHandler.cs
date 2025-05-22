using System;
using UnityEngine;

public class AnimationEventsHandler : MonoBehaviour
{
	public event Action AnimationCompleted;

	public void OnAnimationCompleted() => AnimationCompleted?.Invoke();
}