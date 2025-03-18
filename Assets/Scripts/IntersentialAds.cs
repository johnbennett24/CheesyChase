using System;
using UnityEngine;
using YandexMobileAds.Base;
using YandexMobileAds;

public class IntersentialAds : MonoBehaviour
{
    private const string AdUnitId = "MyUnitId";
    private InterstitialAdLoader interstitialAdLoader;
    private Interstitial interstitial;

    public void HandleInterstitialLoaded(object sender, InterstitialAdLoadedEventArgs args)
    {
        interstitial = args.Interstitial;

        interstitial.OnAdFailedToShow += HandleInterstitialFailedToShow;
        interstitial.OnAdDismissed += HandleInterstitialDismissed;
    }

    public void HandleInterstitialDismissed(object sender, EventArgs args)
    {
        DestroyInterstitial();

        RequestInterstitial();
    }

    public void HandleInterstitialFailedToShow(object sender, EventArgs args)
    {
        DestroyInterstitial();

        RequestInterstitial();
    }
    public void DestroyInterstitial()
    {
        if (interstitial != null)
        {
            interstitial.Destroy();
            interstitial = null;
        }
    }

    private void Awake()
    {
        SetupLoader();
        RequestInterstitial();
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        Context.Instance.StateChanged += OnStateChanged;
    }

    private void OnDisable()
    {
        Context.Instance.StateChanged -= OnStateChanged;
    }

    private void OnStateChanged(GameState state)
    {
        if(state == GameState.GameOver)
            ShowInterstitial();
    }

    private void SetupLoader()
    {
        interstitialAdLoader = new InterstitialAdLoader();
        interstitialAdLoader.OnAdLoaded += HandleInterstitialLoaded;
    }

    private void RequestInterstitial()
    {
        AdRequestConfiguration adRequestConfiguration = new AdRequestConfiguration.Builder(AdUnitId).Build();
        interstitialAdLoader.LoadAd(adRequestConfiguration);
    }

    private void ShowInterstitial()
    {
        if (interstitial != null)
        {
            interstitial.Show();
        }
    }
}
