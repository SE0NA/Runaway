using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build.Content;
using UnityEditor.VersionControl;
using UnityEngine;

public class Block : MonoBehaviour
{
    // Info of this block
    int restcount;
    int blocklist_idx;

    // Content of this block
    Rigidbody rigid_block;
    [SerializeField] List<Color> blockcolors;
    [SerializeField] List<Color> txtcolors;
    TextMeshPro txt_count;

    // Extern
    GameManager gm;

    // 블럭 초기화. GameManager에서 Block Object 생성 시 호출
    public void InitBlock(int idx, int rest, GameManager gm)
    {
        blocklist_idx = idx;
        restcount = rest;
        this.gm = gm;

        txt_count = GetComponentInChildren<TextMeshPro>();
        ChangeBlockSet();
    }

    // 블럭의 상태를 색, 텍스트로 표시
    void ChangeBlockSet()
    {
     //   gameObject.GetComponent<Material>().color = blockcolors[restcount];
        if (restcount > 0)
        {
            txt_count.text = restcount.ToString();
            txt_count.color = txtcolors[restcount];
        }
        else
        {
            txt_count.text = "";
            gm.SetZeroBlock(blocklist_idx);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어에 의해 밟힘 -> 횟수 감소
        if (other.tag == "Player")
        {
            restcount--;
            ChangeBlockSet();
        }

        // 추락 후 제거 지점에 충돌
        else if (other.tag == "DestroyZone")
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // 플레이어가 블록을 나감 -> 남은 횟수가 0이면 추락
        if (other.tag == "Player" && restcount <= 0)
        {
            // 중력
        }
    }
}
