using GoogleMobileAds.Api;
using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

public class GoogleAdMob : MonoBehaviour
{
    public Button btn_AdMode;
    public SR_CreditStore SR_creditStore;
    public GameObject LoadingScreen;

#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
  private string _adUnitId = "unused";
#endif

    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
            //LoadRewardedAd();
        });
    }

    private RewardedAd rewardedAd;

    /// <summary>
    /// Loads the rewarded ad.
    /// </summary>
    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }
        btn_AdMode.interactable = false;
        LoadingScreen.SetActive(true);
        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(_adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {

                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    btn_AdMode.interactable = true;
                    LoadingScreen.SetActive(false);
                    DOTween.Restart("NetWordErrorOn");
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                rewardedAd = ad;

                RegisterEventHandlers(rewardedAd);

                ShowRewardedAd();
            });
    }

    public void ShowRewardedAd()
    {

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
             
                if (reward.Amount >= 10.0f)
                {
                    Debug.Log("리워드 성공!");

                    LoadingScreen.SetActive(false);
                    btn_AdMode.interactable = true;
                    SR_creditStore.BuyDiamond();

                }
            });
        }
    }

   

    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            //LoadRewardedAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
            //LoadRewardedAd();
        };
    }

    #region 전면 광고
    const string frontTestID = "ca-app-pub-3940256099942544/8691691433";
    const string frontID = "";
    InterstitialAd frontAd;

    public void LoadAdAndMoveToMainScene()
    {
        if (GameObject.Find("EnemySpawnManager").GetComponent<SR_EnemySpawnManager>() == null) { LoadMainScene(); return; }
        if(GameObject.Find("EnemySpawnManager").GetComponent<SR_EnemySpawnManager>().startGame)
        {
            LoadFrontAd();
        }
        else { LoadMainScene(); }
    }

    void LoadFrontAd()
    {
        if (frontAd != null)
        {
            frontAd.Destroy();
            frontAd = null;
        }
        // create our request used to load the ad.
        var adRequest = new AdRequest();
        LoadingScreen.SetActive(true);
        Time.timeScale = 0;
        // send the request to load the ad.
        InterstitialAd.Load(frontTestID, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Time.timeScale = 1;
                    SR_SceneManager.instance.LoadScene_MainMenu();
                    Debug.LogError("frontTest ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }
                Debug.Log("frontTest ad loaded with response : "
                          + ad.GetResponseInfo());
                frontAd = ad;
                frontAd.OnAdFullScreenContentClosed += LoadMainScene;
                ShowFrontAd();
            });
    }

    void ShowFrontAd()
    {
        Debug.Log("전면광고 시작");
        if (frontAd != null && frontAd.CanShowAd())
        {
            frontAd.Show();
        }
    }
    void LoadMainScene()
    {
        Time.timeScale =1;
        SR_SceneManager.instance.LoadScene_MainMenu();
    }
    #endregion

  
}