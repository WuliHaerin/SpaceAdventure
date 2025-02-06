using UnityEngine;
using TMPro;
using System.Collections;

public class GameOver : MonoBehaviour
{
    public int continuePrice;

    public RocketController rocketController;
    public GameObject menu;
    public TextMeshProUGUI priceText;
    public Animation notEnough;
    public LevelLoader levelLoader;
    [HideInInspector]
    public bool crashed;

    public static GameOver instance;

    private Animation anim;

    public bool isInvincible;

    public GameObject CountDownObj;


    void Start()
    {
        instance = this;
        anim = this.GetComponent<Animation>();
        priceText.text = continuePrice.ToString();
        crashed = false;
    }

    // When player crashes to obstacle.
    public void Crashed()
    {
        // Used to disable pause/resume function when player crashed.
        crashed = true;
        rocketController.Crashed();
        // Play game over window open animation.
        anim.Play("Game-Over-In");
        // Disable game menu gameobject with all buttons.
        menu.SetActive(false);
        AdManager.ShowInterstitialAd("1lcaf5895d5l1293dc",
    () => {
        Debug.Log("--插屏广告完成--");

    },
    (it, str) => {
        Debug.LogError("Error->" + str);
    });
    }




    // If player selects continue button.
    public void Continue()
    {
        //// If player has enough money to continue.
        //if(Wallet.GetAmount() >= continuePrice)
        //{
        //    // Subract continue price from player wallet.
        //    Wallet.SetAmount(Wallet.GetAmount() - continuePrice);
        //    // Used to check if following game if player has selected continue option.
        //    Score.continueGame = true;
        //    // Load game scene.
        //    levelLoader.LoadLevel(1);
        //}
        //else
        //{
        //    //Play not enough money animation.
        //    notEnough.Play("Not-Enough-In");
        //}
        AdManager.ShowVideoAd("192if3b93qo6991ed0",
            (bol) => {
                if (bol)
                {
                    //Score.continueGame = true;
                    rocketController.Crashed();
                    anim.Play("GameOver-Out");
                    menu.SetActive(true);
                    StartCoroutine("Invincible");
                    rocketController.Resume();

                    AdManager.clickid = "";
                    AdManager.getClickid();
                    AdManager.apiSend("game_addiction", AdManager.clickid);
                    AdManager.apiSend("lt_roi", AdManager.clickid);


                }
                else
                {
                    StarkSDKSpace.AndroidUIManager.ShowToast("观看完整视频才能获取奖励哦！");
                }
            },
            (it, str) => {
                Debug.LogError("Error->" + str);
                //AndroidUIManager.ShowToast("广告加载异常，请重新看广告！");
            });


    }

    public IEnumerator Invincible()
    {
        isInvincible = true;
        CountDownObj.SetActive(true);
        yield return new WaitForSeconds(3);
        CountDownObj.SetActive(false);
        isInvincible = false;
    }
}
