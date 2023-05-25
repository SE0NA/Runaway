using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelBtn : MonoBehaviour
{
    int mylevel;
    [SerializeField] HomeManager homeManager;

    [SerializeField] TextMeshProUGUI txt_title;
    [SerializeField] Slider slider;

    [SerializeField] List<Sprite> list_img;
    [SerializeField] Image img;

    public void Init(int level, string title, HomeManager hm)
    {
        mylevel = level;
        txt_title.text = title;
        homeManager = hm;

        img.sprite = list_img[mylevel - 1];

        slider.interactable = false;
        SettingSlider();
    }

    void SettingSlider()
    {
        
        int total = DataManager.instance.stagedata.levellist[mylevel-1].stagelist.Length;
        int clear = 0;
        foreach (Stage s in DataManager.instance.stagedata.levellist[mylevel-1].stagelist)
            if (s.clear) clear++;

        slider.maxValue = total;
        slider.value = clear;
    }

    public void SelectThisLevel()
    {
        DataManager.instance.selectedLevel = mylevel;
        homeManager.ActiveStageList();
    }
}
