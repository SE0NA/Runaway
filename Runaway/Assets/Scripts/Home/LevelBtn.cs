using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBtn : MonoBehaviour
{
    public int mylevel;
    [SerializeField] HomeManager homeManager;

    [SerializeField] Slider slider;

    public void Start()
    {
        slider.enabled = false;
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
