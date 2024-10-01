using UnityEngine;

namespace GameSystems.Scripts.UI
{
	public static class CanvasGroupExtensions
	{
		public static void SetVisibility(this CanvasGroup canvasGroup, bool visible)
		{
			canvasGroup.interactable = visible;
			canvasGroup.blocksRaycasts = visible;
			canvasGroup.alpha = visible ? 1f : 0f;
		}
        
		public static void Show(this CanvasGroup canvasGroup) => SetVisibility(canvasGroup, true);
		public static void Hide(this CanvasGroup canvasGroup) => SetVisibility(canvasGroup, false);
	}
}