using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeManager : MonoBehaviour
{
    [SerializeField] GameObject obj_start;
    [SerializeField] GameObject obj_levelList;
    [SerializeField] GameObject obj_stageList;

    void Awake()
    {
        DataManager.instance.LoadStageData();
    }


    public void ActiveLevelList()
    {
        obj_start.SetActive(false);
        obj_levelList.SetActive(true);
    }

    public void ActiveStageList()
    {
        obj_levelList.SetActive(false);
        obj_stageList.GetComponent<HomeListManager>().SettingStageList(DataManager.instance.selectedLevel);
        obj_stageList.SetActive(true);
    }

}
