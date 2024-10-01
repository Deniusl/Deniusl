using DG.Tweening;
using UnityEngine;


[System.Serializable]
public class Sound 
{
	[SerializeField] private string _name;
	[SerializeField] private AudioClip _clip;
	[SerializeField, Range(0f, 1f)] private float _volume = 1.0f;
	[SerializeField, Range(0f, 3f)] private float _pitch = 1.0f;
	[SerializeField] private bool _loop;
	[SerializeField] private bool _playOnAwake;
    
	private AudioSource _source;

	public string Name => _name;
	
	public void Initialize(AudioSource source)
	{
		_source = source;
		_source.clip = _clip;
		_source.volume = _volume;
		_source.pitch = _pitch;
		_source.loop = _loop;
		_source.playOnAwake = _playOnAwake;
	}

	public void Play() => _source.Play();

	public void Stop() => _source.Stop();
	
	public void Unmute(float duration = 0.0f)
	{
		_source.DOFade(_volume, duration);
	}

	public void Mute(float duration = 0.0f)
	{
		_source.DOFade(0f, duration);
	}
}