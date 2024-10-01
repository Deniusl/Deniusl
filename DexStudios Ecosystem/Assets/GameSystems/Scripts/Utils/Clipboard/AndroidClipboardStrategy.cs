using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameSystems.Scripts.Utils.Clipboard
{
    internal class AndroidClipboardStrategy : IClipboardStrategy
    {
        public string Get()
        {
            return GetClipboardService().Call<string>("getText");
        }

        public void Set(string text)
        {
            GetClipboardService().Call("setText", text);
        }

        private AndroidJavaObject GetClipboardService()
        {
            var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            var context = new AndroidJavaClass("android.content.Context");
            var service = context.GetStatic<AndroidJavaObject>("CLIPBOARD_SERVICE");
            return activity.Call<AndroidJavaObject>("getSystemService", service);
        }
    }
}
