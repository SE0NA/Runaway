using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;

public class AdManager : MonoBehaviour
{
    [Header("Test")]
    public bool test_mode = false;

    private static AdManager instance = null;
    private string _adUnitID = "ca-app-pub-4969644002755220/1434492590"; // 실제 리워드 
    private RewardedAd rewardedAd;



    public void Start()
    {  
        if(test_mode == true)
        {
            _adUnitID = "ca-app-pub-3940256099942544/5224354917";
            Debug.Log("testMode");
        }

        if (instance == null)
        {
            // Initialize the Google Mobile Ads SDK
            MobileAds.Initialize((InitializationStatus initStatus) =>
            {
                // This callback is called once the MobileAds SDK is initialized.
            });

            LoadAd();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
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

    public void ShowAd()
    {
        if(rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                // TODO: reward the user
                DataManager.instance.ChargeRestPlay();
                rewardedAd.Destroy();

                // 홈에서 광고 재생 후
                if (SceneManager.GetActiveScene().name == "Home")
                {
                    FindObjectOfType<HomeManager>().FinishAd();
                }
                else if(SceneManager.GetActiveScene().name == "Game")
                {
                    FindObjectOfType<UIManager>().FinishAd();
                }

                // 광고 미리 로드
                LoadAd();
            });
        }
    }
}
