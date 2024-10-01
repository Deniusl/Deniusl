using Services;
using UnityEngine;

public class AudioMixerService : IAudioMixerService
{
    private const string SoundKey = "Sound";

    public AudioMixerService() => SwitchStateSound(GetCurrentStateSound);
    
    public bool GetCurrentStateSound
    {
        get => PlayerPrefs.GetInt(SoundKey).IntToBool();
        private set
        {
            PlayerPrefs.SetInt(SoundKey, value.BoolToInt());
            PlayerPrefs.Save();
        }
    }
    
    public void SwitchStateSound(bool state)
    {
        GetCurrentStateSound = state;
        AudioListener.volume = state.BoolToInt();
    }
}

public interface IAudioMixerService : IService
{
    public bool GetCurrentStateSound {get;}
    public void SwitchStateSound(bool state);
}