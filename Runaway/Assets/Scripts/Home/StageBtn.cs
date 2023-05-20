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
    
    public void InitBtn(int stageNo, bool isClear, bool unLock)
    {
        txt_no = gameObject.transform.GetComponentInChildren<TextMeshProUGUI>();
        img_back = GetComponent<Image>();

        this.stageNo = stageNo;
        txt_no.text = stageNo.ToString();

        if (isClear)
            img_back.color = Color.cyan;
        else if (unLock)
            img_back.color = Color.white;
        else
            img_back.color = Color.gray;
    }

    public void StartThisStage()
    {
        DataManager.instance.selectedStage = stageNo;

        Debug.Log("level: " + DataManager.instance.selectedLevel + " / stage: " + DataManager.instance.selectedStage);

        SceneManager.LoadScene("Game");
    }
}
