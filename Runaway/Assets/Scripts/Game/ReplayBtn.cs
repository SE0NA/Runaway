using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReplayBtn : MonoBehaviour
{
    public static int rest = 3;

    [SerializeField] List<Image> list_img_heart;
    [SerializeField] Color color_rest;

    void Start()
    {
        SettingHeart();
    }

    public void onClickReplay()
    {
        // heart ³²À½
        if (rest > 0)
        {
            rest--;
            DataManager.instance.SetRestHeart(ReplayBtn.rest);

            SceneManager.LoadScene("Game");
        }
        else
        {
            // ±¤°í

            rest = 3;
            SettingHeart();
        }
    }

    void SettingHeart()
    {
        for (int i = 0; i < rest; i++)
        {
            list_img_heart[i].color = color_rest;
        }
    }

    public void PlayAds()
    {
        Debug.Log("±¤°í");
    }
}
