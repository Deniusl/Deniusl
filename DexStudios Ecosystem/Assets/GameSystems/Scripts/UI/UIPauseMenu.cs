using UnityEngine;
using UnityEngine.UI;


namespace GameSystems.Scripts.UI
{
    public class UIPauseMenu: UIMenu
    {
        [SerializeField] protected Button _returnButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _resumeButton;

        
        protected override void OnInitialize()
        {
            _returnButton.onClick.AddListener(_gameManager.MainMenu);
            _restartButton.onClick.AddListener(_gameManager.Restart);
            _resumeButton.onClick.AddListener(_gameManager.Resume);
        }
    }
}