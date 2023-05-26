using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
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

    [Header("Sound")]
    [SerializeField] AudioClip audioclip_btn;
    AudioSource audioSource;

    void Awake()
    {
        DataManager.instance.LoadStageData();
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        SettingLevelList();
    }

    void SettingLevelList()
    {
        foreach(Level l in DataManager.instance.stagedata.levellist){
            GameObject newLevelBtn = Instantiate<GameObject>(obj_prf_levelbtn, trans_level_content);
            newLevelBtn.GetComponent<LevelBtn>().Init(l.level, l.title, this);
        }
    }

    void PlayBtnAudio()
    {
        audioSource.Play();
    }

    public void ActiveLevelList()
    {
        PlayBtnAudio();

        obj_start.SetActive(false);
        obj_levelList.SetActive(true);
        obj_stageList.SetActive(false);
        obj_set.SetActive(false);
    }
    public void BackFromLevelList()
    {
        PlayBtnAudio();

        obj_levelList.SetActive(false);
        obj_start.SetActive(true);
    }

    public void ActiveStageList()
    {
        PlayBtnAudio();

        obj_start.SetActive(false);
        obj_levelList.SetActive(false);
        obj_stageList.GetComponent<HomeListManager>().SettingStageList(DataManager.instance.selectedLevel);
        obj_stageList.SetActive(true);
        obj_set.SetActive(false);
    }

    public void ActiveSetting()
    {
        PlayBtnAudio();

        SettingWithSet();

        obj_start.SetActive(false);
        obj_set.SetActive(true);
    }
    void SettingWithSet()
    {
        // º¼·ý
        slider_bgm.value = PlayerPrefs.GetFloat("BGM", -20f);
        slider_sfx.value = PlayerPrefs.GetFloat("SFX", -20f);
        toggle_haptic.isOn = PlayerPrefs.GetInt("haptic", 1) == 1 ? true : false;
    }

    public void CloseSetting()
    {
        PlayBtnAudio();
        // °ª ÀúÀå
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
        PlayBtnAudio();

        obj_set.SetActive(false);
        obj_delete.SetActive(true);
    }
    public void CloseDeletePop()
    {
        PlayBtnAudio();

        obj_delete.SetActive(false);
        obj_set.SetActive(true);
    }
    public void DeleteData()
    {
        PlayBtnAudio();

        DataManager.instance.FileDelete();

        SettingWithSet();   // slider playerprefs ÃÊ±âÈ­ ¸ÂÃã
        AudioManager.instance.SetBGMVolume(slider_bgm.value);
        AudioManager.instance.SetSFXVolume(slider_sfx.value);
        AudioManager.instance.SetHaptic(toggle_haptic.isOn);

        SceneManager.LoadScene("Home");
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
