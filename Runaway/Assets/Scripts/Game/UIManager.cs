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
    [SerializeField] GameObject btn_charge;
    [SerializeField] GameObject ui_charge;

    [SerializeField] List<GameObject> list_extra_next_btn;

    [SerializeField] GameObject btn_next;
    [SerializeField] TextMeshProUGUI txt_paused_stage;
    [SerializeField] TextMeshProUGUI txt_clear_stage;
    [SerializeField] TextMeshProUGUI txt_failed_stage;
    [SerializeField] TextMeshProUGUI txt_failed_reason;
    [SerializeField] TextMeshProUGUI txt_playtime;

    [Header("AudioClips")]
    [SerializeField] AudioClip clip_menu;
    [SerializeField] AudioClip clip_btn;
    [SerializeField] AudioClip clip_failed;
    [SerializeField] AudioClip clip_completed;

    public bool activeMenu = false;
    Player player;
    AudioSource audioSource;

    enum State { pause, failed, clear}
    State thisState;

    void Start()
    {
        ui_set1.SetActive(true);
        ui_set2.SetActive(false);
        ui_clear.SetActive(false);
        ui_failed.SetActive(false);
        btn_charge.SetActive(false);
        ui_charge.SetActive(false);

        player = FindObjectOfType<Player>();

        // 다음 레벨이 없으면 다음 스테이지 버튼 제거
        if (DataManager.instance.selectedStage >= DataManager.instance.stagedata.stagelist.Length)
            Destroy(btn_next);

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        string str_stage = DataManager.instance.selectedLevel.ToString() + " - " + DataManager.instance.selectedStage.ToString();
        txt_paused_stage.text = str_stage;
        txt_clear_stage.text = str_stage;
        txt_failed_stage.text = str_stage;
        txt_playtime.text = DataManager.instance.restPlay.ToString();

        if (!DataManager.instance.stagedata.stagelist[DataManager.instance.selectedStage - 1].clear)
            foreach (GameObject btn in list_extra_next_btn)
                Destroy(btn);
    }

    public void Click_Menu()
    {
        if (!player.isMoving)
        {
            audioSource.PlayOneShot(clip_menu);

            ui_set1.SetActive(false);
            activeMenu = true;
            btn_charge.SetActive(true);
            ui_set2.SetActive(true);
            ui_clear.SetActive(false);
            ui_failed.SetActive(false);
            ui_charge.SetActive(false);

            thisState = State.pause;
        }
    }

    public void Click_Close_Set2()
    {
        audioSource.PlayOneShot(clip_btn);
        btn_charge.SetActive(false);
        ui_set2.SetActive(false);
        activeMenu = false;
        ui_set1.SetActive(true);
        ui_clear.SetActive(false);
        ui_failed.SetActive(false);
        ui_charge.SetActive(false);

        // 게임 UI 실행
    }

    public void Click_PlayTimeBtn()
    {
        audioSource.PlayOneShot(clip_btn);
        if (DataManager.instance.restPlay <= 0)
        {
            ActiveCharge();
        }
    }

    public void Click_Home()
    {
        audioSource.PlayOneShot(clip_btn);

        SceneManager.LoadScene("Home");
    }
    public void Click_Replay()
    {
        if(DataManager.instance.restPlay > 0)
        {
            DataManager.instance.ReduceRestPlay();
            SceneManager.LoadScene("Game");
        }
        else
        {
            ActiveCharge();
        }
    }

    public void Click_Next()
    {
        audioSource.PlayOneShot(clip_btn);

        if (DataManager.instance.restPlay > 0)
        {
            DataManager.instance.ReduceRestPlay();
            DataManager.instance.selectedStage++;
            SceneManager.LoadScene("Game");
        }
        else
        {
            ActiveCharge();
        }
    }

    public void ActiveCharge()
    {
        ui_set1.SetActive(false);
        ui_set2.SetActive(false);
        ui_clear.SetActive(false);
        ui_failed.SetActive(false);
        btn_charge.SetActive(false);
        ui_charge.SetActive(true);
    }
    public void CloseCharge()
    {
        ui_set1.SetActive(false);
        ui_charge.SetActive(false);
        btn_charge.SetActive(true);
        
        if(thisState == State.pause)
        {
            ui_set2.SetActive(true);
            ui_clear.SetActive(false);
            ui_failed.SetActive(false);
        }
        else if(thisState == State.failed)
        {
            ui_set2.SetActive(false);
            ui_clear.SetActive(false);
            ui_failed.SetActive(true);
        }
        else if(thisState == State.clear)
        {
            ui_set2.SetActive(false);
            ui_clear.SetActive(true);
            ui_failed.SetActive(false);
        }
    }
    public void Click_ChargeBtn()
    {
        FindObjectOfType<AdManager>().ShowAd();
    }
    public void FinishAd()
    {
        txt_playtime.text = DataManager.instance.restPlay.ToString();
        CloseCharge();
    }

    public void ActiveResult(GameManager.Result result)
    {
        activeMenu = true;
        btn_charge.SetActive(true);
        if (DataManager.instance.isHaptic)
        {
            Handheld.Vibrate();
        }

        if (result == GameManager.Result.clear)
        {
            audioSource.PlayOneShot(clip_completed);
            ui_set1.SetActive(false);
            ui_clear.SetActive(true);
            thisState = State.clear;
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
            thisState = State.failed;
        }
    }
}
