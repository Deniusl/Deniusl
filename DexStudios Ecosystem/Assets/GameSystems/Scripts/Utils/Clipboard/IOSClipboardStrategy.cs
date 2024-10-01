using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameSystems.Scripts.Utils.Clipboard
{
    internal class IOSClipboardStrategy : IClipboardStrategy
    {
#if UNITY_IOS
        [DllImport("__Internal")]
        static extern void SetText_(string str);
        [DllImport("__Internal")]
        static extern string GetText_();

        public string Get()
        {
            return GetText_();
        }

        public void Set(string text)
        {
            if (Application.platform != RuntimePlatform.OSXEditor)
            {
                SetText_(text);
            }
        }
#else
        public string Get()
        {
            throw new NotImplementedException();
        }

        public void Set(string text)
        {
            throw new NotImplementedException();
        }
#endif
    }
}
