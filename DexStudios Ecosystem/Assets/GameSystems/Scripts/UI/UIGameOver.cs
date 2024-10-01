using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


namespace GameSystems.Scripts.UI
{
    public class UIGameOver : UIMenu
    {
        [SerializeField] protected Button _returnButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private TextView _earnTextView;
        [SerializeField] private TextView _minimalTextView;
        [SerializeField] private HighscoreTable _highScoreTable;

        
        protected override void OnInitialize()
        {
            _returnButton.onClick.AddListener(_gameManager.MainMenu);
            restartButton.onClick.AddListener(_gameManager.Restart);
        }

        public async UniTask UpdateHighScoreTable()
        {
            Debug.Log(2);
            if (_highScoreTable != null)
                await _highScoreTable.SetHighScores(_gameManager);
        }

        public override void Show()
        {
            if (_earnTextView != null) 
                _earnTextView.Text = $"YOU EARNED {_gameManager.TutorialReward} COINS";

            if (_minimalTextView != null) 
                _minimalTextView.Text = $"MINIMAL MOTO PRICE IS  {GetMinimumPrice()} COINS";

#if UNITY_WEBGL
            DisableWebGLElements();
#endif
            
            base.Show();
        }
        
        private int GetMinimumPrice()
        {
            return MainMenuAllTime.Instance.SelectLevelWindow.MotoChooseNftComponent.NftItemComponents
                .Where(nftItemComponent => nftItemComponent.TypeEnum.IsMoto() && nftItemComponent.PriceInCoins.HasValue)
                .Select(nftItemComponent => nftItemComponent.PriceInCoins.Value)
                .Min();
        }

        private void DisableWebGLElements()
        {
            if (_earnTextView != null) 
                _earnTextView.gameObject.SetActive(false);
            
            if (_minimalTextView != null) 
                _minimalTextView.gameObject.SetActive(false);
        }
    }
}
