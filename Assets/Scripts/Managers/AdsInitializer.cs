using System;
using UnityEngine;
using UnityEngine.Advertisements;
 
public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener,
    IUnityAdsShowListener
{
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private MusicManager _musicManager;
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] bool _testMode;
    private string _gameId;
    private int numberOfMinutes = 60 * 5;
    float timer = 0;
    private bool istimeForAds;
    bool isLoadedAds;
    public bool ShowAds;
    
    void Awake()
    {
        InitializeAds();
    }

    private void Start()
    {
        EventsManager.OneWinMenuBtnClicked += ShowAdsToUser;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if ((int)timer>numberOfMinutes)
        {
            istimeForAds = true;
            timer = 0;
        }

        
    }

    public void ShowAdsToUser()
    {
        if (isLoadedAds && istimeForAds && ShowAds)
        {
            ShowAds = false;
            Advertisement.Show("Interstitial_Android", this);
        }
    }

    public void InitializeAds()
    {
        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOSGameId
            : _androidGameId;
        Advertisement.Initialize(_gameId, _testMode, this);
    }
 
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
    public void OnInitializationComplete()
    {
        //Debug.Log("Unity Ads initialization complete.");
        LoadInterstitialAd();
    }
    public void LoadInterstitialAd()
    {
        Advertisement.Load("Interstitial_Android", this);
    }
    
    
    public void OnUnityAdsAdLoaded(string placementId)
    {
        isLoadedAds = true;
        //Debug.Log("Ads is loaded");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Unity Ads Load Failed: {placementId} {error.ToString()} - {message}");
    }
    
    
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Unity Ads Show Failed:{placementId} {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("Start showing Ads, stop Game activity");
        Time.timeScale = 0;
        _soundManager.soundsAudioSourse.mute = true;
        _musicManager.musicAudioSourse.mute = true;
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("Ads was clicked");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log("Showing Ads complete, resume Game activity");
        Debug.Log(showCompletionState);
        Time.timeScale = 1;
        _soundManager.soundsAudioSourse.mute = false;
        _musicManager.musicAudioSourse.mute = false;
        isLoadedAds = false;
        istimeForAds = false;
        timer = 0;
        LoadInterstitialAd();
        
    }


    private void OnDestroy()
    {
        EventsManager.OneWinMenuBtnClicked -= ShowAdsToUser;
    }
}