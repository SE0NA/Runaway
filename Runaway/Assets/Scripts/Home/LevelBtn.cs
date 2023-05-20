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

    public void Init(int level, string title, HomeManager hm)
    {
        mylevel = level;
        txt_title.text = title;
        homeManager = hm;

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
