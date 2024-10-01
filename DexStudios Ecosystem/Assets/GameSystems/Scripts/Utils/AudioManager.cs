using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour 
{
	[SerializeField] private Sound[] _sounds;

	public static AudioManager Instance { get; private set; }

	private Dictionary<string, Sound> _soundDictionary;

	private void Awake()
	{
		if (Instance != null) 
		{
			Debug.LogError("Audio Manager can be only one");
		}
		else
		{
			Instance = this;
			_soundDictionary = new Dictionary<string, Sound>();

			foreach (var sound in _sounds)
			{
				sound.Initialize(gameObject.AddComponent<AudioSource>());

				if (!_soundDictionary.ContainsKey(sound.Name))
				{
					_soundDictionary.Add(sound.Name, sound);
				}
				else
				{
					Debug.LogWarning($"Duplicate sound name found: {sound.Name}. Only the first occurrence will be used.");
				}
			}
		}
	}

	private Sound GetSoundByName(string soundName)
	{
		_soundDictionary.TryGetValue(soundName, out var sound);

		if (sound == null)
		{
			Debug.LogWarning($"Sound not found: {soundName}");
		}

		return sound;
	}

	public void Play(string soundName) => GetSoundByName(soundName)?.Play();

	public void Stop(string soundName) => GetSoundByName(soundName)?.Stop();

	public void Mute(string soundName, float duration = 0) => GetSoundByName(soundName)?.Mute(duration);

	public void Unmute(string soundName, float duration = 0) => GetSoundByName(soundName)?.Unmute(duration);

	public void PlayWithDelay(string soundName, float delay)
	{
		StartCoroutine(PlaySoundWithDelayCoroutine(soundName, delay));
	}

	private IEnumerator PlaySoundWithDelayCoroutine(string soundName, float delay)
	{
		yield return new WaitForSeconds(delay);
		GetSoundByName(soundName)?.Play();
	}
}