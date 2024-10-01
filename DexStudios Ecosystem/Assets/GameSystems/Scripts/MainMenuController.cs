using SceneManagement;
using UnityEngine;
using System;
using DIContainerLib;
using GameSystems.Scripts.Providers;
using PPValues;
using Services;


public class MainMenuController : MonoBehaviour
{
    [SerializeField] private LevelPreset _startTutorialLevelPreset;
    [SerializeField] private LevelPreset _mainTutorialLevelPreset;
    [SerializeField] private LevelPreset[] _levelPresets;
    [SerializeField] private AudioSource backgroundMusic;

    private MenuMusicController _menuMusicController;

    private void Awake()
    {
        new DIServiceCollection().GenerateContainer();
        
        _menuMusicController = new MenuMusicController(backgroundMusic);
    }

    public void SelectLevel(int levelIndex)
    {
        if (_levelPresets.Length > levelIndex)
            LevelServiceProvider.LevelService.LevelPreset = _levelPresets[levelIndex];
    }

    public void LoadSelectedLevel(Action onLoaded = null)
    {
        if (LevelServiceProvider.LevelService.LevelPreset == null)
            return;
        
        _menuMusicController.SetBackgroundMusicState(false);
        AllServices.Get<SceneLoader>().LoadScene(LevelServiceProvider.LevelService.LevelPreset.LevelBundleId, onLoaded);
    }

    public void LoadTutorialLevel(Action onLoaded)
    {
        var tutorialLevelPreset = PlayerPrefsValues.IsStartTutorialPassed
            ? _mainTutorialLevelPreset
            : _startTutorialLevelPreset;

        LevelServiceProvider.LevelService.LevelPreset = tutorialLevelPreset;
        LevelServiceProvider.LevelService.IsTutorial = true;
        
        _menuMusicController.SetBackgroundMusicState(false);
        AllServices.Get<SceneLoader>().LoadScene(tutorialLevelPreset.LevelBundleId, onLoaded);
    }
}