using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameSystems.Scripts.Utils.Clipboard
{
    internal class ClipboardManager
    {
        private IClipboardStrategy _defaultStrategy = null;

        public ClipboardManager()
        {
#if UNITY_WEBGL
            _defaultStrategy = new WindowsClipboardStrategy();
#elif UNITY_IOS
            _defaultStrategy = new IOSClipboardStrategy();
#elif UNITY_ANDROID
            _defaultStrategy = new AndroidClipboardStrategy();
#endif
        }

        public void Copy(IClipboardStrategy strategy, string text)
        {
            strategy.Set(text);
        }

        public void Copy(string text)
        {
            Debug.Log($"#debug #copy #text {text}");
            _defaultStrategy.Set(text);
        }

        public string Paste(IClipboardStrategy strategy)
        {
            return strategy.Get();
        }
    }
}
