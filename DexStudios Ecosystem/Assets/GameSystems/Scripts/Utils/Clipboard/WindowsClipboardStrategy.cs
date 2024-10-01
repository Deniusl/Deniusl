using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameSystems.Scripts.Utils.Clipboard
{
    internal class WindowsClipboardStrategy : IClipboardStrategy
    {
        private static PropertyInfo _systemCopyBufferProperty = null;
        
        #if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void CopyToClipboard(string text);
        #endif

        public WindowsClipboardStrategy()
        {
            _systemCopyBufferProperty ??= GetSystemCopyBufferProperty();
        }

        public string Get()
        {
            PropertyInfo copyBufferProperty = _systemCopyBufferProperty ?? GetSystemCopyBufferProperty();
            var value = copyBufferProperty.GetValue(null, null);
            return value.ToString();
            //return GUIUtility.systemCopyBuffer;
        }

        public void Set(string text)
        {
            Debug.Log($"#debug #copy #text {text}");
            
#if UNITY_EDITOR
            PropertyInfo copyBufferProperty = _systemCopyBufferProperty ?? GetSystemCopyBufferProperty();
            copyBufferProperty.SetValue(null, text, null);
            //GUIUtility.systemCopyBuffer = text;
#elif UNITY_WEBGL
            CopyToClipboard(text);
#endif
        }

        private PropertyInfo GetSystemCopyBufferProperty()
        {
            Type T = typeof(GUIUtility);
            var systemCopyBufferProperty = T.GetProperty("systemCopyBuffer", BindingFlags.Static | BindingFlags.Public);
            if (systemCopyBufferProperty == null)
            {
                systemCopyBufferProperty =
                    T.GetProperty("systemCopyBuffer", BindingFlags.Static | BindingFlags.NonPublic);
            }

            if (systemCopyBufferProperty == null)
            {
                throw new AccessViolationException(
                    "Can't access property 'systemCopyBuffer' of type 'GUIUtility'");
            }
            return systemCopyBufferProperty;
        }
    }
}
