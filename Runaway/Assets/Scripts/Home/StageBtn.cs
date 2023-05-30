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

    [SerializeField] List<Sprite> sprite_clear;
    [SerializeField] List<Sprite> sprite_unlock;
    [SerializeField] List<Sprite> sprite_lock;

    public void InitBtn(int stageNo, bool isClear, bool unLock)
    {
        txt_no = gameObject.transform.GetComponentInChildren<TextMeshProUGUI>();
        img_back = GetComponent<Image>();
        this_btn = GetComponent<Button>();

        this.stageNo = stageNo;
        txt_no.text = stageNo.ToString();

        SpriteState spritestate = new SpriteState();
        spritestate = this_btn.spriteState;

        if (isClear)
        {
            img_back.sprite = sprite_clear[0];
            spritestate.pressedSprite = sprite_clear[1];
        }
        else if (unLock)
        {
            img_back.sprite = sprite_unlock[0];
            spritestate.pressedSprite = sprite_unlock[1];
        }
        else
        {
            this_btn.interactable = false;
            img_back.sprite = sprite_lock[0];
            spritestate.pressedSprite = sprite_lock[1];
        }
        this_btn.spriteState = spritestate;
    }

    public void StartThisStage()
    {
        DataManager.instance.selectedStage = stageNo;

        Debug.Log("level: " + DataManager.instance.selectedLevel + " / stage: " + DataManager.instance.selectedStage);
        
        if(DataManager.instance.restPlay <= 0)
        {
            FindObjectOfType<HomeManager>().ShowChargePanel();
        }
        else
        {
            DataManager.instance.ReduceRestPlay();

            SceneManager.LoadScene("Game");
        }
    }
}
