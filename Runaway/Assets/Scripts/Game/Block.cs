using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Block : MonoBehaviour
{
    // Info of this block
    int restcount;
    int blocklist_idx;

    // Content of this block
    Rigidbody rigid_block;
    [SerializeField] List<Material> block_mat;
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
        rigid_block = GetComponent<Rigidbody>();
       
        ChangeBlockSet();
    }

    // 블럭의 상태를 색, 텍스트로 표시
    void ChangeBlockSet()
    {
        // Material
        gameObject.GetComponent<MeshRenderer>().material = block_mat[restcount];

        // Text
        if (DataManager.instance.isNumbering && restcount > 0)
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

    private void OnCollisionEnter(Collision collision)
    {
        // 플레이어에 의해 밟힘 -> 횟수 감소
        if (collision.gameObject.tag == "Player")
        {
            restcount--;
            ChangeBlockSet();
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        // 플레이어가 블록을 나감 -> 남은 횟수가 0이면 추락
        if (collision.gameObject.tag == "Player" && restcount <= 0)
        {
            rigid_block.useGravity = true;
            rigid_block.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "destroyzone")
        {
            Destroy(gameObject);
        }
    }
}
