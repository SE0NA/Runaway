using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeManager : MonoBehaviour
{
    [SerializeField] GameObject obj_start;
    [SerializeField] GameObject obj_levelList;
    [SerializeField] GameObject obj_stageList;

    [SerializeField] GameObject obj_prf_levelbtn;
    [SerializeField] Transform trans_level_content;

    void Awake()
    {
        DataManager.instance.LoadStageData();
    }

    private void Start()
    {
        SettingLevelList();
    }

    void SettingLevelList()
    {
        foreach(Level l in DataManager.instance.stagedata.levellist){
            GameObject newLevelBtn = Instantiate<GameObject>(obj_prf_levelbtn, trans_level_content);
            newLevelBtn.GetComponent<LevelBtn>().Init(l.level, l.title, this);
        }
    }

    public void ActiveLevelList()
    {
        obj_start.SetActive(false);
        obj_levelList.SetActive(true);
        obj_stageList.SetActive(false);
    }

    public void ActiveStageList()
    {
        obj_start.SetActive(false);
        obj_levelList.SetActive(false);
        obj_stageList.GetComponent<HomeListManager>().SettingStageList(DataManager.instance.selectedLevel);
        obj_stageList.SetActive(true);
    }

}
