using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameSystems.Scripts.Utils.Clipboard
{
    internal class CopyTest : MonoBehaviour
    {
        public Button _copyButton = null;
        private string _text = "Test string for copying";
        private ClipboardManager _clipboardManager = null;

        private void Start()
        {
            _copyButton = GetComponent<Button>();
            _clipboardManager = new();

            _copyButton.onClick.AddListener(() =>
            {
                _clipboardManager.Copy(_text);
            });
        }
    }
}
