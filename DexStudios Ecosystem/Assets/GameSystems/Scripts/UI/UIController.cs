using Cysharp.Threading.Tasks;
using GameSystems.Scripts.Providers;
using UI;
using UnityEngine;


namespace GameSystems.Scripts.UI
{
	public class UIController : MonoBehaviour
	{
		[Header("Level")]
		[SerializeField] private CanvasGroup _levelCanvasGroup;
		[SerializeField] private UIMainMenu _levelMainMenu;
		[Header("Tutorial")]
		[SerializeField] private CanvasGroup _tutorialCanvasGroup;
		[SerializeField] private UIMainMenu _tutorialMainMenu;

		private UIMainMenu _uiMainMenu;
		private UIGameOver _uiGameOver;
		private UIZeroHealthMenu _uiZeroHealthMenu;
		private UIPauseMenu _uiPauseMenu;
		
		public EngineTemperatureView EngineTemperatureView => _uiMainMenu.EngineTemperatureView;
		public SpeedometerView SpeedometerView => _uiMainMenu.SpeedometerView;

		
		public void Initialize(GameManager gameManager)
		{
			_levelCanvasGroup.Hide();
			_tutorialCanvasGroup.Hide();

			if (LevelServiceProvider.LevelService.IsTutorial)
			{
				_uiMainMenu = _tutorialMainMenu;
				_tutorialCanvasGroup.Show();
			}
			else
			{
				_uiMainMenu = _levelMainMenu;
				_levelCanvasGroup.Show();
			}

			InitializeUiElements(gameManager);
		}
		
		private void InitializeUiElements(GameManager gameManager)
		{
			if (_uiMainMenu != null)
			{
				_uiMainMenu.Initialize(gameManager);
				
				_uiGameOver = _uiMainMenu.GameOverMenu;
				_uiZeroHealthMenu = _uiMainMenu.ZeroHealthMenu;
				_uiPauseMenu = _uiMainMenu.PauseMenu;
			}
		}
		
		public void StartCountdown(int timeInSeconds)
		{
			if (_uiMainMenu != null) 
				_uiMainMenu.StartCountdown(timeInSeconds);
		}
		
		public void ShowGameOverMenu()
		{
			if (_uiGameOver != null)
				_uiGameOver.Show();
			
			if (_uiMainMenu != null)
				_uiMainMenu.Hide();
		}

		public async UniTask TryUpdateHighScoreTableAsync()
		{
			if (LevelServiceProvider.LevelService.IsTutorial) return;
			
			if (_uiGameOver != null)
				await _uiGameOver.UpdateHighScoreTable();
		}
		
		public void ShowPauseMenu()
		{
			if (_uiPauseMenu != null)
				_uiPauseMenu.Show();
		}
		
		public void HidePauseMenu()
		{
			if (_uiPauseMenu != null)
				_uiPauseMenu.Hide();
		}

		public void OpenZeroHealthMenu()
		{
			if (_uiZeroHealthMenu != null)
				_uiZeroHealthMenu.Show();
		}
	}
}
