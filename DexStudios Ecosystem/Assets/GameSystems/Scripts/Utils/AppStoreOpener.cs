using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AppStoreOpener
{
    public static void OpenMetaMaskAppStore()
    {
#if UNITY_ANDROID
        string playStoreURL = "https://play.google.com/store/apps/details?id=io.metamask";
        Application.OpenURL(playStoreURL);
#elif UNITY_IOS
        string appStoreURL = "itms-apps://itunes.apple.com/app/metamask/id1438144202";
        Application.OpenURL(appStoreURL);
#else
        Debug.LogWarning("Opening MetaMask app store is only supported on Android and iOS.");
#endif
    }
}
