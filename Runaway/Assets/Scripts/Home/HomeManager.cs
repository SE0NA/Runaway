using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class HomeManager : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] GameObject obj_start;
    [SerializeField] GameObject obj_levelList;
    [SerializeField] GameObject obj_stageList;
    [SerializeField] GameObject obj_set;
    [SerializeField] GameObject obj_delete;

    [Header("Setting")]
    [SerializeField] Slider slider_bgm;
    [SerializeField] Slider slider_sfx;
    [SerializeField] Toggle toggle_haptic;

    [Header("LevelList")]
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
        obj_set.SetActive(false);
    }
    public void BackFromLevelList()
    {
        obj_levelList.SetActive(false);
        obj_start.SetActive(true);
    }

    public void ActiveStageList()
    {
        obj_start.SetActive(false);
        obj_levelList.SetActive(false);
        obj_stageList.GetComponent<HomeListManager>().SettingStageList(DataManager.instance.selectedLevel);
        obj_stageList.SetActive(true);
        obj_set.SetActive(false);
    }

    public void ActiveSetting()
    {
        SettingWithSet();

        obj_start.SetActive(false);
        obj_set.SetActive(true);
    }
    void SettingWithSet()
    {
        // ����
        slider_bgm.value = PlayerPrefs.GetFloat("BGM", -20f);
        slider_sfx.value = PlayerPrefs.GetFloat("SFX", -20f);
        toggle_haptic.isOn = PlayerPrefs.GetInt("haptic", 1) == 1 ? true : false;
    }

    public void CloseSetting()
    {
        // �� ����
        SavePlayerPrefs();

        obj_set.SetActive(false);
        obj_start.SetActive(true);
    }

    public void SavePlayerPrefs()
    {
        PlayerPrefs.SetFloat("BGM", slider_bgm.value);
        PlayerPrefs.SetFloat("SFX", slider_sfx.value);
        PlayerPrefs.SetInt("haptic", toggle_haptic.isOn ? 1 : 0);
    }

    public void ActiveDeletePop()
    {
        obj_set.SetActive(false);
        obj_delete.SetActive(true);
    }
    public void CloseDeletePop()
    {
        obj_delete.SetActive(false);
        obj_set.SetActive(true);
    }
    public void DeleteData()
    {
        DataManager.instance.FileDelete();

        SettingWithSet();   // slider playerprefs �ʱ�ȭ ����
        AudioManager.instance.SetBGMVolume(slider_bgm.value);
        AudioManager.instance.SetSFXVolume(slider_sfx.value);
        AudioManager.instance.SetHaptic(toggle_haptic.isOn);

        CloseDeletePop();
    }

    public void Slider_Set_BGM()
    {
        AudioManager.instance.SetBGMVolume(slider_bgm.value);
    }
    public void Slider_Set_SFX()
    {
        AudioManager.instance.SetSFXVolume(slider_sfx.value);
    }
    public void Toggle_Set_Haptic()
    {
        AudioManager.instance.SetHaptic(toggle_haptic.isOn);
    }


}
