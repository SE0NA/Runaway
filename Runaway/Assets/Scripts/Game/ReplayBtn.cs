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
    [SerializeField] Sprite img_replay;

    [SerializeField] GameObject obj_charge;

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
            FindObjectOfType<UIManager>().ActiveCharge();
        }
    }

    public void SettingHeart()
    {
        for (int i = 0; i < DataManager.instance.restPlay; i++)
        {
            list_img_heart[i].color = color_rest;
        }
    }
}
