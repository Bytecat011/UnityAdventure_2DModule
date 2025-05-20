using System;
using System.Collections;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
	public event Action<Character> CharacterSpawned;

	[SerializeField] private Character _characterPrefab;
	[SerializeField] private float _respawnDelay;
	[SerializeField] private Transform _spawnPoint;

	private Character _activeCharacter;

	private void Awake()
	{
		StartCoroutine(SpawnCharacterTask(0f));
	}

	private IEnumerator SpawnCharacterTask(float delay)
	{
		yield return new WaitForSeconds(delay);
		_activeCharacter = Instantiate(_characterPrefab, _spawnPoint.position, Quaternion.identity);
		_activeCharacter.Died += OnCharacterDied;
		CharacterSpawned?.Invoke(_activeCharacter);
	}

	private void OnCharacterDied()
	{
		_activeCharacter.Died -= OnCharacterDied;
		StartCoroutine(SpawnCharacterTask(_respawnDelay));
	}
}