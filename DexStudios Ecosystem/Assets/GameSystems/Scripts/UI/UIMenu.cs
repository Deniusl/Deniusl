using UnityEngine;


namespace GameSystems.Scripts.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIMenu : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private CanvasGroup CanvasGroup => _canvasGroup ??= GetComponent<CanvasGroup>();

        protected GameManager _gameManager;
        

        public UIMenu Initialize(GameManager gameManager)
        {
            if (gameManager == null) return this;

            _gameManager = gameManager;
            gameObject.SetActive(true);
            OnInitialize();
            
            return this;
        }

        public virtual void Show() => CanvasGroup.Show();
        public virtual void Hide() => CanvasGroup.Hide();
        
        protected abstract void OnInitialize();
    }
}