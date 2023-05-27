using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class HomeListManager : MonoBehaviour
{
    [SerializeField] GameObject obj_btn_proto;
    [SerializeField] GameObject obj_panel_for_list;

    [SerializeField] TextMeshProUGUI txt_level;



    public void SettingStageList()
    {
        txt_level.text = DataManager.instance.leveldata.levellist[DataManager.instance.selectedLevel - 1].title;

        // 기존 리스트 삭제
        foreach (Transform child in obj_panel_for_list.transform)
            Destroy(child.gameObject);

        bool unLock = true;
        foreach (Stage s in DataManager.instance.stagedata.stagelist)
        {
            GameObject stageBtn = Instantiate<GameObject>(obj_btn_proto, obj_panel_for_list.transform);
            stageBtn.GetComponent<StageBtn>().InitBtn(s.stageNo, s.clear, unLock);
            unLock = s.clear;
        }
    }

}
