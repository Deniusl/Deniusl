using System;
using System.Collections.Generic;
using GameSystems.Scripts;
using GameSystems.Scripts.Providers;
using GameSystems.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
	public class UIGameMenu : UIMainMenu
	{
		[SerializeField] private SpritesPreset _pillsIcons;
		[SerializeField] private Image playerIcon;
		[SerializeField] private Text playerName;

		[SerializeField] private Image levelIcon;
		[SerializeField] private Text levelName;

		[SerializeField] private Text levelTime;

		[SerializeField] private Image healthIcon;
		[SerializeField] private TMP_Text healthValue;

		[SerializeField] private TMP_Text positionValue;

		private int _intHealthValue;
		private string _seconds;
		private string _minutes;


		protected override void OnInitialize()
		{
			base.OnInitialize();

			UpdatePlayerAndLevelViews();
			UpdatePlayerHealth();
			UpdateLevelTime();
		}

		private void UpdatePlayerAndLevelViews()
		{
			if (_gameManager == null) return;
			
			playerName.text = _gameManager.Player.Preset.Name;
			playerIcon.sprite = _gameManager.Player.Preset.Icon;
			levelIcon.sprite = _gameManager.CurrentLevelPreset.gameSceneIcon;
			levelName.text = _gameManager.CurrentLevelPreset.levelName;
		}

		private void Update()
		{
			if (_gameManager == null)
				return;
			
			UpdateLevelTime();
			UpdateRacePosition();
			UpdatePlayerHealth();
		}

		private void UpdateLevelTime()
		{
			if (_gameManager.Player.IsFinished) return;
			
			var timeInMin = Math.Truncate(_gameManager.LevelTime / 60);
			var timeInSec = Mathf.Round(_gameManager.LevelTime % 60);
			
			_minutes = timeInMin.ToString("0#");
			_seconds = timeInSec.ToString("0#");
			
			levelTime.text = $"{_minutes} : {_seconds}";
		}

		private void UpdateRacePosition()
		{
			if (_gameManager.PlayersRaceData == null) return;
			
			for (int racePosition = 0; racePosition < _gameManager.PlayersRaceData.Count; racePosition++)
			{
				if (_gameManager.PlayersRaceData[racePosition].player is Player)
				{
					positionValue.text = (racePosition + 1).ToString();
					break;
				}
			}
		}

		private void UpdatePlayerHealth()
		{
#if UNITY_ANDROID || UNITY_IOS
	    
			_intHealthValue = (int)PlayerServiceProvider.NftItemAction.CurrentHealthWei;
			healthIcon.sprite = SwitchHealthIcon();
			healthValue.text = _intHealthValue.ToString();
	    
			return;
#endif
			_intHealthValue = (int) Math.Ceiling((double)PlayerServiceProvider.NftItemAction.CurrentHealthWei / Const.Health.oneHpInWei);
			healthIcon.sprite = SwitchHealthIcon();
			healthValue.text = _intHealthValue.ToString();
		}

		private Sprite SwitchHealthIcon()
		{
			int index = _intHealthValue switch
			{
				<= 5 => 0,
				<= 10 => 1,
				<= 30 => 2,
				_ => 3
			};
	
			return _pillsIcons.sprites[index];
		}
	}
}
