using System;
using System.Collections.Generic;
using GameSystems.Scripts.Constants;
using SceneManagement;
using Services;
using UI;
using UnityEngine;


public class MainMenuAllTime : MonoBehaviour, IUpdateService
{
    [field: SerializeField]
    public SelectLevelWindow SelectLevelWindow { get; private set; }

    [SerializeField]
    private string _editorAccount = "0xa907Cb2211BE82Fef3592b4490C556b7E52B7809";

    [SerializeField] private Curtain _curtain;

    public static MainMenuAllTime Instance;

    private List<IUpdatable> _updatableList;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

#if UNITY_EDITOR
            SetAccountInEditor();
#endif
            SelectLevelWindow.Init();
        }        
        else
        {
            Destroy(gameObject);
            return;
        }

        _updatableList = new List<IUpdatable>();
        
        CreateServices();
    }

    [ContextMenu("Init")]
    public void Reinitialize()
    {
        SelectLevelWindow.Init();
    }

    private void CreateServices()
    {
        AllServices.RegistrateService<IUpdateService>(this);
        AllServices.RegistrateService(_curtain);
        AllServices.RegistrateService(new AssetProviderService());
        AllServices.RegistrateService(new SceneLoader(AllServices.Get<IUpdateService>(), _curtain));
        IAudioMixerService audioMixerService = new AudioMixerService();
        AllServices.RegistrateService(audioMixerService);
    }

    private void SetAccountInEditor()
    {
        if (string.IsNullOrEmpty(_editorAccount))
            throw new NullReferenceException("_editorAccount value is not assigned");

        PlayerPrefs.SetString(PlayerPrefsKeys.Account, _editorAccount);
    }

    public void AddUpdatable(IUpdatable updatable)
    {
        _updatableList.Add(updatable);
    }

    private void Update()
    {
        if (_updatableList == null) return;
        
        for (int i = 0; i < _updatableList.Count; i++)
        {
            _updatableList?[i].Update();
        }
    }
}
