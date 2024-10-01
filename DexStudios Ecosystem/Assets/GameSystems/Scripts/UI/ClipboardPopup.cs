using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Assets.GameSystems.Scripts.UI
{
    internal class ClipboardPopup : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _rect = null;

        [SerializeField]
        private float _cycleDuration = 1.5f;
        [SerializeField]
        private float _stayDuration = 1.5f;
        [SerializeField]
        private int _framerate = 24;
        [SerializeField]
        private bool _useTargetFramerate = default;
        [SerializeField]
        private float _padding = 0f;

        private void Awake()
        {
            if (_rect == null) _rect = GetComponent<RectTransform>();
            if (_useTargetFramerate) _framerate = Application.targetFrameRate;
        }

        public async void PerformCycle()
        {
            gameObject.SetActive(true);

            await Show();
            await UniTask.Delay(TimeSpan.FromSeconds(_stayDuration));
            await Hide();
        }

        private async UniTask Show()
        {
            Vector3 finalPosition = _rect.anchoredPosition;
            finalPosition.y += _rect.rect.height + _padding;

            await _rect.DOAnchorPos(finalPosition, _cycleDuration).SetEase(Ease.InOutQuad).AsyncWaitForCompletion();
        }

        private async UniTask Hide()
        {
            Vector3 finalPosition = _rect.anchoredPosition;
            finalPosition.y -= _rect.rect.height + _padding;

            await _rect.DOAnchorPos(finalPosition, _cycleDuration).SetEase(Ease.InOutQuad).AsyncWaitForCompletion();
        }
    }
}
