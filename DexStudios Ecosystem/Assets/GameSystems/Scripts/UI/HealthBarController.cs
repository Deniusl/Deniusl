using GameSystems.Scripts.Characters.PlayerCharacter;
using UnityEngine;


namespace GameSystems.Scripts.UI
{
    public class HealthBarController : MonoBehaviour
    {
        [SerializeField] private HealthBarView _healthBarView;

        [Header("Back View Settings")]
        [SerializeField] private Vector3 _backLocalPosition = new (0f, 175f, 0f);
        [SerializeField] private Vector3 _backLocalScale = new (0.5f, 0.5f, 1f);

        [Header("Side View Settings")]
        [SerializeField] private Vector3 _sideLocalPosition = new (0f, 175f, 0f);
        [SerializeField] private Vector3 _sideLocalScale = new (1f, 1f, 1f);

        private Transform _cameraTransform;
        private Transform _healthBarTransform;

        
        public void Initialize(Player player)
        {
            _cameraTransform = player.Camera.transform;
            _healthBarTransform = _healthBarView.transform;

            player.ViewChanged += OnViewChanged;

            UpdateHealthCanvas();
        }

        public void UpdateHealthBarView(float fillAmount)
        {
            if (_healthBarView != null)
            {
                _healthBarView.UpdateView(fillAmount);
            }
            else
            {
                Debug.LogError("HealthBarView is not assigned.");
            }
        }

        private void OnViewChanged(ViewType viewType)
        {
            UpdateHealthCanvas(viewType);
        }

        private void UpdateHealthCanvas(ViewType viewType = default)
        {
            if (viewType == ViewType.Back)
            {
                _healthBarTransform.localPosition = _backLocalPosition;
                _healthBarTransform.localScale = _backLocalScale;
            }
            else if (viewType == ViewType.Side)
            {
                _healthBarTransform.localPosition = _sideLocalPosition;
                _healthBarTransform.localScale = _sideLocalScale;
            }
        }

        private void Update()
        {
            if (_healthBarTransform != null && _cameraTransform != null)
            {
                _healthBarTransform.LookAt(_cameraTransform, _cameraTransform.up);

                var eulerAngles = _healthBarTransform.localEulerAngles;

                eulerAngles.z = 0;
                _healthBarTransform.localEulerAngles = eulerAngles;
            }
        }
    }
}
