using Cinemachine;
using UnityEngine;

public class FollowCharacterCamera : MonoBehaviour
{
	[SerializeField] private CharacterSpawner _spawner;

	private CinemachineVirtualCamera _camera;

	private void Awake()
	{
		_camera = GetComponent<CinemachineVirtualCamera>();
		_spawner.CharacterSpawned += OnCharacterSpawned;
	}

	private void OnCharacterSpawned(Character character)
	{
		_camera.Follow = character.GetComponentInChildren<CharacterView>().transform;
	}

	private void OnDestroy()
	{
		_spawner.CharacterSpawned -= OnCharacterSpawned;
	}
}