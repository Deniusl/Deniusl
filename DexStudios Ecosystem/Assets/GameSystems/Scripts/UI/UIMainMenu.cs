using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.UI;


namespace GameSystems.Scripts.UI
{
	public abstract class UIMainMenu : UIMenu
	{
		[Header("HUD")]
		[SerializeField] private Text _countdownView;
		[SerializeField] private SpeedometerView _speedometerView;
		[SerializeField] private EngineTemperatureView _engineTemperatureView;
		[Header("Menus")]
		[SerializeField] private UIGameOver _gameOverMenu;
		[SerializeField] private UIZeroHealthMenu _zeroHealthMenu;
		[SerializeField] private UIPauseMenu _pauseMenu;
		
		public UIGameOver GameOverMenu => _gameOverMenu;
		public UIZeroHealthMenu ZeroHealthMenu => _zeroHealthMenu;
		public UIPauseMenu PauseMenu => _pauseMenu;
		public SpeedometerView SpeedometerView => _speedometerView;
		public EngineTemperatureView EngineTemperatureView => _engineTemperatureView;
		
		
		protected override void OnInitialize()
		{
			if (_countdownView != null) 
				_countdownView.gameObject.SetActive(false);
			
			if (_gameOverMenu != null)
				_gameOverMenu.Initialize(_gameManager).Hide();

			if (_zeroHealthMenu != null)
				_zeroHealthMenu.Initialize(_gameManager).Hide();

			if (_pauseMenu != null)
				_pauseMenu.Initialize(_gameManager).Hide();
		}

		public void StartCountdown(int timeInSeconds)
		{
			if (_countdownView == null) return;
				
			StartCoroutine(CountdownCoroutine(timeInSeconds));
		}

		private IEnumerator CountdownCoroutine(int timeInSeconds)
		{
			AudioManager.Instance.Play("Start");
			
			_countdownView.gameObject.SetActive(true);

			for (int i = timeInSeconds; i > 0; i--)
			{
				_countdownView.text = i.ToString();
				yield return new WaitForSeconds(0.75f);
			}
    
			_countdownView.text = "GO!";
			yield return new WaitForSeconds(1f);

			_countdownView.gameObject.SetActive(false);
		}
	}
}