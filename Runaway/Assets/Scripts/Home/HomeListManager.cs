using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class HomeListManager : MonoBehaviour
{
    int thisLevel = 0;

    [SerializeField] GameObject obj_btn_proto;
    [SerializeField] GameObject obj_panel_for_list;

    [SerializeField] TextMeshProUGUI txt_level;



    public void SettingStageList(int level)
    {
        thisLevel = level;
        txt_level.text = DataManager.instance.stagedata.levellist[level - 1].title;

        // 기존 리스트 삭제
        foreach (Transform child in obj_panel_for_list.transform)
            Destroy(child.gameObject);

        foreach(Stage s in DataManager.instance.stagedata.levellist[level - 1].stagelist)
        {
            GameObject stageBtn = Instantiate<GameObject>(obj_btn_proto, obj_panel_for_list.transform);
            stageBtn.GetComponent<StageBtn>().InitBtn(s.stageNo, s.clear, s.unLock);
        }
    }
}
