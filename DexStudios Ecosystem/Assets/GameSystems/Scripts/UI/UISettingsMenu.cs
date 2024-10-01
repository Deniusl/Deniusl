using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UISettingsMenu : MonoBehaviour
    {
        [SerializeField] private Toggle soundToggle;
        [SerializeField] private Toggle musicToggle;
        [SerializeField] private Button closeButton;

        private void Awake()
        {
            soundToggle.onValueChanged.AddListener(OnSoundToggle);
            musicToggle.onValueChanged.AddListener(OnMusicToggle);
            closeButton.onClick.AddListener(OnCloseButtonClick);
            
            musicToggle.isOn = PlayerPrefs.GetInt("Music", 1) == 1;
            soundToggle.isOn = PlayerPrefs.GetInt("Sound", 1) == 1;
        }
        
        private void OnSoundToggle(bool state)
        {
            PlayerPrefs.SetInt("Sound", state ? 1 : 0);
        }
        
        private void OnMusicToggle(bool state)
        {
            PlayerPrefs.SetInt("Music", state ? 1 : 0);
        }

        private void OnCloseButtonClick()
        {
            gameObject.SetActive(false);
        }
    }
}
