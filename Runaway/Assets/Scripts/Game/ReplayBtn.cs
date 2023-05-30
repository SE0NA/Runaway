using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReplayBtn : MonoBehaviour
{
    [SerializeField] List<Image> list_img_heart;
    [SerializeField] Color color_rest;

    [SerializeField] Image obj_btn_img;
    [SerializeField] Sprite img_ad;
    [SerializeField] Sprite img_replay;

    void Start()
    {
        SettingHeart();
    }

    public void onClickReplay()
    {
        // heart 남음
        if (DataManager.instance.restPlay > 0)
        {
            DataManager.instance.ReduceRestPlay();
            SceneManager.LoadScene("Game");
        }
        else
        {
            Debug.Log("replayBtn");
            // 광고

            DataManager.instance.ChargeRestPlay();

            SettingHeart();
        }
    }

    void SettingHeart()
    {
        for (int i = 0; i < DataManager.instance.restPlay; i++)
        {
            list_img_heart[i].color = color_rest;
        }

        RectTransform rt = obj_btn_img.GetComponent<RectTransform>();

        if (DataManager.instance.restPlay <= 0) // 광고 버튼
        {
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, 110);
            obj_btn_img.sprite = img_ad;
        }
        else        // 리플레이 버튼
        {
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, 140);
            obj_btn_img.sprite = img_replay;
        }
    }
}
