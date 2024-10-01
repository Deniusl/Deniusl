using UnityEngine;
using UnityEngine.UI;


namespace UI
{
	public class EngineTemperatureView : MonoBehaviour
	{
		[SerializeField] private GameObject green;
		[SerializeField] private GameObject yellow;
		[SerializeField] private GameObject red;
		[SerializeField] private Image fillBar;

		private EngineHeatManager _engineHeatManager;

		
		public void Initialize(EngineHeatManager EngineHeatManager)
		{
			OnDisable();
			
			_engineHeatManager = EngineHeatManager;
			
			if (_engineHeatManager != null) 
				_engineHeatManager.TemperatureChanged += OnTemperatureChanged;
		}

		private void OnDisable()
		{
			if (_engineHeatManager != null) 
				_engineHeatManager.TemperatureChanged -= OnTemperatureChanged;
		}
		
		private void OnTemperatureChanged(float value)
		{
			var engineTemperatureRatio = value / EngineHeatManager.EngineTemperatureMax;
			fillBar.fillAmount = engineTemperatureRatio;
			
			SetIndicatorStatus(engineTemperatureRatio);
		}

		private void SetIndicatorStatus(float ratio)
		{
			green.SetActive(ratio < 0.5f);
			yellow.SetActive(ratio is >= 0.5f and < 0.9f);
			red.SetActive(ratio >= 0.9f);
		}
		
	}
}