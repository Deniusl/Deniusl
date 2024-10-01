#if UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameSystems.Scripts;
using GameSystems.Scripts.Constants;
using UnityEngine;
using NearClientUnity;
using NearClientUnity.Utilities;
using NearClientUnity.KeyStores;
using NearClientUnity.Providers;
using UnityEngine.Networking;

public class NearPersistentManager : MonoBehaviour
{
    public static NearPersistentManager Instance { get; private set; }
    public WalletAccount WalletAccount { get; set; }
    public Near Near { get; set; }

    private void NearPersistentManagerSetup()
    {
        if (AvailableNetworks.CurrentNetworkData is null)
            return;
        var nearNetwork = AvailableNetworks.CurrentNetworkData.network;
        Near = new Near(config: new NearConfig()
        {
            NetworkId = nearNetwork,
            NodeUrl = AvailableNetworks.CurrentNetworkData.networkUrl,
            ProviderType = ProviderType.JsonRpc,
            SignerType = SignerType.InMemory,
            KeyStore = new InMemoryKeyStore(),
            ContractName = AvailableNetworks.CurrentNetworkData.contracts[0],
            WalletUrl = nearNetwork == "mainnet" ? "https://wallet.near.org" : "https://testnet.mynearwallet.com/"
        });
        WalletAccount = new WalletAccount(
        Near,
        "",
        new AuthService(),
        new AuthStorage());
        
        
    }
    
    private void Start()
    {
        // NearPersistentManagerSetup();
    }

    public void NearPersistentManagerUpdate()
    {
        NearPersistentManagerSetup();
    }
    
    private async UniTask TestAllEndpoints()
        {
            await GetRequest("motodex");
            await GetRequest("motoDEXgetNFTs");
            await GetRequest("motoDEXgetBalance");
            await GetRequest("motoDEXgetPrices");
            await GetRequest("motoDEXTokenIdsAndOwners");
            await GetRequest("motodex/purchasewithbalance");
            await GetRequest("motodex/appstoreReceipt");
            await GetRequest("motodexHealth");
        }
    
    private async UniTask GetRequest(string endpoint)
    {
        string url = MotoDexApi.Api + endpoint;
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            var operation = await request.SendWebRequest().ToUniTask();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error ({endpoint}): " + request.error);
            }
            else
            {
                Debug.Log($"Response ({endpoint}): " + request.downloadHandler.text);
            }
        }
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            NearPersistentManagerSetup();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

public class AuthService : IExternalAuthService
{
    public bool OpenUrl(string url)
    {
        try
        {
            Application.OpenURL(url);
            return true;
        }
        catch
        {
            return false;
        }
    }
}

public class AuthStorage : IExternalAuthStorage
{  
    public bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    public void Add(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save();
    }

    public string GetValue(string key)
    {
        return PlayerPrefs.GetString(key);
    }

    public void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
        PlayerPrefs.Save();
    }
}
#endif