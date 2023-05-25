using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StageBtn : MonoBehaviour
{
    int stageNo;
    TextMeshProUGUI txt_no;
    Image img_back;
    Button this_btn;

    [SerializeField] Color color_clear;
    [SerializeField] Color color_unlock;
    [SerializeField] Color color_lock;

    public void InitBtn(int stageNo, bool isClear, bool unLock)
    {
        txt_no = gameObject.transform.GetComponentInChildren<TextMeshProUGUI>();
        img_back = GetComponent<Image>();
        this_btn = GetComponent<Button>();

        this.stageNo = stageNo;
        txt_no.text = stageNo.ToString();

        if (isClear)
            img_back.color = color_clear;
        else if (unLock)
            img_back.color = color_unlock;
        else
        {
            img_back.color = color_lock;
            this_btn.interactable = false;
        }

    }

    public void StartThisStage()
    {
        DataManager.instance.selectedStage = stageNo;

        Debug.Log("level: " + DataManager.instance.selectedLevel + " / stage: " + DataManager.instance.selectedStage);
        
        
        /*
        if(ReplayBtn.rest <= 0)
        {
        }
        */

        SceneManager.LoadScene("Game");
    }
}
