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

    // �� �ʱ�ȭ. GameManager���� Block Object ���� �� ȣ��
    public void InitBlock(int idx, int rest, GameManager gm)
    {
        blocklist_idx = idx;
        restcount = rest;
        this.gm = gm;

        txt_count = GetComponentInChildren<TextMeshPro>();
        rigid_block = GetComponent<Rigidbody>();
       
        ChangeBlockSet();
    }

    // ���� ���¸� ��, �ؽ�Ʈ�� ǥ��
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
        // �÷��̾ ���� ���� -> Ƚ�� ����
        if (collision.gameObject.tag == "Player")
        {
            restcount--;
            ChangeBlockSet();
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        // �÷��̾ ����� ���� -> ���� Ƚ���� 0�̸� �߶�
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
