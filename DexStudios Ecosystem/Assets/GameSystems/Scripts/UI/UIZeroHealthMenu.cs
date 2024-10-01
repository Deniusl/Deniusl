using UnityEngine;
using UnityEngine.UI;


namespace GameSystems.Scripts.UI
{
    public class UIZeroHealthMenu: UIMenu
    {
        [SerializeField] protected Button _returnButton;

        
        protected override void OnInitialize()
        {
            _returnButton.onClick.AddListener(_gameManager.MainMenu);
        }
    }
}