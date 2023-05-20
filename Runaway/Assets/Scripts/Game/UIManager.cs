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
    [SerializeField] TextMeshProUGUI txt_failde_reason;

    void Start()
    {
        ui_set1.SetActive(true);
        ui_set2.SetActive(false);
        ui_clear.SetActive(false);
        ui_failed.SetActive(false);

        // 다음 레벨이 없으면 다음 스테이지 버튼 제거
        if (DataManager.instance.selectedStage == DataManager.instance.stagedata.levellist[DataManager.instance.selectedLevel - 1].stagelist.Length)
            Destroy(btn_next);
    }

    public void Click_Menu()
    {
        ui_set1.SetActive(false);
        ui_set2.SetActive(true);

        // 게임 UI 제한
    }

    public void Click_Close_Set2()
    {
        ui_set2.SetActive(false);
        ui_set1.SetActive(true);

        // 게임 UI 실행
    }

    public void Click_Home()
    {
        SceneManager.LoadScene("Home");
    }

    public void Click_Replay()
    {
        SceneManager.LoadScene("Game");
    }

    public void Click_Next()
    {
        DataManager.instance.selectedStage++;
        SceneManager.LoadScene("Game");
    }

    public void ActiveResult(GameManager.Result result)
    {
        if(result == GameManager.Result.clear)
        {
            ui_set1.SetActive(false);
            ui_clear.SetActive(true);
        }
        else
        {
            ui_set1.SetActive(false);

            if (result == GameManager.Result.dead)
                txt_failde_reason.text = "You were falled!";
            else
                txt_failde_reason.text = "The blocks are left!";

            ui_failed.SetActive(true);
        }
    }
}
