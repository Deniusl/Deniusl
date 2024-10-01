using Services;
using UnityEngine;
using UnityEngine.UI;

public class UIMuteSoundsButton : MonoBehaviour
{
    [SerializeField] private Toggle _muteOrUnmuteSounds;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Sprite _soundOn;
    [SerializeField] private Sprite _soundOff;

    private IAudioMixerService _audioMixerService;
    
    private void Start()
    {
        _audioMixerService = AllServices.Get<IAudioMixerService>();
        
        InitButton();
        _muteOrUnmuteSounds.onValueChanged.AddListener(MuteOrUnmuteButtonClicked);
    }

    private void MuteOrUnmuteButtonClicked(bool state)
    {
        _audioMixerService.SwitchStateSound(state);
        SwitchSpiteOnSoundButton(state);
    }

    private void InitButton()
    {
        var soundState = _audioMixerService.GetCurrentStateSound;
        _muteOrUnmuteSounds.isOn = soundState;
        SwitchSpiteOnSoundButton(soundState);
    }

    private void SwitchSpiteOnSoundButton(bool state) => _backgroundImage.sprite = state ? _soundOn : _soundOff;
}