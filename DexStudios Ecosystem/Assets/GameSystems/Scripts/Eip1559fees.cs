using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Cysharp.Threading.Tasks;
using GameSystems.Scripts;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Network = GameSystems.Scripts.Network;


#if UNITY_WEBGL && !UNITY_EDITOR
public class Eip1559 : MonoBehaviour
{
    public static async UniTask<string> Eip1559feesRequest()
    {
        var url =  "https://gasstation-mainnet.matic.network/v2";
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        await webRequest.SendWebRequest();
        Debug.Log(webRequest.downloadHandler.text);
        var _eip1559fees = JsonConvert.DeserializeObject<Eip1559fees>(webRequest.downloadHandler.text);
        var maxFee = Mathf.Round((float) _eip1559fees.fast?.Last().Value).ToString();
        Debug.Log(maxFee);
        return maxFee;
    }
}
#endif