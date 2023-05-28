using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReplayBtn : MonoBehaviour
{
    [SerializeField] List<Image> list_img_heart;
    [SerializeField] Color color_rest;

    void Start()
    {
        SettingHeart();
    }

    public void onClickReplay()
    {
        // heart ³²À½
        if (DataManager.instance.restPlay > 0)
        {
            DataManager.instance.ReduceRestPlay();
            SceneManager.LoadScene("Game");
        }
        else
        {
            Debug.Log("replayBtn");
            // ±¤°í

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
    }
}
