using GameSystems.Scripts.UI;
using UnityEngine;


namespace UI
{
	public class UITutorialMenu : UIMainMenu
	{
		[Space]
		[SerializeField] private GameObject _label;


		private void Awake()
		{
			#if UNITY_WEBGL
				_label.SetActive(false);
			#else
				_label.SetActive(true);
			#endif
		}
	}
}