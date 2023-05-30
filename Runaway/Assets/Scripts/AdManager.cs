using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;

public class AdManager : MonoBehaviour
{
    [Header("Ads")]
    [SerializeField] private string _adUnitID;
    private RewardedAd rewardedAd;

    [Header("GameObject")]
    [SerializeField] GameObject obj_toast;


    public void Start()
    {   
        MobileAds.RaiseAdEventsOnUnityMainThread = true;

        // Initialize the Google Mobile Ads SDK
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });

        LoadAd();
    }


    public void LoadAd()
    {
        // Clean up the old ad before loading a new one.
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        Debug.Log("Loading the ad.");

        // create out request used to load the ad.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // send the request to load the ad
        RewardedAd.Load(_adUnitID, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            // if error is not null, the load request failed.
            if (error != null || ad == null)
            {
                
                Debug.LogError("Rewarded ad failed to load an ad");
                return;
            }

            Debug.Log("Rewarded ad loaded with response");

            rewardedAd = ad;
        });
    }

    public bool ShowAd()
    {
        bool showed = false;
        if(rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                // TODO: reward the user
                DataManager.instance.ChargeRestPlay();
                showed = true;
                rewardedAd.Destroy();
                // 광고 미리 로드
                RegisterReloadHandler(rewardedAd);
            });
        }
        else
        {
            // 보여줄 광고 없음
            obj_toast.gameObject.SetActive(true);
            // obj_toast.Toast();
        }
        return showed;
    }

    private void RegisterReloadHandler(RewardedAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            LoadAd();
        };
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            LoadAd();
        };
    }
}
