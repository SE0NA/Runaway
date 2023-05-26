using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject ui_set1;
    [SerializeField] GameObject ui_set2;
    [SerializeField] GameObject ui_clear;
    [SerializeField] GameObject ui_failed;

    [SerializeField] GameObject btn_next;
    [SerializeField] TextMeshProUGUI txt_clear_stage;
    [SerializeField] TextMeshProUGUI txt_failed_stage;
    [SerializeField] TextMeshProUGUI txt_failed_reason;

    [Header("AudioClips")]
    [SerializeField] AudioClip clip_menu;
    [SerializeField] AudioClip clip_btn;
    [SerializeField] AudioClip clip_failed;
    [SerializeField] AudioClip clip_completed;

    public bool activeMenu = false;
    Player player;
    AudioSource audioSource;


    void Start()
    {
        ui_set1.SetActive(true);
        ui_set2.SetActive(false);
        ui_clear.SetActive(false);
        ui_failed.SetActive(false);

        player = FindObjectOfType<Player>();

        // 다음 레벨이 없으면 다음 스테이지 버튼 제거
        if (DataManager.instance.selectedStage == DataManager.instance.stagedata.levellist[DataManager.instance.selectedLevel - 1].stagelist.Length)
            Destroy(btn_next);

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        string str_stage = DataManager.instance.selectedLevel.ToString() + " - " + DataManager.instance.selectedStage.ToString();
        txt_clear_stage.text = str_stage;
        txt_failed_stage.text = str_stage;
    }

    public void Click_Menu()
    {
        if (!player.isMoving)
        {
            audioSource.PlayOneShot(clip_menu);

            ui_set1.SetActive(false);
            activeMenu = true;
            ui_set2.SetActive(true);
        }
    }

    public void Click_Close_Set2()
    {
        audioSource.PlayOneShot(clip_btn);
        ui_set2.SetActive(false);
        activeMenu = false;
        ui_set1.SetActive(true);

        // 게임 UI 실행
    }

    public void Click_Home()
    {
        audioSource.PlayOneShot(clip_btn);

        SceneManager.LoadScene("Home");
    }

    public void Click_Next()
    {
        audioSource.PlayOneShot(clip_btn);

        DataManager.instance.selectedStage++;
        SceneManager.LoadScene("Game");
    }

    public void ActiveResult(GameManager.Result result)
    {
        activeMenu = true;
        Handheld.Vibrate(); // 진동

        if (result == GameManager.Result.clear)
        {
            audioSource.PlayOneShot(clip_completed);
            ui_set1.SetActive(false);
            ui_clear.SetActive(true);
        }
        else
        {
            audioSource.PlayOneShot(clip_failed);
            ui_set1.SetActive(false);

            if (result == GameManager.Result.dead)
                txt_failed_reason.text = "You fell!";
            else
                txt_failed_reason.text = "There are blocks left!";

            ui_failed.SetActive(true);
        }
    }
}
