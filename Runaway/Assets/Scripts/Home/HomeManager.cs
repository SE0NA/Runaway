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
    [SerializeField] Slider slider_volume;
    [SerializeField] Toggle toggle_haptic;

    [Header("LevelList")]
    [SerializeField] GameObject obj_prf_levelbtn;
    [SerializeField] Transform trans_level_content;

    [Header("Sound")]
    [SerializeField] AudioClip audioclip_btn;
    AudioSource audioSource;
    [SerializeField] AudioMixer mixer;

    void Awake()
    {
        DataManager.instance.LoadLevelData();
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        SettingLevelList();

        DataManager.instance.isHaptic = PlayerPrefs.GetFloat("haptic", 1) == 1 ? true : false;
        DataManager.instance.restPlay = PlayerPrefs.GetInt("restPlay", 3);

        mixer.SetFloat("sfx", PlayerPrefs.GetFloat("sfx", -20f));
    }

    void SettingLevelList()
    {
        foreach(Level l in DataManager.instance.leveldata.levellist){
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
        obj_stageList.SetActive(true);
        obj_stageList.GetComponent<HomeListManager>().SettingStageList();
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
        slider_volume.value = PlayerPrefs.GetFloat("sfx", -20f);
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
        PlayerPrefs.SetFloat("sfx", slider_volume.value);
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

        SceneManager.LoadScene("Home");
    }

    public void Slider_Set_SFX()
    {
        if (slider_volume.value < -40f) mixer.SetFloat("sfx", -80f);
        else    mixer.SetFloat("sfx", slider_volume.value);
    }
    public void Toggle_Set_Haptic()
    {
        DataManager.instance.isHaptic = toggle_haptic.isOn;
    }


}
