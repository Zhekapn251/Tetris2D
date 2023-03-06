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
    private int _numberOfMinutes = 60 * 5;
    private float _timer = 0;
    private bool _isTimeForAds;
    private bool _isLoadedAds;
    public bool showAds;
    
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
        _timer += Time.deltaTime;
        if ((int)_timer>_numberOfMinutes)
        {
            _isTimeForAds = true;
            _timer = 0;
        }
    }

    public void ShowAdsToUser()
    {
        if (_isLoadedAds && _isTimeForAds && showAds)
        {
            showAds = false;
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
        LoadInterstitialAd();
    }
    public void LoadInterstitialAd()
    {
        Advertisement.Load("Interstitial_Android", this);
    }
    
    
    public void OnUnityAdsAdLoaded(string placementId)
    {
        _isLoadedAds = true;
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
        _musicManager.musicAudioSource.mute = true;
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
        _musicManager.musicAudioSource.mute = false;
        _isLoadedAds = false;
        _isTimeForAds = false;
        _timer = 0;
        LoadInterstitialAd();
    }


    private void OnDestroy()
    {
        EventsManager.OneWinMenuBtnClicked -= ShowAdsToUser;
    }
}