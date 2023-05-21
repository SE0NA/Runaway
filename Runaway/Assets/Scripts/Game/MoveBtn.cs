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

    // Start is called before the first frame update
    void Start()
    {
        // 버튼 모양 이미지에 맞추기
        this.GetComponent<Image>().alphaHitTestMinimumThreshold = AlphaThreshold;

        player = FindObjectOfType<Player>();
        btn = GetComponent<Button>();
        btn.onClick.AddListener(BtnClick);
    }

    void BtnClick()
    {
        player.Move(dir);
    }
}
