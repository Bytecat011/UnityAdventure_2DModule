using UnityEngine;

public class CharacterDeathEffectView : MonoBehaviour
{
	[SerializeField] private Character _character;
	[SerializeField] private AnimationEventsHandler _deathEffectPrefab;
	[SerializeField] private Transform _viewPosition;

	private AnimationEventsHandler _deathEffect;

	private void Awake()
	{
		_character.Died += OnCharacterDied;
	}

	private void OnDestroy()
	{
		_character.Died -= OnCharacterDied;
	}

	private void OnCharacterDied()
	{
		_deathEffect = Instantiate(_deathEffectPrefab, _viewPosition.position, Quaternion.identity);
		_deathEffect.AnimationCompleted += OnDeathEffectAnimationCompleted;
	}

	private void OnDeathEffectAnimationCompleted()
	{
		_deathEffect.AnimationCompleted -= OnDeathEffectAnimationCompleted;
		Destroy(_deathEffect.gameObject);
	}
}