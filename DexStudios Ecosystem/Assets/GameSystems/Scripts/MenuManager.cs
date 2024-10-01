using System;
using Cysharp.Threading.Tasks.Triggers;
using GameSystems.Scripts;
using GameSystems.Scripts.Constants;
using GameSystems.Scripts.Providers;
using SceneManagement;
using Services;
using UI;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject levelSelect;

    // Start is called before the first frame update
    // private void Start()
    // {
    //     //levelSelect.SetActive(false);
    //     settingsMenu.SetActive(false);
    //     // for(int i=2; i<=PlayerPrefs.GetInt("Completed Level", 1)+2; i++)
    //     // {
    //     //     lockedLevels[i-2].SetActive(false);
    //     // }
    // }

    // Update is called once per frame

    
    private void Awake()
    {
        // if (GameObject.FindGameObjectsWithTag("MenuManager").Length > 1)
        //     Destroy(gameObject);
        if (levelSelect == null)
            levelSelect = GameObject.FindWithTag("SelectWindow");
    }
    
    public void LeaveAccount()
    {
        Debug.Log($"#debug #account #switchScene LeaveAccount, remove players prefs NetworkIndex, Account, etc");
        PriceForTypeManager.ClearPriceForType();
        if (levelSelect == null)
            levelSelect = GameObject.FindWithTag("SelectWindow");
        PlayerPrefs.DeleteKey(PlayerPrefsKeys.Account);
        PlayerPrefs.DeleteKey(PlayerPrefs.GetString("default_wallet_auth_key"));
        PlayerPrefs.DeleteKey("default_wallet_auth_key");
        PlayerPrefs.DeleteKey(PlayerPrefsKeys.NetworkIndex);
        PlayerPrefs.Save();
#if UNITY_ANDROID
        Destroy(NearPersistentManager.Instance.gameObject);
#elif UNITY_WEBGL
        Web3GL.WebGLReload(true);
#endif
        PlayerServiceProvider.NftItemAction = null;
        if (GameObject.FindGameObjectsWithTag("Canvas").Length > 0)
            Destroy(GameObject.FindGameObjectWithTag("Canvas").gameObject);
        AllServices.Get<SceneLoader>().LoadMainMenu();
    }

    public void NewGame()
    {
        levelSelect.SetActive(true);
        //settingsMenu.SetActive(false);
    }
}
