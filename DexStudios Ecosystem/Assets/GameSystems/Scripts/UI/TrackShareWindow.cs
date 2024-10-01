using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameSystems.Scripts.UI
{
    public class TrackShareWindow : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _statusTMP = null;
        [SerializeField]
        private Button _shareButton = null;
        [SerializeField]
        private Button _closeButton = null;
        [SerializeField]
        private Button _closeBackgroundButton = null;

        [SerializeField]
        private ClipboardPopup _popup = null;

        private void Start()
        {
            _closeButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
            });
            _closeBackgroundButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
            });

            _shareButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);

                _popup.PerformCycle();
            });
        }
    }
}
