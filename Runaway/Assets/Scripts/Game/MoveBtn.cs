using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveBtn : MonoBehaviour
{
    public Player.DIR dir;
    public float AlphaThreshold = 0.1f;
    Button btn;
    Player player;
    UIManager uimanager;

    // Start is called before the first frame update
    void Start()
    {
        // 버튼 모양 이미지에 맞추기
        this.GetComponent<Image>().alphaHitTestMinimumThreshold = AlphaThreshold;
        
        btn = GetComponent<Button>();

        player = FindObjectOfType<Player>();
        btn.onClick.AddListener(BtnClick);
        uimanager = FindObjectOfType<UIManager>();
    }

    void BtnClick()
    {
        if (!uimanager.activeMenu)
        {
            player.Move(dir);
        }
    }
}
